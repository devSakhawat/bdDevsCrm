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

internal sealed class CrmStudentAcademicProfileService : ICrmStudentAcademicProfileService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmStudentAcademicProfileService> _logger;
    private readonly IConfiguration _config;

    public CrmStudentAcademicProfileService(IRepositoryManager repository, ILogger<CrmStudentAcademicProfileService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    public async Task<CrmStudentAcademicProfileDto> CreateAsync(CreateCrmStudentAcademicProfileRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmStudentAcademicProfileRecord));

        bool exists = await _repository.CrmStudentAcademicProfiles.ExistsAsync(x => x.StudentId == record.StudentId, cancellationToken: cancellationToken);
        if (exists) throw new DuplicateRecordException("StudentAcademicProfile", "StudentId");
        var entity = record.MapTo<CrmStudentAcademicProfile>();
        int newId = await _repository.CrmStudentAcademicProfiles.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        _logger.LogInformation("CrmStudentAcademicProfile created. ID: {Id}", newId);
        return entity.MapTo<CrmStudentAcademicProfileDto>() with { StudentAcademicProfileId = newId };
    }

    public async Task<CrmStudentAcademicProfileDto> UpdateAsync(UpdateCrmStudentAcademicProfileRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmStudentAcademicProfileRecord));
        _ = await _repository.CrmStudentAcademicProfiles.CrmStudentAcademicProfileAsync(record.StudentAcademicProfileId, false, cancellationToken)
            ?? throw new NotFoundException("CrmStudentAcademicProfile", "StudentAcademicProfileId", record.StudentAcademicProfileId.ToString());

        bool duplicate = await _repository.CrmStudentAcademicProfiles.ExistsAsync(x => x.StudentId == record.StudentId && x.StudentAcademicProfileId != record.StudentAcademicProfileId, cancellationToken: cancellationToken);
        if (duplicate) throw new DuplicateRecordException("StudentAcademicProfile", "StudentId");
        var entity = record.MapTo<CrmStudentAcademicProfile>();
        _repository.CrmStudentAcademicProfiles.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmStudentAcademicProfileDto>();
    }

    public async Task DeleteAsync(DeleteCrmStudentAcademicProfileRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.StudentAcademicProfileId <= 0) throw new BadRequestException("Invalid delete request!");
        _ = await _repository.CrmStudentAcademicProfiles.CrmStudentAcademicProfileAsync(record.StudentAcademicProfileId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CrmStudentAcademicProfile", "StudentAcademicProfileId", record.StudentAcademicProfileId.ToString());
        await _repository.CrmStudentAcademicProfiles.DeleteAsync(x => x.StudentAcademicProfileId == record.StudentAcademicProfileId, false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmStudentAcademicProfileDto> StudentAcademicProfileAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.CrmStudentAcademicProfiles.CrmStudentAcademicProfileAsync(id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CrmStudentAcademicProfile", "StudentAcademicProfileId", id.ToString());
        return entity.MapTo<CrmStudentAcademicProfileDto>();
    }

    public async Task<IEnumerable<CrmStudentAcademicProfileDto>> StudentAcademicProfilesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmStudentAcademicProfiles.StudentAcademicProfilesAsync(trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmStudentAcademicProfileDto>() : Enumerable.Empty<CrmStudentAcademicProfileDto>();
    }

    public async Task<IEnumerable<CrmStudentAcademicProfileDto>> StudentAcademicProfilesByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmStudentAcademicProfiles.StudentAcademicProfilesByStudentIdAsync(studentId, trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmStudentAcademicProfileDto>() : Enumerable.Empty<CrmStudentAcademicProfileDto>();
    }

    public async Task<GridEntity<CrmStudentAcademicProfileDto>> StudentAcademicProfilesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT StudentAcademicProfileId, StudentId, SscResult, HscResult, BachelorResult, MasterResult, PhdResult, CurrentEnglishProficiency FROM CrmStudentAcademicProfile";
        const string orderBy = "StudentAcademicProfileId DESC";
        return await _repository.CrmStudentAcademicProfiles.AdoGridDataAsync<CrmStudentAcademicProfileDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }

}
