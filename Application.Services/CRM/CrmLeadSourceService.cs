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
/// CrmLeadSource service implementing business logic for lead source management.
/// </summary>
internal sealed class CrmLeadSourceService : ICrmLeadSourceService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmLeadSourceService> _logger;
    private readonly IConfiguration _config;

    public CrmLeadSourceService(IRepositoryManager repository, ILogger<CrmLeadSourceService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    /// <summary>Creates a new lead source record.</summary>
    public async Task<CrmLeadSourceDto> CreateAsync(CreateCrmLeadSourceRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateCrmLeadSourceRecord));

        var safeName = record.SourceName?.Replace("\r", "\\r").Replace("\n", "\\n") ?? string.Empty;
        _logger.LogInformation("Creating new lead source. SourceName: {SourceName}, Time: {Time}", safeName, DateTime.UtcNow);

        bool exists = await _repository.CrmLeadSources.ExistsAsync(
            x => x.SourceName.Trim().ToLower() == record.SourceName.Trim().ToLower(),
            cancellationToken: cancellationToken);

        if (exists)
            throw new DuplicateRecordException("LeadSource", "SourceName");

        CrmLeadSource entity = record.MapTo<CrmLeadSource>();
        int newId = await _repository.CrmLeadSources.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Lead source created successfully. ID: {LeadSourceId}, Time: {Time}", newId, DateTime.UtcNow);

        return entity.MapTo<CrmLeadSourceDto>() with { LeadSourceId = newId };
    }

    /// <summary>Updates an existing lead source record.</summary>
    public async Task<CrmLeadSourceDto> UpdateAsync(UpdateCrmLeadSourceRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateCrmLeadSourceRecord));

        _logger.LogInformation("Updating lead source. ID: {LeadSourceId}, Time: {Time}", record.LeadSourceId, DateTime.UtcNow);

        _ = await _repository.CrmLeadSources
            .FirstOrDefaultAsync(x => x.LeadSourceId == record.LeadSourceId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("LeadSource", "LeadSourceId", record.LeadSourceId.ToString());

        bool duplicateExists = await _repository.CrmLeadSources.ExistsAsync(
            x => x.SourceName.Trim().ToLower() == record.SourceName.Trim().ToLower()
                 && x.LeadSourceId != record.LeadSourceId,
            cancellationToken: cancellationToken);

        if (duplicateExists)
            throw new DuplicateRecordException("LeadSource", "SourceName");

        CrmLeadSource entity = record.MapTo<CrmLeadSource>();
        _repository.CrmLeadSources.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Lead source updated successfully. ID: {LeadSourceId}, Time: {Time}", record.LeadSourceId, DateTime.UtcNow);

        return entity.MapTo<CrmLeadSourceDto>();
    }

    /// <summary>Deletes a lead source record.</summary>
    public async Task DeleteAsync(DeleteCrmLeadSourceRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.LeadSourceId <= 0)
            throw new BadRequestException("Invalid delete request!");

        _logger.LogInformation("Deleting lead source. ID: {LeadSourceId}, Time: {Time}", record.LeadSourceId, DateTime.UtcNow);

        _ = await _repository.CrmLeadSources
            .FirstOrDefaultAsync(x => x.LeadSourceId == record.LeadSourceId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("LeadSource", "LeadSourceId", record.LeadSourceId.ToString());

        await _repository.CrmLeadSources.DeleteAsync(x => x.LeadSourceId == record.LeadSourceId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogWarning("Lead source deleted successfully. ID: {LeadSourceId}, Time: {Time}", record.LeadSourceId, DateTime.UtcNow);
    }

    /// <summary>Retrieves a single lead source record by ID.</summary>
    public async Task<CrmLeadSourceDto> LeadSourceAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching lead source. ID: {LeadSourceId}, Time: {Time}", id, DateTime.UtcNow);

        var entity = await _repository.CrmLeadSources
            .FirstOrDefaultAsync(x => x.LeadSourceId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("LeadSource", "LeadSourceId", id.ToString());

        return entity.MapTo<CrmLeadSourceDto>();
    }

    /// <summary>Retrieves all lead source records.</summary>
    public async Task<IEnumerable<CrmLeadSourceDto>> LeadSourcesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all lead sources. Time: {Time}", DateTime.UtcNow);

        var entities = await _repository.CrmLeadSources.CrmLeadSourcesAsync(trackChanges, cancellationToken);

        if (!entities.Any())
        {
            _logger.LogWarning("No lead sources found. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmLeadSourceDto>();
        }

        _logger.LogInformation("Lead sources fetched successfully. Count: {Count}, Time: {Time}", entities.Count(), DateTime.UtcNow);
        return entities.MapToList<CrmLeadSourceDto>();
    }

    /// <summary>Retrieves a lightweight list of lead sources for dropdown binding.</summary>
    public async Task<IEnumerable<CrmLeadSourceDto>> LeadSourceForDDLAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching lead sources for DDL. Time: {Time}", DateTime.UtcNow);

        var entities = await _repository.CrmLeadSources.CrmLeadSourcesAsync(false, cancellationToken);

        if (!entities.Any())
        {
            _logger.LogWarning("No lead sources found for DDL. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmLeadSourceDto>();
        }

        return entities.MapToList<CrmLeadSourceDto>();
    }

    /// <summary>Retrieves a paginated summary grid of lead sources.</summary>
    public async Task<GridEntity<CrmLeadSourceDto>> LeadSourcesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching lead sources summary grid. Time: {Time}", DateTime.UtcNow);

        const string sql = @"SELECT LeadSourceId, SourceName, SourceCode, IsActive, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmLeadSource";
        const string orderBy = "SourceName ASC";

        return await _repository.CrmLeadSources.AdoGridDataAsync<CrmLeadSourceDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }
}
