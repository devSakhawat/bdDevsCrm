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

internal sealed class CrmVisaApplicationService : ICrmVisaApplicationService
{
    private const byte ApplicationStatusUnconditional = 5;
    private const byte ApplicationStatusEnrolled = 9;
    private const byte VisaStatusApproved = 7;
    private const byte VisaStatusRefused = 8;
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmVisaApplicationService> _logger;
    private readonly IConfiguration _configuration;

    public CrmVisaApplicationService(IRepositoryManager repository, ILogger<CrmVisaApplicationService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<CrmVisaApplicationDto> CreateAsync(CreateCrmVisaApplicationRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmVisaApplicationRecord));
        await ValidatePreVisaAsync(record.ApplicationId, record.StudentId, cancellationToken);
        if (record.Status == VisaStatusRefused && string.IsNullOrWhiteSpace(record.RefusalReason))
            throw new BadRequestException("Refusal reason is required for refused visas.");

        var entity = record.MapTo<CrmVisaApplication>();
        int newId = await _repository.CrmVisaApplications.CreateAndIdAsync(entity, cancellationToken);
        await _repository.CrmVisaStatusHistories.CreateAsync(new CrmVisaStatusHistory
        {
            VisaApplicationId = newId,
            OldStatus = 0,
            NewStatus = entity.Status,
            ChangedBy = entity.CreatedBy,
            ChangedDate = DateTime.UtcNow,
            Notes = entity.Notes
        }, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        entity.VisaApplicationId = newId;
        if (entity.Status == VisaStatusApproved)
            await HandleApprovedOutcomeAsync(entity.StudentId, entity.CreatedBy, cancellationToken);
        return entity.MapTo<CrmVisaApplicationDto>();
    }

