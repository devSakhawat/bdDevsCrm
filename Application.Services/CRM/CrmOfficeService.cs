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

/// <summary>
/// CrmOffice service implementing business logic for office management.
/// </summary>
internal sealed class CrmOfficeService : ICrmOfficeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmOfficeService> _logger;
    private readonly IConfiguration _config;

    public CrmOfficeService(IRepositoryManager repository, ILogger<CrmOfficeService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    /// <summary>Creates a new office record.</summary>
    public async Task<CrmOfficeDto> CreateAsync(CreateCrmOfficeRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateCrmOfficeRecord));

        var safeName = record.OfficeName?.Replace("\r", "\\r").Replace("\n", "\\n") ?? string.Empty;
        _logger.LogInformation("Creating new office. OfficeName: {OfficeName}, Time: {Time}", safeName, DateTime.UtcNow);

        bool exists = await _repository.CrmOffices.ExistsAsync(
            x => x.OfficeName.Trim().ToLower() == record.OfficeName.Trim().ToLower(),
            cancellationToken: cancellationToken);

        if (exists)
            throw new DuplicateRecordException("Office", "OfficeName");

        CrmOffice entity = record.MapTo<CrmOffice>();
        int newId = await _repository.CrmOffices.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Office created successfully. ID: {OfficeId}, Time: {Time}", newId, DateTime.UtcNow);

        return entity.MapTo<CrmOfficeDto>() with { OfficeId = newId };
    }

    /// <summary>Updates an existing office record.</summary>
    public async Task<CrmOfficeDto> UpdateAsync(UpdateCrmOfficeRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateCrmOfficeRecord));

        _logger.LogInformation("Updating office. ID: {OfficeId}, Time: {Time}", record.OfficeId, DateTime.UtcNow);

        _ = await _repository.CrmOffices
            .FirstOrDefaultAsync(x => x.OfficeId == record.OfficeId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("Office", "OfficeId", record.OfficeId.ToString());

        bool duplicateExists = await _repository.CrmOffices.ExistsAsync(
            x => x.OfficeName.Trim().ToLower() == record.OfficeName.Trim().ToLower()
                 && x.OfficeId != record.OfficeId,
            cancellationToken: cancellationToken);

        if (duplicateExists)
            throw new DuplicateRecordException("Office", "OfficeName");

        CrmOffice entity = record.MapTo<CrmOffice>();
        _repository.CrmOffices.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Office updated successfully. ID: {OfficeId}, Time: {Time}", record.OfficeId, DateTime.UtcNow);

        return entity.MapTo<CrmOfficeDto>();
    }

    /// <summary>Deletes an office record.</summary>
    public async Task DeleteAsync(DeleteCrmOfficeRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.OfficeId <= 0)
            throw new BadRequestException("Invalid delete request!");

        _logger.LogInformation("Deleting office. ID: {OfficeId}, Time: {Time}", record.OfficeId, DateTime.UtcNow);

        _ = await _repository.CrmOffices
            .FirstOrDefaultAsync(x => x.OfficeId == record.OfficeId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Office", "OfficeId", record.OfficeId.ToString());

        await _repository.CrmOffices.DeleteAsync(x => x.OfficeId == record.OfficeId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogWarning("Office deleted successfully. ID: {OfficeId}, Time: {Time}", record.OfficeId, DateTime.UtcNow);
    }

    /// <summary>Retrieves a single office record by ID.</summary>
    public async Task<CrmOfficeDto> OfficeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching office. ID: {OfficeId}, Time: {Time}", id, DateTime.UtcNow);

        var entity = await _repository.CrmOffices
            .FirstOrDefaultAsync(x => x.OfficeId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Office", "OfficeId", id.ToString());

        return entity.MapTo<CrmOfficeDto>();
    }

    /// <summary>Retrieves all office records.</summary>
    public async Task<IEnumerable<CrmOfficeDto>> OfficesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all offices. Time: {Time}", DateTime.UtcNow);

        var entities = await _repository.CrmOffices.CrmOfficesAsync(trackChanges, cancellationToken);

        if (!entities.Any())
        {
            _logger.LogWarning("No offices found. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmOfficeDto>();
        }

        _logger.LogInformation("Offices fetched successfully. Count: {Count}, Time: {Time}", entities.Count(), DateTime.UtcNow);
        return entities.MapToList<CrmOfficeDto>();
    }

    /// <summary>Retrieves a lightweight list of offices for dropdown binding.</summary>
    public async Task<IEnumerable<CrmOfficeDto>> OfficeForDDLAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching offices for DDL. Time: {Time}", DateTime.UtcNow);

        var entities = await _repository.CrmOffices.CrmOfficesAsync(false, cancellationToken);

        if (!entities.Any())
        {
            _logger.LogWarning("No offices found for DDL. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmOfficeDto>();
        }

        return entities.MapToList<CrmOfficeDto>();
    }

    /// <summary>Retrieves a paginated summary grid of offices.</summary>
    public async Task<GridEntity<CrmOfficeDto>> OfficesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching offices summary grid. Time: {Time}", DateTime.UtcNow);

        const string sql = @"SELECT OfficeId, OfficeName, OfficeCode, Address, City, Phone, Email, IsActive, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmOffice";
        const string orderBy = "OfficeName ASC";

        return await _repository.CrmOffices.AdoGridDataAsync<CrmOfficeDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }
}
