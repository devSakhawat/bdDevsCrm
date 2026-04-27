using System.Collections.Concurrent;
using System.Globalization;
using System.Text;
using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Extensions;
using bdDevs.Shared.Records.CRM;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services.CRM;
using Domain.Entities.Entities.CRM;
using Domain.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.CRM;

internal sealed class CrmCommissionService : ICrmCommissionService
{
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> InvoiceLocks = new();
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmCommissionService> _logger;
    private readonly IConfiguration _configuration;

    public CrmCommissionService(IRepositoryManager repository, ILogger<CrmCommissionService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<CrmCommissionDto> CreateAsync(CreateCrmCommissionRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmCommissionRecord));
        var draft = await BuildCommissionDraftAsync(record.ApplicationId, record.AgentId, cancellationToken);
        if (await _repository.CrmCommissions.ExistsAsync(x => x.ApplicationId == record.ApplicationId && x.AgentId == draft.AgentId && !x.IsDeleted, cancellationToken))
            throw new DuplicateRecordException("CrmCommission", "ApplicationId");

        var entity = new CrmCommission
        {
            ApplicationId = record.ApplicationId,
            UniversityId = record.UniversityId > 0 ? record.UniversityId : draft.UniversityId,
            AgentId = record.AgentId ?? draft.AgentId,
            BranchId = record.BranchId > 0 ? record.BranchId : draft.BranchId,
            StudentNameSnapshot = string.IsNullOrWhiteSpace(record.StudentNameSnapshot) ? draft.StudentNameSnapshot : record.StudentNameSnapshot.Trim(),
            UniversityNameSnapshot = string.IsNullOrWhiteSpace(record.UniversityNameSnapshot) ? draft.UniversityNameSnapshot : record.UniversityNameSnapshot.Trim(),
            TuitionFeeBase = record.TuitionFeeBase > 0 ? record.TuitionFeeBase : draft.TuitionFeeBase,
            CommissionRate = record.CommissionRate > 0 ? record.CommissionRate : draft.CommissionRate,
            CommissionType = record.CommissionType > 0 ? record.CommissionType : draft.CommissionType,
            GrossAmount = record.GrossAmount > 0 ? record.GrossAmount : draft.GrossAmount,
            ScholarshipDeduction = record.ScholarshipDeduction > 0 ? record.ScholarshipDeduction : draft.ScholarshipDeduction,
            NetAmount = record.NetAmount > 0 ? record.NetAmount : draft.NetAmount,
            Currency = string.IsNullOrWhiteSpace(record.Currency) ? draft.Currency : record.Currency.Trim().ToUpperInvariant(),
            ExchangeRate = record.ExchangeRate > 0 ? record.ExchangeRate : draft.ExchangeRate,
            NetAmountBdt = record.NetAmountBdt > 0 ? record.NetAmountBdt : draft.NetAmountBdt,
            Status = record.Status == 0 ? (byte)1 : record.Status,
            DueDate = record.DueDate ?? draft.DueDate,
            PaidDate = record.PaidDate,
            PaidAmount = record.PaidAmount,
            InvoiceNo = string.IsNullOrWhiteSpace(record.InvoiceNo) ? null : record.InvoiceNo.Trim(),
            Notes = string.IsNullOrWhiteSpace(record.Notes) ? draft.Notes : record.Notes.Trim(),
            IsDeleted = record.IsDeleted,
            CreatedDate = record.CreatedDate,
            CreatedBy = record.CreatedBy,
            UpdatedDate = record.UpdatedDate,
            UpdatedBy = record.UpdatedBy
        };

        if (entity.Status >= 3 && string.IsNullOrWhiteSpace(entity.InvoiceNo))
            entity.InvoiceNo = await GenerateInvoiceNoInternalAsync(entity.UniversityId, DateTime.UtcNow, cancellationToken);
        if (entity.Status == 4 && !entity.PaidDate.HasValue)
            entity.PaidDate = DateTime.UtcNow;
        if (entity.Status == 4 && !entity.PaidAmount.HasValue)
            entity.PaidAmount = entity.NetAmount;

