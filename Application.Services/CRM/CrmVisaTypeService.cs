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
/// CrmVisaType service implementing business logic for visa type management.
/// </summary>
internal sealed class CrmVisaTypeService : ICrmVisaTypeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmVisaTypeService> _logger;
    private readonly IConfiguration _config;

    public CrmVisaTypeService(IRepositoryManager repository, ILogger<CrmVisaTypeService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    /// <summary>Creates a new visa type record.</summary>
    public async Task<CrmVisaTypeDto> CreateAsync(CreateCrmVisaTypeRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateCrmVisaTypeRecord));

        _logger.LogInformation("Creating new visa type. VisaTypeName: {VisaTypeName}, Time: {Time}", record.VisaTypeName, DateTime.UtcNow);

        bool exists = await _repository.CrmVisaTypes.ExistsAsync(
            x => x.VisaTypeName.Trim().ToLower() == record.VisaTypeName.Trim().ToLower(),
            cancellationToken: cancellationToken);

        if (exists)
            throw new DuplicateRecordException("VisaType", "VisaTypeName");

        CrmVisaType entity = record.MapTo<CrmVisaType>();
        int newId = await _repository.CrmVisaTypes.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Visa type created successfully. ID: {VisaTypeId}, Time: {Time}", newId, DateTime.UtcNow);

        return entity.MapTo<CrmVisaTypeDto>() with { VisaTypeId = newId };
    }

    /// <summary>Updates an existing visa type record.</summary>
    public async Task<CrmVisaTypeDto> UpdateAsync(UpdateCrmVisaTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateCrmVisaTypeRecord));

        _logger.LogInformation("Updating visa type. ID: {VisaTypeId}, Time: {Time}", record.VisaTypeId, DateTime.UtcNow);

        _ = await _repository.CrmVisaTypes
            .FirstOrDefaultAsync(x => x.VisaTypeId == record.VisaTypeId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("VisaType", "VisaTypeId", record.VisaTypeId.ToString());

        bool duplicateExists = await _repository.CrmVisaTypes.ExistsAsync(
            x => x.VisaTypeName.Trim().ToLower() == record.VisaTypeName.Trim().ToLower()
                 && x.VisaTypeId != record.VisaTypeId,
            cancellationToken: cancellationToken);

        if (duplicateExists)
            throw new DuplicateRecordException("VisaType", "VisaTypeName");

        CrmVisaType entity = record.MapTo<CrmVisaType>();
        _repository.CrmVisaTypes.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Visa type updated successfully. ID: {VisaTypeId}, Time: {Time}", record.VisaTypeId, DateTime.UtcNow);

        return entity.MapTo<CrmVisaTypeDto>();
    }

    /// <summary>Deletes a visa type record.</summary>
    public async Task DeleteAsync(DeleteCrmVisaTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.VisaTypeId <= 0)
            throw new BadRequestException("Invalid delete request!");

        _logger.LogInformation("Deleting visa type. ID: {VisaTypeId}, Time: {Time}", record.VisaTypeId, DateTime.UtcNow);

        _ = await _repository.CrmVisaTypes
            .FirstOrDefaultAsync(x => x.VisaTypeId == record.VisaTypeId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("VisaType", "VisaTypeId", record.VisaTypeId.ToString());

        await _repository.CrmVisaTypes.DeleteAsync(x => x.VisaTypeId == record.VisaTypeId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogWarning("Visa type deleted successfully. ID: {VisaTypeId}, Time: {Time}", record.VisaTypeId, DateTime.UtcNow);
    }

    /// <summary>Retrieves a single visa type record by ID.</summary>
    public async Task<CrmVisaTypeDto> VisaTypeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching visa type. ID: {VisaTypeId}, Time: {Time}", id, DateTime.UtcNow);

        var entity = await _repository.CrmVisaTypes
            .FirstOrDefaultAsync(x => x.VisaTypeId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("VisaType", "VisaTypeId", id.ToString());

        return entity.MapTo<CrmVisaTypeDto>();
    }

    /// <summary>Retrieves all visa type records.</summary>
    public async Task<IEnumerable<CrmVisaTypeDto>> VisaTypesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all visa types. Time: {Time}", DateTime.UtcNow);

        var entities = await _repository.CrmVisaTypes.CrmVisaTypesAsync(trackChanges, cancellationToken);

        if (!entities.Any())
        {
            _logger.LogWarning("No visa types found. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmVisaTypeDto>();
        }

        _logger.LogInformation("Visa types fetched successfully. Count: {Count}, Time: {Time}", entities.Count(), DateTime.UtcNow);
        return entities.MapToList<CrmVisaTypeDto>();
    }

    /// <summary>Retrieves a lightweight list of visa types for dropdown binding.</summary>
    public async Task<IEnumerable<CrmVisaTypeDto>> VisaTypeForDDLAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching visa types for DDL. Time: {Time}", DateTime.UtcNow);

        var entities = await _repository.CrmVisaTypes.CrmVisaTypesAsync(false, cancellationToken);

        if (!entities.Any())
        {
            _logger.LogWarning("No visa types found for DDL. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmVisaTypeDto>();
        }

        return entities.MapToList<CrmVisaTypeDto>();
    }

    /// <summary>Retrieves a paginated summary grid of visa types.</summary>
    public async Task<GridEntity<CrmVisaTypeDto>> VisaTypesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching visa types summary grid. Time: {Time}", DateTime.UtcNow);

        const string sql = @"SELECT VisaTypeId, VisaTypeName, VisaCode, Description, IsActive, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmVisaType";
        const string orderBy = "VisaTypeName ASC";

        return await _repository.CrmVisaTypes.AdoGridDataAsync<CrmVisaTypeDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }
}
