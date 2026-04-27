using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;
using Domain.Contracts.Services.CRM;
using bdDevs.Shared.DataTransferObjects.CRM;
using Domain.Exceptions;
using Application.Shared.Grid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using bdDevs.Shared.Records.CRM;
using bdDevs.Shared.Extensions;

namespace Application.Services.CRM;

internal sealed class CrmStudentService : ICrmStudentService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmStudentService> _logger;
    private readonly IConfiguration _config;

    public CrmStudentService(IRepositoryManager repository, ILogger<CrmStudentService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    public async Task<CrmStudentDto> CreateAsync(CreateCrmStudentRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmStudentRecord));
        await EnsureApplicationReadyConsistencyAsync(record.IsApplicationReady, record.PassportNumber, record.PreferredCountryId, record.PreferredDegreeLevelId, record.DesiredIntake, record.ConsentPersonalData, record.ConsentMarketing, record.ConsentDocumentProcessing, record.ConsentInternationalSharing, record.ConsentTermsAccepted, cancellationToken);

        var entity = record.MapTo<CrmStudent>();
        int newId = await _repository.CrmStudents.CreateAndIdAsync(entity, cancellationToken);
        await AddStatusHistoryAsync(newId, null, entity.StudentStatusId, entity.CreatedBy, "Student created", cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmStudentDto>() with { StudentId = newId };
    }

    public async Task<CrmStudentDto> UpdateAsync(UpdateCrmStudentRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmStudentRecord));
        var existing = await _repository.CrmStudents.CrmStudentAsync(record.StudentId, false, cancellationToken)
            ?? throw new NotFoundException("Student", "StudentId", record.StudentId.ToString());

        await EnsureApplicationReadyConsistencyAsync(record.IsApplicationReady, record.PassportNumber, record.PreferredCountryId, record.PreferredDegreeLevelId, record.DesiredIntake, record.ConsentPersonalData, record.ConsentMarketing, record.ConsentDocumentProcessing, record.ConsentInternationalSharing, record.ConsentTermsAccepted, cancellationToken);
        await ValidateStatusTransitionAsync(existing.StudentStatusId, record.StudentStatusId, cancellationToken);

        var entity = record.MapTo<CrmStudent>();
        _repository.CrmStudents.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);

        if (existing.StudentStatusId != entity.StudentStatusId && entity.StudentStatusId.HasValue)
            await AddStatusHistoryAsync(entity.StudentId, existing.StudentStatusId, entity.StudentStatusId, entity.UpdatedBy ?? entity.CreatedBy, "Status changed from update", cancellationToken);

        return entity.MapTo<CrmStudentDto>();
    }

    public async Task DeleteAsync(DeleteCrmStudentRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.StudentId <= 0) throw new BadRequestException("Invalid delete request!");
        var entity = await _repository.CrmStudents.CrmStudentAsync(record.StudentId, true, cancellationToken)
            ?? throw new NotFoundException("Student", "StudentId", record.StudentId.ToString());
        entity.IsDeleted = true;
        entity.IsActive = false;
        entity.UpdatedDate = DateTime.UtcNow;
        _repository.CrmStudents.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmStudentDto> StudentAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.CrmStudents.CrmStudentAsync(id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Student", "StudentId", id.ToString());
        return entity.MapTo<CrmStudentDto>();
    }

    public async Task<IEnumerable<CrmStudentDto>> StudentsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmStudents.CrmStudentsAsync(trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmStudentDto>() : Enumerable.Empty<CrmStudentDto>();
    }

    public async Task<IEnumerable<CrmStudentDto>> StudentsByBranchIdAsync(int branchId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmStudents.CrmStudentsByBranchIdAsync(branchId, trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmStudentDto>() : Enumerable.Empty<CrmStudentDto>();
    }

    public async Task<IEnumerable<CrmStudentDto>> StudentForDDLAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmStudents.CrmStudentsAsync(false, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmStudentDto>() : Enumerable.Empty<CrmStudentDto>();
    }

    public async Task<GridEntity<CrmStudentDto>> StudentsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT StudentId, StudentName, StudentCode, Email, Phone, LeadId, StudentStatusId, AgentId, CounselorId, BranchId, ProcessingOfficerId, DateOfBirth, Gender, PassportNumber, PassportExpiryDate, PassportIssueDate, PassportIssueCountryId, VisaTypeId, Nationality, NationalityCountryId, EmergencyContactName, EmergencyContactPhone, EmergencyContactRelation, PreferredCountryId, PreferredDegreeLevelId, DesiredIntake, IeltsStatus, IeltsScore, IeltsExamDate, IsApplicationReady, ApplicationReadyDate, ApplicationReadySetBy, ConsentPersonalData, ConsentMarketing, ConsentDocumentProcessing, ConsentInternationalSharing, ConsentTermsAccepted, IsDeleted, IsActive, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmStudent";
        const string orderBy = "StudentName ASC";
        return await _repository.CrmStudents.AdoGridDataAsync<CrmStudentDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }

    public async Task<CrmStudentDto> ChangeStatusAsync(ChangeCrmStudentStatusRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(ChangeCrmStudentStatusRecord));
        var entity = await _repository.CrmStudents.CrmStudentAsync(record.StudentId, false, cancellationToken)
            ?? throw new NotFoundException("Student", "StudentId", record.StudentId.ToString());
        await ValidateStatusTransitionAsync(entity.StudentStatusId, record.NewStatus, cancellationToken);
        var old = entity.StudentStatusId;
        entity.StudentStatusId = record.NewStatus;
        entity.UpdatedBy = record.ChangedBy;
        entity.UpdatedDate = DateTime.UtcNow;
        _repository.CrmStudents.UpdateByState(entity);
        await AddStatusHistoryAsync(entity.StudentId, old, record.NewStatus, record.ChangedBy, record.Notes, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmStudentDto>();
    }

    public async Task<CrmStudentApplicationReadyCheckDto> ApplicationReadyCheckAsync(int studentId, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.CrmStudents.CrmStudentAsync(studentId, false, cancellationToken)
            ?? throw new NotFoundException("Student", "StudentId", studentId.ToString());

        var missing = BuildApplicationReadyMissingList(entity);
        return new CrmStudentApplicationReadyCheckDto
        {
            StudentId = studentId,
            IsReady = !missing.Any(),
            MissingRequirements = missing
        };
    }

    private async Task ValidateStatusTransitionAsync(int? oldStatusId, int? newStatusId, CancellationToken cancellationToken)
    {
        if (!oldStatusId.HasValue || !newStatusId.HasValue || oldStatusId == newStatusId) return;
        var current = await _repository.CrmStudentStatuses.CrmStudentStatusAsync(oldStatusId.Value, false, cancellationToken);
        var next = await _repository.CrmStudentStatuses.CrmStudentStatusAsync(newStatusId.Value, false, cancellationToken);
        if (current == null || next == null) return;

        var terminalNames = new[] { "rejected", "enrolled", "completed", "visa granted" };
        if (terminalNames.Contains(current.StatusName.ToLower()) && !string.Equals(current.StatusName, next.StatusName, StringComparison.OrdinalIgnoreCase))
            throw new BadRequestException($"Cannot transition from terminal student status '{current.StatusName}'.");
    }

    private async Task EnsureApplicationReadyConsistencyAsync(bool isApplicationReady, string? passportNumber, int? preferredCountryId, int? preferredDegreeLevelId, string? desiredIntake,
        bool consentPersonalData, bool consentMarketing, bool consentDocumentProcessing, bool consentInternationalSharing, bool consentTermsAccepted, CancellationToken cancellationToken)
    {
        if (!isApplicationReady) return;
        var missing = new List<string>();
        if (string.IsNullOrWhiteSpace(passportNumber)) missing.Add("Passport Number");
        if (!preferredCountryId.HasValue) missing.Add("Preferred Country");
        if (!preferredDegreeLevelId.HasValue) missing.Add("Preferred Degree Level");
        if (string.IsNullOrWhiteSpace(desiredIntake)) missing.Add("Desired Intake");
        if (!consentPersonalData || !consentMarketing || !consentDocumentProcessing || !consentInternationalSharing || !consentTermsAccepted)
            missing.Add("All consent fields must be accepted");
        if (missing.Any()) throw new BadRequestException("Student is not application-ready: " + string.Join(", ", missing));
    }

    private IEnumerable<string> BuildApplicationReadyMissingList(CrmStudent student)
    {
        var missing = new List<string>();
        if (string.IsNullOrWhiteSpace(student.PassportNumber)) missing.Add("Passport Number");
        if (!student.PreferredCountryId.HasValue) missing.Add("Preferred Country");
        if (!student.PreferredDegreeLevelId.HasValue) missing.Add("Preferred Degree Level");
        if (string.IsNullOrWhiteSpace(student.DesiredIntake)) missing.Add("Desired Intake");
        if (!(student.ConsentPersonalData && student.ConsentMarketing && student.ConsentDocumentProcessing && student.ConsentInternationalSharing && student.ConsentTermsAccepted))
            missing.Add("All consent fields");
        return missing;
    }

    private async Task AddStatusHistoryAsync(int studentId, int? oldStatus, int? newStatus, int changedBy, string? notes, CancellationToken cancellationToken)
    {
        if (!newStatus.HasValue) return;
        var history = new CrmStudentStatusHistory
        {
            StudentId = studentId,
            OldStatus = oldStatus,
            NewStatus = newStatus.Value,
            ChangedBy = changedBy,
            ChangedDate = DateTime.UtcNow,
            Notes = notes
        };
        await _repository.CrmStudentStatusHistories.CreateAsync(history, cancellationToken);
    }
}
