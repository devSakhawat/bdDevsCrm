using System.Globalization;
using System.Threading;
using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Extensions;
using bdDevs.Shared.Records.CRM;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services.CRM;
using Domain.Entities.Entities.CRM;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.CRM;

internal sealed class CrmStudentPaymentService : ICrmStudentPaymentService
{
    private static readonly SemaphoreSlim ReceiptLock = new(1, 1);
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmStudentPaymentService> _logger;
    private readonly IConfiguration _configuration;

    public CrmStudentPaymentService(IRepositoryManager repository, ILogger<CrmStudentPaymentService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<CrmStudentPaymentDto> CreateAsync(CreateCrmStudentPaymentRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmStudentPaymentRecord));
        await ValidatePaymentReferencesAsync(record.StudentId, record.ApplicationId, cancellationToken);
        var entity = new CrmStudentPayment
        {
            StudentId = record.StudentId,
            ApplicationId = record.ApplicationId,
            BranchId = record.BranchId,
            PaymentType = record.PaymentType,
            Amount = record.Amount,
            Currency = record.Currency,
            ExchangeRate = record.ExchangeRate <= 0 ? 1m : record.ExchangeRate,
            AmountBdt = Math.Round(record.Amount * (record.ExchangeRate <= 0 ? 1m : record.ExchangeRate), 2),
            PaymentDate = record.PaymentDate,
            PaymentMethod = record.PaymentMethod,
            BankName = record.BankName,
            TransactionRef = record.TransactionRef,
            Status = record.Status == 0 ? (byte)1 : record.Status,
            ReceivedBy = record.ReceivedBy,
            VerifiedBy = record.VerifiedBy,
            Notes = record.Notes,
            IsDeleted = record.IsDeleted,
            CreatedDate = record.CreatedDate,
            CreatedBy = record.CreatedBy,
            UpdatedDate = record.UpdatedDate,
            UpdatedBy = record.UpdatedBy
        };
        entity.ReceiptNo = await GenerateReceiptNoInternalAsync(entity.BranchId, entity.PaymentDate, cancellationToken);
        int newId = await _repository.CrmStudentPayments.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        entity.StudentPaymentId = newId;
        return entity.MapTo<CrmStudentPaymentDto>();
    }

    public async Task<CrmStudentPaymentDto> UpdateAsync(UpdateCrmStudentPaymentRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmStudentPaymentRecord));
        _ = await _repository.CrmStudentPayments.StudentPaymentAsync(record.StudentPaymentId, false, cancellationToken)
            ?? throw new NotFoundException("CrmStudentPayment", "StudentPaymentId", record.StudentPaymentId.ToString());
        var entity = record.MapTo<CrmStudentPayment>();
        if (string.IsNullOrWhiteSpace(entity.ReceiptNo))
            entity.ReceiptNo = await GenerateReceiptNoInternalAsync(entity.BranchId, entity.PaymentDate, cancellationToken);
        entity.AmountBdt = Math.Round(entity.Amount * (entity.ExchangeRate <= 0 ? 1m : entity.ExchangeRate), 2);
        _repository.CrmStudentPayments.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmStudentPaymentDto>();
    }

    public async Task DeleteAsync(DeleteCrmStudentPaymentRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.StudentPaymentId <= 0) throw new BadRequestException("Invalid delete request!");
        var entity = await _repository.CrmStudentPayments.StudentPaymentAsync(record.StudentPaymentId, true, cancellationToken)
            ?? throw new NotFoundException("CrmStudentPayment", "StudentPaymentId", record.StudentPaymentId.ToString());
        entity.IsDeleted = true;
        entity.UpdatedDate = DateTime.UtcNow;
        _repository.CrmStudentPayments.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmStudentPaymentDto> StudentPaymentAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => (await _repository.CrmStudentPayments.StudentPaymentAsync(id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CrmStudentPayment", "StudentPaymentId", id.ToString())).MapTo<CrmStudentPaymentDto>();

    public async Task<IEnumerable<CrmStudentPaymentDto>> StudentPaymentsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmStudentPayments.StudentPaymentsAsync(trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmStudentPaymentDto>() : Enumerable.Empty<CrmStudentPaymentDto>();
    }

    public async Task<IEnumerable<CrmStudentPaymentDto>> StudentPaymentsByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmStudentPayments.StudentPaymentsByStudentIdAsync(studentId, trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmStudentPaymentDto>() : Enumerable.Empty<CrmStudentPaymentDto>();
    }

    public async Task<IEnumerable<CrmStudentPaymentDto>> StudentPaymentsByApplicationIdAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmStudentPayments.StudentPaymentsByApplicationIdAsync(applicationId, trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmStudentPaymentDto>() : Enumerable.Empty<CrmStudentPaymentDto>();
    }

    public async Task<GridEntity<CrmStudentPaymentDto>> StudentPaymentsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT StudentPaymentId, StudentId, ApplicationId, BranchId, PaymentType, ReceiptNo, Amount, Currency, ExchangeRate, AmountBdt, PaymentDate, PaymentMethod, BankName, TransactionRef, Status, ReceivedBy, VerifiedBy, Notes, IsDeleted, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmStudentPayment";
        return await _repository.CrmStudentPayments.AdoGridDataAsync<CrmStudentPaymentDto>(sql, options, "StudentPaymentId DESC", string.Empty, cancellationToken);
    }

    public async Task<CrmStudentPaymentDto> ChangeStatusAsync(ChangeCrmStudentPaymentStatusRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(ChangeCrmStudentPaymentStatusRecord));
        var entity = await _repository.CrmStudentPayments.StudentPaymentAsync(record.StudentPaymentId, true, cancellationToken)
            ?? throw new NotFoundException("CrmStudentPayment", "StudentPaymentId", record.StudentPaymentId.ToString());
        if (!IsAllowedTransition(entity.Status, record.NewStatus))
            throw new BadRequestException($"Invalid payment status transition from {entity.Status} to {record.NewStatus}.");
        entity.Status = record.NewStatus;
        entity.UpdatedBy = record.ChangedBy;
        entity.UpdatedDate = DateTime.UtcNow;
        entity.Notes = string.IsNullOrWhiteSpace(record.Notes) ? entity.Notes : record.Notes;
        if (record.NewStatus >= 4 && string.IsNullOrWhiteSpace(entity.ReceiptNo))
            entity.ReceiptNo = await GenerateReceiptNoInternalAsync(entity.BranchId, entity.PaymentDate, cancellationToken);
        if (record.NewStatus >= 3 && !entity.VerifiedBy.HasValue)
            entity.VerifiedBy = record.ChangedBy;
        _repository.CrmStudentPayments.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmStudentPaymentDto>();
    }

    public async Task<CrmStudentPaymentReceiptDto> GenerateReceiptAsync(int paymentId, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.CrmStudentPayments.StudentPaymentAsync(paymentId, true, cancellationToken)
            ?? throw new NotFoundException("CrmStudentPayment", "StudentPaymentId", paymentId.ToString());
        if (string.IsNullOrWhiteSpace(entity.ReceiptNo))
        {
            entity.ReceiptNo = await GenerateReceiptNoInternalAsync(entity.BranchId, entity.PaymentDate, cancellationToken);
            _repository.CrmStudentPayments.UpdateByState(entity);
            await _repository.SaveAsync(cancellationToken);
        }
        return new CrmStudentPaymentReceiptDto
        {
            StudentPaymentId = entity.StudentPaymentId,
            ReceiptNo = entity.ReceiptNo ?? string.Empty,
            StudentId = entity.StudentId,
            ApplicationId = entity.ApplicationId,
            Amount = entity.Amount,
            Currency = entity.Currency,
            AmountBdt = entity.AmountBdt,
            PaymentDate = entity.PaymentDate,
            PaymentMethod = entity.PaymentMethod,
            BankName = entity.BankName,
            TransactionRef = entity.TransactionRef,
            Status = entity.Status
        };
    }

    private async Task ValidatePaymentReferencesAsync(int studentId, int applicationId, CancellationToken cancellationToken)
    {
        _ = await _repository.CrmStudents.CrmStudentAsync(studentId, false, cancellationToken)
            ?? throw new NotFoundException("Student", "StudentId", studentId.ToString());
        _ = await _repository.CrmApplications.CrmApplicationAsync(applicationId, false, cancellationToken)
            ?? throw new NotFoundException("CrmApplication", "ApplicationId", applicationId.ToString());
    }

    private async Task<string> GenerateReceiptNoInternalAsync(int branchId, DateTime paymentDate, CancellationToken cancellationToken)
    {
        await ReceiptLock.WaitAsync(cancellationToken);
        try
        {
            var branch = await _repository.Branches.FirstOrDefaultAsync(x => x.Branchid == branchId, false, cancellationToken);
            var branchCode = string.IsNullOrWhiteSpace(branch?.Branchcode) ? $"BR{branchId}" : branch.Branchcode!.Trim().ToUpperInvariant();
            var yearMonth = paymentDate.ToString("yyyyMM", CultureInfo.InvariantCulture);
            var monthCount = await _repository.CrmStudentPayments.CountAsync(
                x => x.BranchId == branchId && x.PaymentDate.Year == paymentDate.Year && x.PaymentDate.Month == paymentDate.Month,
                cancellationToken);
            string receiptNo;
            int seq = monthCount + 1;
            do
            {
                receiptNo = $"RCT-{branchCode}-{yearMonth}-{seq:00000}";
                seq++;
            }
            while (await _repository.CrmStudentPayments.ExistsAsync(x => x.ReceiptNo == receiptNo, cancellationToken));
            return receiptNo;
        }
        finally
        {
            ReceiptLock.Release();
        }
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
}
