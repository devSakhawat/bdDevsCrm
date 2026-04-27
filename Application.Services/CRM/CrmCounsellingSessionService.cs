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

internal sealed class CrmCounsellingSessionService : ICrmCounsellingSessionService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmCounsellingSessionService> _logger;
    private readonly IConfiguration _config;

    public CrmCounsellingSessionService(IRepositoryManager repository, ILogger<CrmCounsellingSessionService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    public async Task<CrmCounsellingSessionDto> CreateAsync(CreateCrmCounsellingSessionRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmCounsellingSessionRecord));

        await ApplyOutcomeToLeadAsync(record.LeadId, record.Outcome, cancellationToken);
        var entity = record.MapTo<CrmCounsellingSession>();
        int newId = await _repository.CrmCounsellingSessions.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        _logger.LogInformation("CrmCounsellingSession created. ID: {Id}", newId);
        return entity.MapTo<CrmCounsellingSessionDto>() with { CounsellingSessionId = newId };
    }

    public async Task<CrmCounsellingSessionDto> UpdateAsync(UpdateCrmCounsellingSessionRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmCounsellingSessionRecord));
        _ = await _repository.CrmCounsellingSessions.CrmCounsellingSessionAsync(record.CounsellingSessionId, false, cancellationToken)
            ?? throw new NotFoundException("CrmCounsellingSession", "CounsellingSessionId", record.CounsellingSessionId.ToString());

        await ApplyOutcomeToLeadAsync(record.LeadId, record.Outcome, cancellationToken);
        var entity = record.MapTo<CrmCounsellingSession>();
        _repository.CrmCounsellingSessions.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmCounsellingSessionDto>();
    }

    public async Task DeleteAsync(DeleteCrmCounsellingSessionRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.CounsellingSessionId <= 0) throw new BadRequestException("Invalid delete request!");
        _ = await _repository.CrmCounsellingSessions.CrmCounsellingSessionAsync(record.CounsellingSessionId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CrmCounsellingSession", "CounsellingSessionId", record.CounsellingSessionId.ToString());
        await _repository.CrmCounsellingSessions.DeleteAsync(x => x.CounsellingSessionId == record.CounsellingSessionId, false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmCounsellingSessionDto> CounsellingSessionAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.CrmCounsellingSessions.CrmCounsellingSessionAsync(id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CrmCounsellingSession", "CounsellingSessionId", id.ToString());
        return entity.MapTo<CrmCounsellingSessionDto>();
    }

    public async Task<IEnumerable<CrmCounsellingSessionDto>> CounsellingSessionsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmCounsellingSessions.CounsellingSessionsAsync(trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmCounsellingSessionDto>() : Enumerable.Empty<CrmCounsellingSessionDto>();
    }

    public async Task<IEnumerable<CrmCounsellingSessionDto>> CounsellingSessionsByLeadIdAsync(int leadId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmCounsellingSessions.CounsellingSessionsByLeadIdAsync(leadId, trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmCounsellingSessionDto>() : Enumerable.Empty<CrmCounsellingSessionDto>();
    }

    public async Task<GridEntity<CrmCounsellingSessionDto>> CounsellingSessionsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT CounsellingSessionId, LeadId, BranchId, CounselorId, SessionDate, Duration, SessionType, Outcome, TargetIntake, IsDeleted FROM CrmCounsellingSession";
        const string orderBy = "SessionDate DESC";
        return await _repository.CrmCounsellingSessions.AdoGridDataAsync<CrmCounsellingSessionDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }

    public async Task<IEnumerable<CrmProgramEligibilityDto>> EligibleProgramsAsync(int studentId, CancellationToken cancellationToken = default)
    {
        var student = await _repository.CrmStudents.CrmStudentAsync(studentId, false, cancellationToken)
            ?? throw new NotFoundException("Student", "StudentId", studentId.ToString());
        var profile = (await _repository.CrmStudentAcademicProfiles.CrmStudentAcademicProfilesByStudentIdAsync(studentId, false, cancellationToken)).FirstOrDefault();
        var courses = await _repository.CrmCourses.CrmCoursesAsync(false, cancellationToken);
        var institutes = await _repository.CrmInstitutes.CrmInstitutesAsync(false, cancellationToken);

        var results = courses.Select(course =>
        {
            var institute = institutes.FirstOrDefault(x => x.InstituteId == course.InstituteId);
            var missing = new List<string>();
            var matched = new List<string>();

            if (student.PreferredDegreeLevelId.HasValue && course.DegreeLevelId.HasValue && student.PreferredDegreeLevelId == course.DegreeLevelId)
                matched.Add("Preferred degree level");
            else if (student.PreferredDegreeLevelId.HasValue && course.DegreeLevelId.HasValue)
                missing.Add("Degree level mismatch");

            var score = student.IeltsScore ?? profile?.CurrentEnglishScore;
            var requiredIelts = course.OverrideMinIelts ?? institute?.MinIeltsScore;
            if (requiredIelts.HasValue)
            {
                if (score.HasValue && score.Value >= requiredIelts.Value)
                    matched.Add("IELTS score");
                else
                    missing.Add($"IELTS {requiredIelts:0.0} required");
            }

            var academicResult = ParseAcademicScore(profile?.BachelorResult ?? profile?.MasterResult ?? profile?.HscResult);
            var requiredAcademic = course.OverrideMinAcademic ?? institute?.MinAcademicScore;
            if (requiredAcademic.HasValue)
            {
                if (academicResult.HasValue && academicResult.Value >= requiredAcademic.Value)
                    matched.Add("Academic score");
                else
                    missing.Add($"Academic score {requiredAcademic:0.##} required");
            }

            if (student.PreferredCountryId.HasValue && institute != null)
            {
                if (student.PreferredCountryId == institute.CountryId)
                    matched.Add("Preferred country");
                else
                    missing.Add("Country mismatch");
            }

            return new CrmProgramEligibilityDto
            {
                ProgramId = course.CourseId,
                ProgramName = course.CourseTitle ?? $"Program #{course.CourseId}",
                InstituteId = institute?.InstituteId ?? 0,
                InstituteName = institute?.InstituteName ?? "Unknown Institute",
                IsEligible = missing.Count == 0,
                MatchedCriteria = string.Join(", ", matched),
                MissingCriteria = string.Join(", ", missing)
            };
        }).OrderByDescending(x => x.IsEligible).ThenBy(x => x.ProgramName);

        return results.ToList();
    }

    private async Task ApplyOutcomeToLeadAsync(int leadId, byte outcome, CancellationToken cancellationToken)
    {
        var lead = await _repository.CrmLeads.CrmLeadAsync(leadId, false, cancellationToken);
        if (lead == null) return;

        string? targetStatusName = outcome switch
        {
            1 => "Interested",
            2 => "Qualified",
            3 => "Follow Up",
            4 => "Application Ready",
            5 => "Lost",
            _ => null
        };

        if (string.IsNullOrWhiteSpace(targetStatusName)) return;

        var status = (await _repository.CrmLeadStatuses.ListByConditionAsync(
            x => x.IsActive && x.StatusName.ToLower().Contains(targetStatusName.ToLower()),
            x => x.LeadStatusId,
            false,
            false,
            cancellationToken)).FirstOrDefault();

        if (status == null) return;

        lead.LeadStatusId = status.LeadStatusId;
        lead.UpdatedDate = DateTime.UtcNow;
        _repository.CrmLeads.UpdateByState(lead);
        await _repository.SaveAsync(cancellationToken);
    }

    private static decimal? ParseAcademicScore(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        return decimal.TryParse(value, out var parsed) ? parsed : null;
    }

}
