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

/// <summary>CrmFollowUp service implementing business logic for follow-up management.</summary>
internal sealed class CrmFollowUpService : ICrmFollowUpService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmFollowUpService> _logger;
    private readonly IConfiguration _config;

    public CrmFollowUpService(IRepositoryManager repository, ILogger<CrmFollowUpService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    /// <summary>Creates a new follow-up record.</summary>
    public async Task<CrmFollowUpDto> CreateAsync(CreateCrmFollowUpRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateCrmFollowUpRecord));

        _logger.LogInformation("Creating new FollowUp. Time: {Time}", DateTime.UtcNow);

        CrmFollowUp entity = record.MapTo<CrmFollowUp>();
        int newId = await _repository.CrmFollowUps.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("FollowUp created successfully. ID: {Id}, Time: {Time}", newId, DateTime.UtcNow);
        return entity.MapTo<CrmFollowUpDto>() with { FollowUpId = newId };
    }

    /// <summary>Updates an existing follow-up record.</summary>
    public async Task<CrmFollowUpDto> UpdateAsync(UpdateCrmFollowUpRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateCrmFollowUpRecord));

        _ = await _repository.CrmFollowUps
            .FirstOrDefaultAsync(x => x.FollowUpId == record.FollowUpId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("FollowUp", "FollowUpId", record.FollowUpId.ToString());

        _logger.LogInformation("Updating FollowUp. ID: {Id}, Time: {Time}", record.FollowUpId, DateTime.UtcNow);

        CrmFollowUp entity = record.MapTo<CrmFollowUp>();
        _repository.CrmFollowUps.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("FollowUp updated successfully. ID: {Id}, Time: {Time}", record.FollowUpId, DateTime.UtcNow);
        return entity.MapTo<CrmFollowUpDto>();
    }

    /// <summary>Deletes a follow-up record.</summary>
    public async Task DeleteAsync(DeleteCrmFollowUpRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.FollowUpId <= 0)
            throw new BadRequestException("Invalid delete request!");

        _ = await _repository.CrmFollowUps
            .FirstOrDefaultAsync(x => x.FollowUpId == record.FollowUpId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("FollowUp", "FollowUpId", record.FollowUpId.ToString());

        _logger.LogInformation("Deleting FollowUp. ID: {Id}, Time: {Time}", record.FollowUpId, DateTime.UtcNow);
        await _repository.CrmFollowUps.DeleteAsync(x => x.FollowUpId == record.FollowUpId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        _logger.LogWarning("FollowUp deleted successfully. ID: {Id}, Time: {Time}", record.FollowUpId, DateTime.UtcNow);
    }

    /// <summary>Retrieves a single follow-up record by ID.</summary>
    public async Task<CrmFollowUpDto> FollowUpAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching FollowUp. ID: {Id}, Time: {Time}", id, DateTime.UtcNow);
        var entity = await _repository.CrmFollowUps
            .FirstOrDefaultAsync(x => x.FollowUpId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("FollowUp", "FollowUpId", id.ToString());
        return entity.MapTo<CrmFollowUpDto>();
    }

    /// <summary>Retrieves all follow-up records.</summary>
    public async Task<IEnumerable<CrmFollowUpDto>> FollowUpsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all FollowUps. Time: {Time}", DateTime.UtcNow);
        var entities = await _repository.CrmFollowUps.CrmFollowUpsAsync(trackChanges, cancellationToken);
        if (!entities.Any())
        {
            _logger.LogWarning("No FollowUps found. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmFollowUpDto>();
        }
        _logger.LogInformation("FollowUps fetched. Count: {Count}, Time: {Time}", entities.Count(), DateTime.UtcNow);
        return entities.MapToList<CrmFollowUpDto>();
    }

    /// <summary>Retrieves a lightweight list of follow-ups for dropdown binding.</summary>
    public async Task<IEnumerable<CrmFollowUpDto>> FollowUpForDDLAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching FollowUps for DDL. Time: {Time}", DateTime.UtcNow);
        var entities = await _repository.CrmFollowUps.CrmFollowUpsAsync(false, cancellationToken);
        if (!entities.Any())
        {
            _logger.LogWarning("No FollowUps found for DDL. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmFollowUpDto>();
        }
        return entities.MapToList<CrmFollowUpDto>();
    }

    /// <summary>Retrieves a paginated summary grid of follow-ups.</summary>
    public async Task<GridEntity<CrmFollowUpDto>> FollowUpsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching FollowUps summary grid. Time: {Time}", DateTime.UtcNow);
        const string sql = @"SELECT FollowUpId, LeadId, EnquiryId, FollowUpDate, FollowUpType, Notes, NextFollowUpDate, IsCompleted, CounselorId, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmFollowUp";
        const string orderBy = "FollowUpDate DESC";
        return await _repository.CrmFollowUps.AdoGridDataAsync<CrmFollowUpDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }
}