    public async Task<CrmVisaApplicationDto> UpdateAsync(UpdateCrmVisaApplicationRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmVisaApplicationRecord));
        var existing = await _repository.CrmVisaApplications.VisaApplicationAsync(record.VisaApplicationId, true, cancellationToken)
            ?? throw new NotFoundException("CrmVisaApplication", "VisaApplicationId", record.VisaApplicationId.ToString());
        await ValidatePreVisaAsync(record.ApplicationId, record.StudentId, cancellationToken);
        if (record.Status == VisaStatusRefused && string.IsNullOrWhiteSpace(record.RefusalReason))
            throw new BadRequestException("Refusal reason is required for refused visas.");

        var entity = record.MapTo<CrmVisaApplication>();
        _repository.CrmVisaApplications.UpdateByState(entity);
        if (existing.Status != entity.Status)
        {
            await _repository.CrmVisaStatusHistories.CreateAsync(new CrmVisaStatusHistory
            {
                VisaApplicationId = entity.VisaApplicationId,
                OldStatus = existing.Status,
                NewStatus = entity.Status,
                ChangedBy = entity.UpdatedBy ?? entity.CreatedBy,
                ChangedDate = DateTime.UtcNow,
                Notes = entity.Notes
            }, cancellationToken);
        }
        await _repository.SaveAsync(cancellationToken);
        if (entity.Status == VisaStatusApproved)
            await HandleApprovedOutcomeAsync(entity.StudentId, entity.UpdatedBy ?? entity.CreatedBy, cancellationToken);
        return entity.MapTo<CrmVisaApplicationDto>();
    }

    public async Task DeleteAsync(DeleteCrmVisaApplicationRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.VisaApplicationId <= 0) throw new BadRequestException("Invalid delete request!");
        var entity = await _repository.CrmVisaApplications.VisaApplicationAsync(record.VisaApplicationId, true, cancellationToken)
            ?? throw new NotFoundException("CrmVisaApplication", "VisaApplicationId", record.VisaApplicationId.ToString());
        entity.IsDeleted = true;
        entity.UpdatedDate = DateTime.UtcNow;
        _repository.CrmVisaApplications.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmVisaApplicationDto> VisaApplicationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => (await _repository.CrmVisaApplications.VisaApplicationAsync(id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CrmVisaApplication", "VisaApplicationId", id.ToString())).MapTo<CrmVisaApplicationDto>();

    public async Task<IEnumerable<CrmVisaApplicationDto>> VisaApplicationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmVisaApplications.VisaApplicationsAsync(trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmVisaApplicationDto>() : Enumerable.Empty<CrmVisaApplicationDto>();
    }

    public async Task<IEnumerable<CrmVisaApplicationDto>> VisaApplicationsByApplicationIdAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmVisaApplications.VisaApplicationsByApplicationIdAsync(applicationId, trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmVisaApplicationDto>() : Enumerable.Empty<CrmVisaApplicationDto>();
    }

    public async Task<IEnumerable<CrmVisaApplicationDto>> VisaApplicationsByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmVisaApplications.VisaApplicationsByStudentIdAsync(studentId, trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmVisaApplicationDto>() : Enumerable.Empty<CrmVisaApplicationDto>();
    }

    public async Task<GridEntity<CrmVisaApplicationDto>> VisaApplicationsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT VisaApplicationId, ApplicationId, StudentId, BranchId, VisaCountryId, EmbassyName, ApplicationRefNo, Status, SubmittedDate, BiometricDate, InterviewDate, DecisionDate, ExpiryDate, RefusalReason, Notes, IsDeleted, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmVisaApplication";
        return await _repository.CrmVisaApplications.AdoGridDataAsync<CrmVisaApplicationDto>(sql, options, "VisaApplicationId DESC", string.Empty, cancellationToken);
    }

    public async Task<CrmVisaApplicationDto> ChangeStatusAsync(ChangeCrmVisaApplicationStatusRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(ChangeCrmVisaApplicationStatusRecord));
        var entity = await _repository.CrmVisaApplications.VisaApplicationAsync(record.VisaApplicationId, true, cancellationToken)
            ?? throw new NotFoundException("CrmVisaApplication", "VisaApplicationId", record.VisaApplicationId.ToString());
        if (!IsAllowedTransition(entity.Status, record.NewStatus))
            throw new BadRequestException($"Invalid visa status transition from {entity.Status} to {record.NewStatus}.");
        if (record.NewStatus == VisaStatusRefused && string.IsNullOrWhiteSpace(record.RefusalReason))
            throw new BadRequestException("Refusal reason is required for refused visas.");

        var old = entity.Status;
        entity.Status = record.NewStatus;
        entity.RefusalReason = record.NewStatus == VisaStatusRefused ? record.RefusalReason : entity.RefusalReason;
        entity.Notes = record.Notes ?? entity.Notes;
        entity.UpdatedBy = record.ChangedBy;
        entity.UpdatedDate = DateTime.UtcNow;
        if (record.NewStatus == 3 && !entity.SubmittedDate.HasValue) entity.SubmittedDate = DateTime.UtcNow;
        if (record.NewStatus == 4 && !entity.BiometricDate.HasValue) entity.BiometricDate = DateTime.UtcNow;
        if (record.NewStatus == 5 && !entity.InterviewDate.HasValue) entity.InterviewDate = DateTime.UtcNow;
        if (record.NewStatus is 7 or 8 && !entity.DecisionDate.HasValue) entity.DecisionDate = DateTime.UtcNow;
        _repository.CrmVisaApplications.UpdateByState(entity);
        await _repository.CrmVisaStatusHistories.CreateAsync(new CrmVisaStatusHistory
        {
            VisaApplicationId = entity.VisaApplicationId,
            OldStatus = old,
            NewStatus = entity.Status,
            ChangedBy = record.ChangedBy,
            ChangedDate = DateTime.UtcNow,
            Notes = record.NewStatus == VisaStatusRefused ? record.RefusalReason ?? record.Notes : record.Notes
        }, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        if (record.NewStatus == VisaStatusApproved)
            await HandleApprovedOutcomeAsync(entity.StudentId, record.ChangedBy, cancellationToken);
        return entity.MapTo<CrmVisaApplicationDto>();
    }

    private async Task ValidatePreVisaAsync(int applicationId, int studentId, CancellationToken cancellationToken)
    {
        var application = await _repository.CrmApplications.CrmApplicationAsync(applicationId, false, cancellationToken)
            ?? throw new NotFoundException("CrmApplication", "ApplicationId", applicationId.ToString());
        if (application.Status is not (ApplicationStatusUnconditional or ApplicationStatusEnrolled))
            throw new BadRequestException("Application must be Unconditional Offer or Enrolled before visa processing.");
        var student = await _repository.CrmStudents.CrmStudentAsync(studentId, false, cancellationToken)
            ?? throw new NotFoundException("Student", "StudentId", studentId.ToString());
        if (!student.PassportExpiryDate.HasValue)
            throw new BadRequestException("Student passport is missing or expired.");
        var normalizedPassportExpiry = student.PassportExpiryDate.Value.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(student.PassportExpiryDate.Value, DateTimeKind.Utc)
            : student.PassportExpiryDate.Value.ToUniversalTime();
        if (normalizedPassportExpiry.Date < DateTime.UtcNow.Date)
            throw new BadRequestException("Student passport is missing or expired.");
    }

    private async Task HandleApprovedOutcomeAsync(int studentId, int changedBy, CancellationToken cancellationToken)
    {
        var student = await _repository.CrmStudents.CrmStudentAsync(studentId, true, cancellationToken);
        if (student == null) return;
        var visaApprovedStatus = (await _repository.CrmStudentStatuses.ListByConditionAsync(
            x => x.IsActive && x.StatusName.ToLower().Contains("visa approved"),
            x => x.StudentStatusId,
            false,
            false,
            cancellationToken)).FirstOrDefault();
        if (visaApprovedStatus != null)
        {
            var oldStatus = student.StudentStatusId;
            student.StudentStatusId = visaApprovedStatus.StudentStatusId;
            student.UpdatedBy = changedBy;
            student.UpdatedDate = DateTime.UtcNow;
            _repository.CrmStudents.UpdateByState(student);
            await _repository.CrmStudentStatusHistories.CreateAsync(new CrmStudentStatusHistory
            {
                StudentId = student.StudentId,
                OldStatus = oldStatus,
                NewStatus = student.StudentStatusId ?? 0,
                ChangedBy = changedBy,
                ChangedDate = DateTime.UtcNow,
                Notes = "Student status updated from visa approval"
            }, cancellationToken);
        }
        await _repository.SaveAsync(cancellationToken);
        _logger.LogInformation("Pre-departure workflow queued for StudentId {StudentId}", studentId);
    }

    private static bool IsAllowedTransition(byte oldStatus, byte newStatus)
    {
        if (oldStatus == newStatus) return true;
        return oldStatus switch
        {
            1 => newStatus is 2 or 3 or 8,
            2 => newStatus is 3 or 8,
            3 => newStatus is 4 or 5 or 6 or 8,
            4 => newStatus is 5 or 6 or 8,
            5 => newStatus is 6 or 7 or 8,
            6 => newStatus is 7 or 8,
            7 => newStatus is 9,
            _ => false
        };
    }
}
