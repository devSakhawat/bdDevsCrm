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

internal sealed class CrmFacultyService : ICrmFacultyService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmFacultyService> _logger;
    private readonly IConfiguration _config;

    public CrmFacultyService(IRepositoryManager repository, ILogger<CrmFacultyService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    public async Task<CrmFacultyDto> CreateAsync(CreateCrmFacultyRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmFacultyRecord));

        bool exists = await _repository.CrmFaculties.ExistsAsync(
            x => x.FacultyName.Trim().ToLower() == record.FacultyName.Trim().ToLower() && x.InstituteId == record.InstituteId, cancellationToken: cancellationToken);
        if (exists) throw new DuplicateRecordException("Faculty", "FacultyName");

        CrmFaculty entity = record.MapTo<CrmFaculty>();
        int newId = await _repository.CrmFaculties.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Faculty created. ID: {Id}, Time: {Time}", newId, DateTime.UtcNow);
        return entity.MapTo<CrmFacultyDto>() with { FacultyId = newId };
    }

    public async Task<CrmFacultyDto> UpdateAsync(UpdateCrmFacultyRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmFacultyRecord));

        _ = await _repository.CrmFaculties
            .FirstOrDefaultAsync(x => x.FacultyId == record.FacultyId, false, cancellationToken)
            ?? throw new NotFoundException("Faculty", "FacultyId", record.FacultyId.ToString());

        bool dup = await _repository.CrmFaculties.ExistsAsync(
            x => x.FacultyName.Trim().ToLower() == record.FacultyName.Trim().ToLower()
              && x.InstituteId == record.InstituteId && x.FacultyId != record.FacultyId, cancellationToken: cancellationToken);
        if (dup) throw new DuplicateRecordException("Faculty", "FacultyName");

        CrmFaculty entity = record.MapTo<CrmFaculty>();
        _repository.CrmFaculties.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmFacultyDto>();
    }

    public async Task DeleteAsync(DeleteCrmFacultyRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.FacultyId <= 0) throw new BadRequestException("Invalid delete request!");

        _ = await _repository.CrmFaculties
            .FirstOrDefaultAsync(x => x.FacultyId == record.FacultyId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Faculty", "FacultyId", record.FacultyId.ToString());

        await _repository.CrmFaculties.DeleteAsync(x => x.FacultyId == record.FacultyId, false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmFacultyDto> FacultyAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.CrmFaculties
            .FirstOrDefaultAsync(x => x.FacultyId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Faculty", "FacultyId", id.ToString());
        return entity.MapTo<CrmFacultyDto>();
    }

    public async Task<IEnumerable<CrmFacultyDto>> FacultiesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmFaculties.CrmFacultiesAsync(trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmFacultyDto>() : Enumerable.Empty<CrmFacultyDto>();
    }

    public async Task<IEnumerable<CrmFacultyDto>> FacultiesForDDLAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmFaculties.CrmFacultiesAsync(false, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmFacultyDto>() : Enumerable.Empty<CrmFacultyDto>();
    }

    public async Task<IEnumerable<CrmFacultyDto>> FacultiesByInstituteIdAsync(int instituteId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmFaculties.CrmFacultiesByInstituteIdAsync(instituteId, trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmFacultyDto>() : Enumerable.Empty<CrmFacultyDto>();
    }

    public async Task<GridEntity<CrmFacultyDto>> FacultiesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT FacultyId, InstituteId, FacultyName, IsActive, CreatedDate, CreatedBy FROM CrmFaculty";
        const string orderBy = "FacultyName ASC";
        return await _repository.CrmFaculties.AdoGridDataAsync<CrmFacultyDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }
}
