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
using Domain.Entities.Entities.DMS;

namespace Application.Services.CRM;

internal sealed class CrmStudentService : ICrmStudentService
{
    private const string InterestedLeadStatusName = "Interested";
    private const decimal DefaultBudgetThresholdBdt = 50000m;
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

        if (existing.StudentStatusId != entity.StudentStatusId && entity.StudentStatusId.HasValue)
            await AddStatusHistoryAsync(entity.StudentId, existing.StudentStatusId, entity.StudentStatusId, entity.UpdatedBy ?? entity.CreatedBy, "Status changed from update", cancellationToken);

        await _repository.SaveAsync(cancellationToken);

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


    public async Task<ConvertToStudentResultDto> EvaluateLeadConversionAsync(ConvertToStudentRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request == null) throw new BadRequestException(nameof(ConvertToStudentRequestDto));
        return await EvaluateLeadConversionInternalAsync(request, cancellationToken);
    }

    public async Task<ConvertToStudentResultDto> ConvertLeadToStudentAsync(ConvertToStudentRequestDto request, CancellationToken cancellationToken = default)
    {
        if (request == null) throw new BadRequestException(nameof(ConvertToStudentRequestDto));
        var evaluation = await EvaluateLeadConversionInternalAsync(request, cancellationToken);
        if (evaluation.HardErrors.Any())
        {
            evaluation.ResultType = "HARD_BLOCK";
            evaluation.CanConvert = false;
            return evaluation;
        }

        if (evaluation.SoftWarnings.Any() && !request.ForceProceed)
        {
            evaluation.ResultType = "SOFT_WARNING";
            evaluation.CanConvert = false;
            evaluation.Message = "Conversion has warnings. Confirm to proceed.";
            return evaluation;
        }

        var lead = await _repository.CrmLeads.FirstOrDefaultAsync(x => x.LeadId == request.LeadId, true, cancellationToken)
            ?? throw new NotFoundException("Lead", "LeadId", request.LeadId.ToString());
        var requestedBy = request.RequestedBy ?? 1;

        await _repository.CrmStudents.TransactionBeginAsync(cancellationToken);
        try
        {
            var studentStatus = (await _repository.CrmStudentStatuses.ListByConditionAsync(x => x.IsActive && x.StatusName.ToLower().Contains("new"), x => x.StudentStatusId, false, false, cancellationToken)).FirstOrDefault()
                ?? (await _repository.CrmStudentStatuses.ListAsync(x => x.StudentStatusId, false, cancellationToken)).FirstOrDefault();

            var student = new CrmStudent
            {
                LeadId = lead.LeadId,
                StudentName = lead.LeadName,
                StudentCode = $"STD-{DateTime.UtcNow:yyyyMMddHHmmssfff}-{Guid.NewGuid():N}"[..39],
                Email = lead.Email,
                Phone = lead.Phone,
                CounselorId = lead.AssignedCounselorId,
                AgentId = lead.AgentId,
                BranchId = lead.BranchId,
                ProcessingOfficerId = request.ProcessingOfficerId,
                PreferredCountryId = request.PreferredCountryId ?? lead.InterestedCountryId,
                PreferredDegreeLevelId = request.PreferredDegreeLevelId ?? lead.InterestedDegreeLevelId,
                DesiredIntake = string.IsNullOrWhiteSpace(request.DesiredIntake) ? null : request.DesiredIntake,
                PassportNumber = string.IsNullOrWhiteSpace(request.PassportNumber) ? null : request.PassportNumber,
                StudentStatusId = studentStatus?.StudentStatusId,
                IsApplicationReady = false,
                IsDeleted = false,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = requestedBy
            };

            int studentId = await _repository.CrmStudents.CreateAndIdAsync(student, cancellationToken);
            student.StudentId = studentId;

            await _repository.CrmStudentAcademicProfiles.CreateAsync(new CrmStudentAcademicProfile
            {
                StudentId = studentId,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = requestedBy
            }, cancellationToken);

            var convertedStatus = (await _repository.CrmLeadStatuses.ListByConditionAsync(x => x.IsActive && x.StatusName.ToLower().Contains("converted"), x => x.LeadStatusId, false, false, cancellationToken)).FirstOrDefault();
            if (convertedStatus != null)
            {
                lead.LeadStatusId = convertedStatus.LeadStatusId;
            }
            lead.UpdatedBy = requestedBy;
            lead.UpdatedDate = DateTime.UtcNow;
            _repository.CrmLeads.UpdateByState(lead);

            await GenerateDefaultDocumentChecklistAsyncInternal(student, requestedBy, cancellationToken);
            await _repository.SaveAsync(cancellationToken);
            await _repository.CrmStudents.TransactionCommitAsync(cancellationToken);

            _logger.LogInformation("Welcome notification queued for StudentId {StudentId}", studentId);
            _logger.LogInformation("Processing officer assignment notification queued for StudentId {StudentId}", studentId);

            evaluation.StudentId = studentId;
            evaluation.CanConvert = true;
            evaluation.ResultType = "SUCCESS";
            evaluation.Message = "Lead converted to student successfully.";
            return evaluation;
        }
        catch
        {
            await _repository.CrmStudents.TransactionRollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            await _repository.CrmStudents.TransactionDisposeAsync();
        }
    }

    private async Task<ConvertToStudentResultDto> EvaluateLeadConversionInternalAsync(ConvertToStudentRequestDto request, CancellationToken cancellationToken)
    {
        var result = new ConvertToStudentResultDto { LeadId = request.LeadId };
        var lead = await _repository.CrmLeads.FirstOrDefaultAsync(x => x.LeadId == request.LeadId, false, cancellationToken);
        if (lead == null)
            throw new NotFoundException("Lead", "LeadId", request.LeadId.ToString());
        if (lead.IsDeleted)
            throw new BadRequestException("Lead is deleted and cannot be converted.");

        var interestedStatus = lead.LeadStatusId.HasValue
            ? await _repository.CrmLeadStatuses.FirstOrDefaultAsync(x => x.LeadStatusId == lead.LeadStatusId.Value, false, cancellationToken)
            : null;
        if (!string.Equals(interestedStatus?.StatusName, InterestedLeadStatusName, StringComparison.OrdinalIgnoreCase))
            throw new BadRequestException("Only interested leads can be converted to students.");

        var sessions = (await _repository.CrmCounsellingSessions.CounsellingSessionsByLeadIdAsync(request.LeadId, false, cancellationToken)).Where(x => !x.IsDeleted).ToList();
        if (sessions.Any(x => x.SessionDate.Date >= DateTime.UtcNow.Date))
            result.HardErrors.Add("An active counselling session exists for this lead.");
        if (!sessions.Any(x => x.SessionDate.Date < DateTime.UtcNow.Date && x.Outcome > 0))
            result.HardErrors.Add("At least one completed counselling session is required before conversion.");

        if (!string.IsNullOrWhiteSpace(lead.Phone))
        {
            var duplicateStudent = (await _repository.CrmStudents.CrmStudentsAsync(false, cancellationToken)).FirstOrDefault(x => !x.IsDeleted && x.Phone == lead.Phone);
            if (duplicateStudent != null)
            {
                result.HardErrors.Add("Student phone must be unique across students.");
                result.ExistingStudentId = duplicateStudent.StudentId;
            }
        }

        if (string.IsNullOrWhiteSpace(request.PassportNumber))
            result.SoftWarnings.Add("Passport information is missing.");
        if (!(request.PreferredCountryId ?? lead.InterestedCountryId).HasValue)
            result.SoftWarnings.Add("Preferred country is not set.");

        var budgetThreshold = _config.GetValue<decimal?>("CrmConversion:BudgetThreshold") ?? DefaultBudgetThresholdBdt;
        if ((lead.Budget ?? 0m) > 0m && lead.Budget.Value < budgetThreshold)
            result.SoftWarnings.Add($"Lead budget is below the threshold of {budgetThreshold:0.##}.");

        result.CanConvert = !result.HardErrors.Any() && !result.SoftWarnings.Any();
        result.Message = result.HardErrors.Any() ? "Conversion is blocked." : result.SoftWarnings.Any() ? "Conversion has warnings." : "Lead is ready for conversion.";
        return result;
    }

    private async Task GenerateDefaultDocumentChecklistAsyncInternal(CrmStudent student, int requestedBy, CancellationToken cancellationToken)
    {
        var requirements = (await _repository.CrmCountryDocumentRequirements.RequirementsByCountryIdAsync(student.PreferredCountryId ?? 0, false, cancellationToken))
            .Where(x => !student.PreferredDegreeLevelId.HasValue || x.DegreeLevelId == student.PreferredDegreeLevelId.Value)
            .ToList();
        var documentTypes = (await _repository.DmsDocumentTypes.DocumentTypesAsync(false, cancellationToken)).ToList();

        var selectedDocumentTypes = new List<DmsDocumentType>();
        foreach (var requirement in requirements)
        {
            var docType = documentTypes.FirstOrDefault(x => x.Name.Equals(requirement.DocumentTypeName, StringComparison.OrdinalIgnoreCase));
            if (docType != null && selectedDocumentTypes.All(x => x.DocumentTypeId != docType.DocumentTypeId))
                selectedDocumentTypes.Add(docType);
        }
        if (!selectedDocumentTypes.Any())
            selectedDocumentTypes = documentTypes.Where(x => x.IsMandatory).ToList();

        foreach (var docType in selectedDocumentTypes)
        {
            await _repository.CrmStudentDocumentChecklists.CreateAsync(new CrmStudentDocumentChecklist
            {
                StudentId = student.StudentId,
                DocumentTypeId = docType.DocumentTypeId,
                IsMandatory = docType.IsMandatory,
                IsSubmitted = false,
                IsVerified = false,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = requestedBy
            }, cancellationToken);
        }
        _logger.LogInformation("Generated default document checklist for StudentId {StudentId}", student.StudentId);
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
