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
/// CrmLeadStatus service implementing business logic for lead status management.
/// </summary>
internal sealed class CrmLeadStatusService : ICrmLeadStatusService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmLeadStatusService> _logger;
    private readonly IConfiguration _config;

    public CrmLeadStatusService(IRepositoryManager repository, ILogger<CrmLeadStatusService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    /// <summary>Creates a new lead status record.</summary>
    public async Task<CrmLeadStatusDto> CreateAsync(CreateCrmLeadStatusRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateCrmLeadStatusRecord));

        var safeName = record.StatusName?.Replace("\r", "\\r").Replace("\n", "\\n") ?? string.Empty;
        _logger.LogInformation("Creating new lead status. StatusName: {StatusName}, Time: {Time}", safeName, DateTime.UtcNow);

        bool exists = await _repository.CrmLeadStatuses.ExistsAsync(
            x => x.StatusName.Trim().ToLower() == record.StatusName.Trim().ToLower(),
            cancellationToken: cancellationToken);

        if (exists)
            throw new DuplicateRecordException("LeadStatus", "StatusName");

        CrmLeadStatus entity = record.MapTo<CrmLeadStatus>();
        int newId = await _repository.CrmLeadStatuses.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Lead status created successfully. ID: {LeadStatusId}, Time: {Time}", newId, DateTime.UtcNow);

        return entity.MapTo<CrmLeadStatusDto>() with { LeadStatusId = newId };
    }

    /// <summary>Updates an existing lead status record.</summary>
    public async Task<CrmLeadStatusDto> UpdateAsync(UpdateCrmLeadStatusRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateCrmLeadStatusRecord));

        _logger.LogInformation("Updating lead status. ID: {LeadStatusId}, Time: {Time}", record.LeadStatusId, DateTime.UtcNow);

        _ = await _repository.CrmLeadStatuses
            .FirstOrDefaultAsync(x => x.LeadStatusId == record.LeadStatusId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("LeadStatus", "LeadStatusId", record.LeadStatusId.ToString());

        bool duplicateExists = await _repository.CrmLeadStatuses.ExistsAsync(
            x => x.StatusName.Trim().ToLower() == record.StatusName.Trim().ToLower()
                 && x.LeadStatusId != record.LeadStatusId,
            cancellationToken: cancellationToken);

        if (duplicateExists)
            throw new DuplicateRecordException("LeadStatus", "StatusName");

        CrmLeadStatus entity = record.MapTo<CrmLeadStatus>();
        _repository.CrmLeadStatuses.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Lead status updated successfully. ID: {LeadStatusId}, Time: {Time}", record.LeadStatusId, DateTime.UtcNow);

        return entity.MapTo<CrmLeadStatusDto>();
    }

    /// <summary>Deletes a lead status record.</summary>
    public async Task DeleteAsync(DeleteCrmLeadStatusRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.LeadStatusId <= 0)
            throw new BadRequestException("Invalid delete request!");

        _logger.LogInformation("Deleting lead status. ID: {LeadStatusId}, Time: {Time}", record.LeadStatusId, DateTime.UtcNow);

        _ = await _repository.CrmLeadStatuses
            .FirstOrDefaultAsync(x => x.LeadStatusId == record.LeadStatusId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("LeadStatus", "LeadStatusId", record.LeadStatusId.ToString());

        await _repository.CrmLeadStatuses.DeleteAsync(x => x.LeadStatusId == record.LeadStatusId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogWarning("Lead status deleted successfully. ID: {LeadStatusId}, Time: {Time}", record.LeadStatusId, DateTime.UtcNow);
    }

    /// <summary>Retrieves a single lead status record by ID.</summary>
    public async Task<CrmLeadStatusDto> LeadStatusAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching lead status. ID: {LeadStatusId}, Time: {Time}", id, DateTime.UtcNow);

        var entity = await _repository.CrmLeadStatuses
            .FirstOrDefaultAsync(x => x.LeadStatusId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("LeadStatus", "LeadStatusId", id.ToString());

        return entity.MapTo<CrmLeadStatusDto>();
    }

    /// <summary>Retrieves all lead status records.</summary>
    public async Task<IEnumerable<CrmLeadStatusDto>> LeadStatusesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all lead statuses. Time: {Time}", DateTime.UtcNow);

        var entities = await _repository.CrmLeadStatuses.CrmLeadStatusesAsync(trackChanges, cancellationToken);

        if (!entities.Any())
        {
            _logger.LogWarning("No lead statuses found. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmLeadStatusDto>();
        }

        _logger.LogInformation("Lead statuses fetched successfully. Count: {Count}, Time: {Time}", entities.Count(), DateTime.UtcNow);
        return entities.MapToList<CrmLeadStatusDto>();
    }

    /// <summary>Retrieves a lightweight list of lead statuses for dropdown binding.</summary>
    public async Task<IEnumerable<CrmLeadStatusDto>> LeadStatusForDDLAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching lead statuses for DDL. Time: {Time}", DateTime.UtcNow);

        var entities = await _repository.CrmLeadStatuses.CrmLeadStatusesAsync(false, cancellationToken);

        if (!entities.Any())
        {
            _logger.LogWarning("No lead statuses found for DDL. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmLeadStatusDto>();
        }

        return entities.MapToList<CrmLeadStatusDto>();
    }

    /// <summary>Retrieves a paginated summary grid of lead statuses.</summary>
    public async Task<GridEntity<CrmLeadStatusDto>> LeadStatusesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching lead statuses summary grid. Time: {Time}", DateTime.UtcNow);

        const string sql = @"SELECT LeadStatusId, StatusName, StatusCode, ColorCode, IsActive, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmLeadStatus";
        const string orderBy = "StatusName ASC";

        return await _repository.CrmLeadStatuses.AdoGridDataAsync<CrmLeadStatusDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }
}