        int newId = await _repository.CrmCommissions.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        entity.CommissionId = newId;
        return entity.MapTo<CrmCommissionDto>();
    }

    public async Task<CrmCommissionDto> UpdateAsync(UpdateCrmCommissionRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmCommissionRecord));
        var existing = await _repository.CrmCommissions.CommissionAsync(record.CommissionId, false, cancellationToken)
            ?? throw new NotFoundException("CrmCommission", "CommissionId", record.CommissionId.ToString());
        var draft = await BuildCommissionDraftAsync(record.ApplicationId, record.AgentId, cancellationToken);
        existing.ApplicationId = record.ApplicationId;
        existing.UniversityId = record.UniversityId > 0 ? record.UniversityId : draft.UniversityId;
        existing.AgentId = record.AgentId ?? draft.AgentId;
        existing.BranchId = record.BranchId > 0 ? record.BranchId : draft.BranchId;
        existing.StudentNameSnapshot = string.IsNullOrWhiteSpace(record.StudentNameSnapshot) ? draft.StudentNameSnapshot : record.StudentNameSnapshot.Trim();
        existing.UniversityNameSnapshot = string.IsNullOrWhiteSpace(record.UniversityNameSnapshot) ? draft.UniversityNameSnapshot : record.UniversityNameSnapshot.Trim();
        existing.TuitionFeeBase = record.TuitionFeeBase > 0 ? record.TuitionFeeBase : draft.TuitionFeeBase;
        existing.CommissionRate = record.CommissionRate > 0 ? record.CommissionRate : draft.CommissionRate;
        existing.CommissionType = record.CommissionType > 0 ? record.CommissionType : draft.CommissionType;
        existing.GrossAmount = record.GrossAmount > 0 ? record.GrossAmount : draft.GrossAmount;
        existing.ScholarshipDeduction = record.ScholarshipDeduction >= 0 ? record.ScholarshipDeduction : draft.ScholarshipDeduction;
        existing.NetAmount = record.NetAmount > 0 ? record.NetAmount : draft.NetAmount;
        existing.Currency = string.IsNullOrWhiteSpace(record.Currency) ? draft.Currency : record.Currency.Trim().ToUpperInvariant();
        existing.ExchangeRate = record.ExchangeRate > 0 ? record.ExchangeRate : draft.ExchangeRate;
        existing.NetAmountBdt = existing.NetAmount * existing.ExchangeRate;
        existing.Status = record.Status == 0 ? existing.Status : record.Status;
        existing.DueDate = record.DueDate ?? existing.DueDate ?? draft.DueDate;
        existing.PaidDate = record.PaidDate ?? existing.PaidDate;
        existing.PaidAmount = record.PaidAmount ?? existing.PaidAmount;
        existing.InvoiceNo = string.IsNullOrWhiteSpace(record.InvoiceNo) ? existing.InvoiceNo : record.InvoiceNo.Trim();
        existing.Notes = string.IsNullOrWhiteSpace(record.Notes) ? existing.Notes : record.Notes.Trim();
        existing.IsDeleted = record.IsDeleted;
        existing.CreatedDate = record.CreatedDate;
        existing.CreatedBy = record.CreatedBy;
        existing.UpdatedDate = record.UpdatedDate ?? DateTime.UtcNow;
        existing.UpdatedBy = record.UpdatedBy;

        if (existing.Status >= 3 && string.IsNullOrWhiteSpace(existing.InvoiceNo))
            existing.InvoiceNo = await GenerateInvoiceNoInternalAsync(existing.UniversityId, DateTime.UtcNow, cancellationToken);
        if (existing.Status == 4 && !existing.PaidDate.HasValue)
            existing.PaidDate = DateTime.UtcNow;
        if (existing.Status == 4 && !existing.PaidAmount.HasValue)
            existing.PaidAmount = existing.NetAmount;

        _repository.CrmCommissions.UpdateByState(existing);
        await _repository.SaveAsync(cancellationToken);
        return existing.MapTo<CrmCommissionDto>();
    }

    public async Task DeleteAsync(DeleteCrmCommissionRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.CommissionId <= 0) throw new BadRequestException("Invalid delete request!");
        var entity = await _repository.CrmCommissions.CommissionAsync(record.CommissionId, true, cancellationToken)
            ?? throw new NotFoundException("CrmCommission", "CommissionId", record.CommissionId.ToString());
        entity.IsDeleted = true;
        entity.UpdatedDate = DateTime.UtcNow;
        _repository.CrmCommissions.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmCommissionDto> CommissionAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => (await _repository.CrmCommissions.CommissionAsync(id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CrmCommission", "CommissionId", id.ToString())).MapTo<CrmCommissionDto>();

    public async Task<IEnumerable<CrmCommissionDto>> CommissionsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmCommissions.CommissionsAsync(trackChanges, cancellationToken);
        var active = entities.Where(x => !x.IsDeleted);
        return active.Any() ? active.MapToList<CrmCommissionDto>() : Enumerable.Empty<CrmCommissionDto>();
    }

    public async Task<IEnumerable<CrmCommissionDto>> CommissionsByApplicationIdAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmCommissions.CommissionsByApplicationIdAsync(applicationId, trackChanges, cancellationToken);
        var active = entities.Where(x => !x.IsDeleted);
        return active.Any() ? active.MapToList<CrmCommissionDto>() : Enumerable.Empty<CrmCommissionDto>();
    }

    public async Task<IEnumerable<CrmCommissionDto>> CommissionsByAgentIdAsync(int agentId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmCommissions.CommissionsByAgentIdAsync(agentId, trackChanges, cancellationToken);
        var active = entities.Where(x => !x.IsDeleted);
        return active.Any() ? active.MapToList<CrmCommissionDto>() : Enumerable.Empty<CrmCommissionDto>();
    }

    public async Task<GridEntity<CrmCommissionDto>> CommissionsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT CommissionId, ApplicationId, UniversityId, AgentId, BranchId, StudentNameSnapshot, UniversityNameSnapshot, TuitionFeeBase, CommissionRate, CommissionType, GrossAmount, ScholarshipDeduction, NetAmount, Currency, ExchangeRate, NetAmountBdt, Status, DueDate, PaidDate, PaidAmount, InvoiceNo, Notes, IsDeleted, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmCommission WHERE IsDeleted = 0";
        return await _repository.CrmCommissions.AdoGridDataAsync<CrmCommissionDto>(sql, options, "CommissionId DESC", string.Empty, cancellationToken);
    }

    public async Task<CrmCommissionDto> ChangeStatusAsync(ChangeCrmCommissionStatusRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(ChangeCrmCommissionStatusRecord));
        var entity = await _repository.CrmCommissions.CommissionAsync(record.CommissionId, true, cancellationToken)
            ?? throw new NotFoundException("CrmCommission", "CommissionId", record.CommissionId.ToString());
        if (!IsAllowedTransition(entity.Status, record.NewStatus))
            throw new BadRequestException($"Invalid commission status transition from {entity.Status} to {record.NewStatus}.");

        entity.Status = record.NewStatus;
        entity.UpdatedBy = record.ChangedBy;
        entity.UpdatedDate = DateTime.UtcNow;
        entity.Notes = MergeNotes(entity.Notes, record.Notes);
        if (record.NewStatus >= 3 && string.IsNullOrWhiteSpace(entity.InvoiceNo))
            entity.InvoiceNo = await GenerateInvoiceNoInternalAsync(entity.UniversityId, DateTime.UtcNow, cancellationToken);
        if (record.NewStatus == 4)
        {
            entity.PaidDate ??= DateTime.UtcNow;
            entity.PaidAmount = record.PaidAmount ?? entity.NetAmount;
        }
        _repository.CrmCommissions.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmCommissionDto>();
    }

    public async Task<CrmCommissionDto> GenerateInvoiceAsync(int commissionId, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.CrmCommissions.CommissionAsync(commissionId, true, cancellationToken)
            ?? throw new NotFoundException("CrmCommission", "CommissionId", commissionId.ToString());
        if (string.IsNullOrWhiteSpace(entity.InvoiceNo))
        {
            entity.InvoiceNo = await GenerateInvoiceNoInternalAsync(entity.UniversityId, DateTime.UtcNow, cancellationToken);
            entity.UpdatedDate = DateTime.UtcNow;
            _repository.CrmCommissions.UpdateByState(entity);
            await _repository.SaveAsync(cancellationToken);
        }
        return entity.MapTo<CrmCommissionDto>();
    }

    public async Task<CrmCommissionDashboardDto> DashboardAsync(CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT
            SUM(CASE WHEN Status = 1 THEN 1 ELSE 0 END) AS PendingCount,
            SUM(CASE WHEN Status = 2 THEN 1 ELSE 0 END) AS DueCount,
            SUM(CASE WHEN Status = 3 THEN 1 ELSE 0 END) AS InvoicedCount,
            SUM(CASE WHEN Status = 4 THEN 1 ELSE 0 END) AS PaidCount,
            SUM(CASE WHEN Status = 5 THEN 1 ELSE 0 END) AS DisputedCount,
            SUM(CASE WHEN Status = 6 THEN 1 ELSE 0 END) AS WrittenOffCount,
            CAST(ISNULL(SUM(NetAmount), 0) AS decimal(18,2)) AS TotalNetAmount,
            CAST(ISNULL(SUM(NetAmountBdt), 0) AS decimal(18,2)) AS TotalNetAmountBdt,
            CAST(ISNULL(SUM(ISNULL(PaidAmount, 0)), 0) AS decimal(18,2)) AS TotalPaidAmount,
            CAST(ISNULL(SUM(NetAmount - ISNULL(PaidAmount, 0)), 0) AS decimal(18,2)) AS TotalOutstandingAmount
        FROM CrmCommission WHERE IsDeleted = 0";
        return await _repository.CrmCommissions.AdoExecuteSingleDataAsync<CrmCommissionDashboardDto>(sql, null, cancellationToken)
            ?? new CrmCommissionDashboardDto();
    }

    public async Task<IEnumerable<CrmCommissionAgingDto>> AgingReportAsync(CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT CommissionId, ApplicationId, AgentId, StudentNameSnapshot, UniversityNameSnapshot, InvoiceNo, DueDate,
            CASE WHEN DueDate IS NULL THEN 0 ELSE DATEDIFF(DAY, DueDate, GETDATE()) END AS AgingDays,
            NetAmount, CAST(ISNULL(PaidAmount, 0) AS decimal(18,2)) AS PaidAmount,
            CAST(NetAmount - ISNULL(PaidAmount, 0) AS decimal(18,2)) AS OutstandingAmount,
            Status
        FROM CrmCommission
        WHERE IsDeleted = 0 AND NetAmount - ISNULL(PaidAmount, 0) > 0
        ORDER BY CASE WHEN DueDate IS NULL THEN 1 ELSE 0 END, DueDate, CommissionId DESC";
        return await _repository.CrmCommissions.AdoExecuteListQueryAsync<CrmCommissionAgingDto>(sql, null, cancellationToken);
    }

    public async Task<IEnumerable<CrmAgentCommissionSummaryDto>> AgentSummaryAsync(CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT a.AgentId, a.AgentName,
            COUNT(c.CommissionId) AS TotalCommissions,
            SUM(CASE WHEN app.Status = 9 THEN 1 ELSE 0 END) AS EnrolledStudentCount,
            SUM(CASE WHEN c.Status IN (1, 2, 3) THEN 1 ELSE 0 END) AS PendingCommissions,
            CAST(ISNULL(SUM(ISNULL(c.GrossAmount, 0)), 0) AS decimal(18,2)) AS GrossAmount,
            CAST(ISNULL(SUM(ISNULL(c.NetAmount, 0)), 0) AS decimal(18,2)) AS NetAmount,
            CAST(ISNULL(SUM(ISNULL(c.PaidAmount, 0)), 0) AS decimal(18,2)) AS PaidAmount,
            CAST(ISNULL(SUM(ISNULL(c.NetAmount, 0) - ISNULL(c.PaidAmount, 0)), 0) AS decimal(18,2)) AS OutstandingAmount
        FROM CrmAgent a
        LEFT JOIN CrmCommission c ON c.AgentId = a.AgentId AND c.IsDeleted = 0
        LEFT JOIN CrmApplication app ON app.ApplicationId = c.ApplicationId
        GROUP BY a.AgentId, a.AgentName
        ORDER BY a.AgentName";
        return await _repository.CrmCommissions.AdoExecuteListQueryAsync<CrmAgentCommissionSummaryDto>(sql, null, cancellationToken);
    }

    public async Task<CrmCommissionDto?> EnsureCommissionForEnrollmentAsync(int applicationId, int changedBy, CancellationToken cancellationToken = default)
    {
        var existing = (await _repository.CrmCommissions.CommissionsByApplicationIdAsync(applicationId, true, cancellationToken))
            .FirstOrDefault(x => !x.IsDeleted);
        if (existing != null)
            return existing.MapTo<CrmCommissionDto>();

        var draft = await BuildCommissionDraftAsync(applicationId, null, cancellationToken);
        var entity = new CrmCommission
        {
            ApplicationId = applicationId,
            UniversityId = draft.UniversityId,
            AgentId = draft.AgentId,
            BranchId = draft.BranchId,
            StudentNameSnapshot = draft.StudentNameSnapshot,
            UniversityNameSnapshot = draft.UniversityNameSnapshot,
            TuitionFeeBase = draft.TuitionFeeBase,
            CommissionRate = draft.CommissionRate,
            CommissionType = draft.CommissionType,
            GrossAmount = draft.GrossAmount,
            ScholarshipDeduction = draft.ScholarshipDeduction,
            NetAmount = draft.NetAmount,
            Currency = draft.Currency,
            ExchangeRate = draft.ExchangeRate,
            NetAmountBdt = draft.NetAmountBdt,
            Status = 1,
            DueDate = draft.DueDate,
            PaidAmount = null,
            PaidDate = null,
            InvoiceNo = null,
            Notes = draft.Notes,
            IsDeleted = false,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = changedBy,
            UpdatedDate = null,
            UpdatedBy = null
        };
        int newId = await _repository.CrmCommissions.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        entity.CommissionId = newId;
        return entity.MapTo<CrmCommissionDto>();
    }

    public async Task<CrmCommissionDto?> RecalculateByApplicationAsync(int applicationId, int changedBy, CancellationToken cancellationToken = default)
    {
        var draft = await BuildCommissionDraftAsync(applicationId, null, cancellationToken);
        var commissions = (await _repository.CrmCommissions.CommissionsByApplicationIdAsync(applicationId, true, cancellationToken)).Where(x => !x.IsDeleted).ToList();
        if (!commissions.Any())
        {
            var application = await _repository.CrmApplications.CrmApplicationAsync(applicationId, false, cancellationToken);
            if (application?.Status == 9)
                return await EnsureCommissionForEnrollmentAsync(applicationId, changedBy, cancellationToken);
            return null;
        }

        foreach (var entity in commissions)
        {
            entity.UniversityId = draft.UniversityId;
            entity.AgentId = draft.AgentId;
            entity.BranchId = draft.BranchId;
            entity.StudentNameSnapshot = draft.StudentNameSnapshot;
            entity.UniversityNameSnapshot = draft.UniversityNameSnapshot;
            entity.TuitionFeeBase = draft.TuitionFeeBase;
            entity.CommissionRate = draft.CommissionRate;
            entity.CommissionType = draft.CommissionType;
            entity.GrossAmount = draft.GrossAmount;
            entity.ScholarshipDeduction = draft.ScholarshipDeduction;
            entity.NetAmount = draft.NetAmount;
            entity.Currency = draft.Currency;
            entity.ExchangeRate = draft.ExchangeRate;
            entity.NetAmountBdt = draft.NetAmountBdt;
            entity.DueDate ??= draft.DueDate;
            entity.UpdatedBy = changedBy;
            entity.UpdatedDate = DateTime.UtcNow;
            _repository.CrmCommissions.UpdateByState(entity);
        }
        await _repository.SaveAsync(cancellationToken);
        return commissions.First().MapTo<CrmCommissionDto>();
    }

    public async Task ReverseByRefundAsync(int paymentId, decimal refundAmount, DateTime refundDate, int changedBy, string? reason, CancellationToken cancellationToken = default)
    {
        if (refundAmount <= 0) return;
        var payment = await _repository.CrmStudentPayments.StudentPaymentAsync(paymentId, false, cancellationToken);
        if (payment == null || payment.Amount <= 0) return;

        var commissions = (await _repository.CrmCommissions.CommissionsByApplicationIdAsync(payment.ApplicationId, true, cancellationToken))
            .Where(x => !x.IsDeleted)
            .ToList();
        if (!commissions.Any()) return;

        var ratio = Math.Min(1m, refundAmount / payment.Amount);
        foreach (var commission in commissions)
        {
            var reversalNet = Math.Round(commission.NetAmount * ratio, 2);
            var reversalBdt = Math.Round(commission.NetAmountBdt * ratio, 2);
            commission.NetAmount = Math.Max(0m, commission.NetAmount - reversalNet);
            commission.NetAmountBdt = Math.Max(0m, commission.NetAmountBdt - reversalBdt);
            commission.ScholarshipDeduction = Math.Round(commission.GrossAmount - commission.NetAmount, 2);
            if (commission.PaidAmount.HasValue)
                commission.PaidAmount = Math.Max(0m, commission.PaidAmount.Value - reversalNet);
            commission.Status = commission.Status == 4 ? (byte)5 : (byte)6;
            commission.Notes = MergeNotes(commission.Notes, $"Refund reversal applied on {refundDate:dd MMM yyyy}: {refundAmount:0.##}. {reason}".Trim());
            commission.UpdatedBy = changedBy;
            commission.UpdatedDate = DateTime.UtcNow;
            _repository.CrmCommissions.UpdateByState(commission);
        }
        await _repository.SaveAsync(cancellationToken);
    }

    private async Task<CommissionDraft> BuildCommissionDraftAsync(int applicationId, int? requestedAgentId, CancellationToken cancellationToken)
    {
        var application = await _repository.CrmApplications.CrmApplicationAsync(applicationId, false, cancellationToken)
            ?? throw new NotFoundException("CrmApplication", "ApplicationId", applicationId.ToString());
        var student = await _repository.CrmStudents.CrmStudentAsync(application.StudentId, false, cancellationToken)
            ?? throw new NotFoundException("CrmStudent", "StudentId", application.StudentId.ToString());
        var institute = await _repository.CrmInstitutes.CrmInstituteAsync(application.UniversityId, false, cancellationToken)
            ?? throw new NotFoundException("CrmInstitute", "InstituteId", application.UniversityId.ToString());

        var courseFees = await _repository.CrmCourseFees.CrmCourseFeesByCourseIdAsync(application.ProgramId, false, cancellationToken);
        var tuitionFees = courseFees.Where(x => x.IntakeId == application.IntakeId && x.FeeType == 1).ToList();
        var tuitionBase = tuitionFees.Sum(x => x.Amount);
        var currency = tuitionFees.Select(x => x.Currency).FirstOrDefault(x => !string.IsNullOrWhiteSpace(x)) ?? "BDT";
        var scholarships = await _repository.CrmScholarshipApplications.ScholarshipApplicationsByApplicationIdAsync(applicationId, false, cancellationToken);
        var scholarshipAmount = scholarships.Where(x => x.Status != 4).Sum(x => x.GrantedAmount);
        var commissionableBase = Math.Max(0m, tuitionBase - scholarshipAmount);
        var commissionRate = institute.CommissionRate ?? 0m;
        var commissionType = institute.CommissionType ?? (byte)1;
        var grossAmount = commissionType == 2 ? commissionRate : Math.Round(tuitionBase * commissionRate / 100m, 2);
        decimal netAmount;
        if (commissionType == 2)
        {
            netAmount = tuitionBase <= 0 ? 0m : Math.Round(grossAmount * (commissionableBase / tuitionBase), 2);
        }
        else
        {
            netAmount = Math.Round(commissionableBase * commissionRate / 100m, 2);
        }
        var scholarshipDeduction = Math.Max(0m, Math.Round(grossAmount - netAmount, 2));
        var exchangeRate = await ResolveExchangeRateAsync(currency, cancellationToken);
        var resolvedAgentId = requestedAgentId;
        if (!resolvedAgentId.HasValue || resolvedAgentId <= 0)
        {
            resolvedAgentId = student.AgentId;
            if ((!resolvedAgentId.HasValue || resolvedAgentId <= 0) && student.LeadId.HasValue && student.LeadId.Value > 0)
            {
                var agentLead = await _repository.CrmAgentLeads.CrmAgentLeadByLeadIdAsync(student.LeadId.Value, false, cancellationToken);
                resolvedAgentId = agentLead?.AgentId;
            }
        }

        return new CommissionDraft(
            application.UniversityId,
            resolvedAgentId,
            application.BranchId,
            string.IsNullOrWhiteSpace(student.StudentName) ? $"Student-{student.StudentId}" : student.StudentName,
            string.IsNullOrWhiteSpace(institute.InstituteName) ? $"University-{application.UniversityId}" : institute.InstituteName,
            tuitionBase,
            commissionRate,
            commissionType,
            grossAmount,
            scholarshipDeduction,
            netAmount,
            currency.ToUpperInvariant(),
            exchangeRate,
            Math.Round(netAmount * exchangeRate, 2),
            (application.EnrollmentDate ?? DateTime.UtcNow).Date.AddDays(30),
            scholarshipDeduction > 0 ? $"Scholarship deduction applied: {scholarshipDeduction:0.##} {currency.ToUpperInvariant()}." : null);
    }

    private async Task<decimal> ResolveExchangeRateAsync(string currencyCode, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(currencyCode) || currencyCode.Equals("BDT", StringComparison.OrdinalIgnoreCase))
            return 1m;

        const string sql = @"SELECT TOP 1 CAST(ISNULL(cr.CurrencyRateRation, 1) AS decimal(18,6)) AS Rate
            FROM CurrencyRate cr
            INNER JOIN Currency c ON c.CurrencyId = cr.CurrencyId
            WHERE c.CurrencyCode = @CurrencyCode
            ORDER BY ISNULL(cr.CurrencyMonth, cr.CreatedDate) DESC, cr.CurencyRateId DESC";
        var result = await _repository.CrmCommissions.AdoExecuteSingleDataAsync<ExchangeRateLookupDto>(sql, [new SqlParameter("@CurrencyCode", currencyCode.Trim().ToUpperInvariant())], cancellationToken);
        return result?.Rate > 0 ? result.Rate : 1m;
    }

    private async Task<string> GenerateInvoiceNoInternalAsync(int universityId, DateTime invoiceDate, CancellationToken cancellationToken)
    {
        var lockKey = $"{universityId}:{invoiceDate:yyyyMM}";
        var invoiceLock = InvoiceLocks.GetOrAdd(lockKey, _ => new SemaphoreSlim(1, 1));
        await invoiceLock.WaitAsync(cancellationToken);
        try
        {
            var institute = await _repository.CrmInstitutes.CrmInstituteAsync(universityId, false, cancellationToken);
            var universityCode = string.IsNullOrWhiteSpace(institute?.InstituteCode) ? $"UNI{universityId}" : institute!.InstituteCode!.Trim().ToUpperInvariant();
            var yearMonth = invoiceDate.ToString("yyyyMM", CultureInfo.InvariantCulture);
            var monthCount = await _repository.CrmCommissions.CountAsync(
                x => x.UniversityId == universityId && x.CreatedDate.Year == invoiceDate.Year && x.CreatedDate.Month == invoiceDate.Month,
                cancellationToken);
            var seq = monthCount + 1;
            string invoiceNo;
            do
            {
                invoiceNo = $"INV-{universityCode}-{yearMonth}-{seq:0000}";
                seq++;
            }
            while (await _repository.CrmCommissions.ExistsAsync(x => x.InvoiceNo == invoiceNo, cancellationToken));
            return invoiceNo;
        }
        finally
        {
            invoiceLock.Release();
        }
    }

    private static string? MergeNotes(string? existing, string? addition)
    {
        if (string.IsNullOrWhiteSpace(addition)) return existing;
        if (string.IsNullOrWhiteSpace(existing)) return addition.Trim();
        if (existing.Contains(addition.Trim(), StringComparison.OrdinalIgnoreCase)) return existing;
        var builder = new StringBuilder(existing.Trim());
        builder.AppendLine();
        builder.Append(addition.Trim());
        return builder.ToString();
    }

    private static bool IsAllowedTransition(byte oldStatus, byte newStatus)
    {
        if (oldStatus == newStatus) return true;
        return oldStatus switch
        {
            1 => newStatus == 2,
            2 => newStatus == 3,
            3 => newStatus == 4,
            4 => newStatus == 5,
            5 => newStatus == 6,
            _ => false
        };
    }

    private sealed record CommissionDraft(int UniversityId, int? AgentId, int BranchId, string StudentNameSnapshot, string UniversityNameSnapshot, decimal TuitionFeeBase, decimal CommissionRate, byte CommissionType, decimal GrossAmount, decimal ScholarshipDeduction, decimal NetAmount, string Currency, decimal ExchangeRate, decimal NetAmountBdt, DateTime DueDate, string? Notes);
    private sealed record ExchangeRateLookupDto(decimal Rate);
}
