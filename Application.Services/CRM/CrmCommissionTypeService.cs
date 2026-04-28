using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Extensions;
using bdDevs.Shared.Records.CRM;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services.CRM;
using Domain.Entities.Entities.CRM;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Application.Services.CRM;

internal sealed class CrmCommissionTypeService : ICrmCommissionTypeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmCommissionTypeService> _logger;
    public CrmCommissionTypeService(IRepositoryManager repository, ILogger<CrmCommissionTypeService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<CrmCommissionTypeDto> CreateAsync(CreateCrmCommissionTypeRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateCrmCommissionTypeRecord));

        bool exists = await _repository.CrmCommissionTypes.ExistsAsync(x => x.CommissionTypeName.Trim().ToLower() == record.CommissionTypeName.Trim().ToLower(), cancellationToken: cancellationToken);
        if (exists)
            throw new ConflictException("Commission Type with this value already exists!");

        CrmCommissionType entity = record.MapTo<CrmCommissionType>();
        int newId = await _repository.CrmCommissionTypes.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Commission Type created successfully. ID: {CommissionTypeId}", newId);

        return entity.MapTo<CrmCommissionTypeDto>() with { CommissionTypeId = newId };
    }

    public async Task<CrmCommissionTypeDto> UpdateAsync(UpdateCrmCommissionTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateCrmCommissionTypeRecord));

        _ = await _repository.CrmCommissionTypes.FirstOrDefaultAsync(x => x.CommissionTypeId == record.CommissionTypeId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("Commission Type", "CommissionTypeId", record.CommissionTypeId.ToString());

        bool duplicateExists = await _repository.CrmCommissionTypes.ExistsAsync(x => x.CommissionTypeName.Trim().ToLower() == record.CommissionTypeName.Trim().ToLower() && x.CommissionTypeId != record.CommissionTypeId, cancellationToken: cancellationToken);
        if (duplicateExists)
            throw new ConflictException("Commission Type with this value already exists!");

        CrmCommissionType entity = record.MapTo<CrmCommissionType>();
        _repository.CrmCommissionTypes.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Commission Type updated successfully. ID: {CommissionTypeId}", record.CommissionTypeId);

        return entity.MapTo<CrmCommissionTypeDto>();
    }

    public async Task DeleteAsync(DeleteCrmCommissionTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.CommissionTypeId <= 0)
            throw new BadRequestException("Invalid delete request!");

        var entity = await _repository.CrmCommissionTypes.FirstOrDefaultAsync(x => x.CommissionTypeId == record.CommissionTypeId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("Commission Type", "CommissionTypeId", record.CommissionTypeId.ToString());

        await _repository.CrmCommissionTypes.DeleteAsync(x => x.CommissionTypeId == record.CommissionTypeId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogWarning("Commission Type deleted successfully. ID: {CommissionTypeId}", record.CommissionTypeId);
    }

    public async Task<CrmCommissionTypeDto> CommissionTypeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.CrmCommissionTypes.FirstOrDefaultAsync(x => x.CommissionTypeId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Commission Type", "CommissionTypeId", id.ToString());

        return entity.MapTo<CrmCommissionTypeDto>();
    }

    public async Task<IEnumerable<CrmCommissionTypeDto>> CommissionTypesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmCommissionTypes.CommissionTypesAsync(trackChanges, cancellationToken);
        if (!entities.Any())
        {
            _logger.LogWarning("No Commission Type records found.");
            return Enumerable.Empty<CrmCommissionTypeDto>();
        }

        return entities.MapToList<CrmCommissionTypeDto>();
    }

    public async Task<IEnumerable<CrmCommissionTypeDDLDto>> CommissionTypeForDDLAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmCommissionTypes.ListByConditionAsync(x => true, x => x.CommissionTypeName, trackChanges: false, descending: false, cancellationToken: cancellationToken);
        if (!entities.Any())
        {
            _logger.LogWarning("No Commission Type records found for dropdown.");
            return Enumerable.Empty<CrmCommissionTypeDDLDto>();
        }

        return entities.MapToList<CrmCommissionTypeDDLDto>();
    }

    public async Task<GridEntity<CrmCommissionTypeDto>> CommissionTypeSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string query = @"SELECT CommissionTypeId, CommissionTypeName, CalculationMode, IsActive, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmCommissionType";
        const string orderBy = "CommissionTypeName ASC";
        return await _repository.CrmCommissionTypes.AdoGridDataAsync<CrmCommissionTypeDto>(query, options, orderBy, string.Empty, cancellationToken);
    }
}
