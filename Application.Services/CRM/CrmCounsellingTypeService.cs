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

internal sealed class CrmCounsellingTypeService : ICrmCounsellingTypeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmCounsellingTypeService> _logger;
    public CrmCounsellingTypeService(IRepositoryManager repository, ILogger<CrmCounsellingTypeService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<CrmCounsellingTypeDto> CreateAsync(CreateCrmCounsellingTypeRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateCrmCounsellingTypeRecord));

        bool exists = await _repository.CrmCounsellingTypes.ExistsAsync(x => x.CounsellingTypeName.Trim().ToLower() == record.CounsellingTypeName.Trim().ToLower(), cancellationToken: cancellationToken);
        if (exists)
            throw new ConflictException("Counselling Type with this value already exists!");

        CrmCounsellingType entity = record.MapTo<CrmCounsellingType>();
        int newId = await _repository.CrmCounsellingTypes.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Counselling Type created successfully. ID: {CounsellingTypeId}", newId);

        return entity.MapTo<CrmCounsellingTypeDto>() with { CounsellingTypeId = newId };
    }

    public async Task<CrmCounsellingTypeDto> UpdateAsync(UpdateCrmCounsellingTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateCrmCounsellingTypeRecord));

        _ = await _repository.CrmCounsellingTypes.FirstOrDefaultAsync(x => x.CounsellingTypeId == record.CounsellingTypeId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("Counselling Type", "CounsellingTypeId", record.CounsellingTypeId.ToString());

        bool duplicateExists = await _repository.CrmCounsellingTypes.ExistsAsync(x => x.CounsellingTypeName.Trim().ToLower() == record.CounsellingTypeName.Trim().ToLower() && x.CounsellingTypeId != record.CounsellingTypeId, cancellationToken: cancellationToken);
        if (duplicateExists)
            throw new ConflictException("Counselling Type with this value already exists!");

        CrmCounsellingType entity = record.MapTo<CrmCounsellingType>();
        _repository.CrmCounsellingTypes.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Counselling Type updated successfully. ID: {CounsellingTypeId}", record.CounsellingTypeId);

        return entity.MapTo<CrmCounsellingTypeDto>();
    }

    public async Task DeleteAsync(DeleteCrmCounsellingTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.CounsellingTypeId <= 0)
            throw new BadRequestException("Invalid delete request!");

        var entity = await _repository.CrmCounsellingTypes.FirstOrDefaultAsync(x => x.CounsellingTypeId == record.CounsellingTypeId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("Counselling Type", "CounsellingTypeId", record.CounsellingTypeId.ToString());

        await _repository.CrmCounsellingTypes.DeleteAsync(x => x.CounsellingTypeId == record.CounsellingTypeId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogWarning("Counselling Type deleted successfully. ID: {CounsellingTypeId}", record.CounsellingTypeId);
    }

    public async Task<CrmCounsellingTypeDto> CounsellingTypeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.CrmCounsellingTypes.FirstOrDefaultAsync(x => x.CounsellingTypeId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Counselling Type", "CounsellingTypeId", id.ToString());

        return entity.MapTo<CrmCounsellingTypeDto>();
    }

    public async Task<IEnumerable<CrmCounsellingTypeDto>> CounsellingTypesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmCounsellingTypes.CounsellingTypesAsync(trackChanges, cancellationToken);
        if (!entities.Any())
        {
            _logger.LogWarning("No Counselling Type records found.");
            return Enumerable.Empty<CrmCounsellingTypeDto>();
        }

        return entities.MapToList<CrmCounsellingTypeDto>();
    }

    public async Task<IEnumerable<CrmCounsellingTypeDDLDto>> CounsellingTypeForDDLAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmCounsellingTypes.ListByConditionAsync(x => true, x => x.CounsellingTypeName, trackChanges: false, descending: false, cancellationToken: cancellationToken);
        if (!entities.Any())
        {
            _logger.LogWarning("No Counselling Type records found for dropdown.");
            return Enumerable.Empty<CrmCounsellingTypeDDLDto>();
        }

        return entities.MapToList<CrmCounsellingTypeDDLDto>();
    }

    public async Task<GridEntity<CrmCounsellingTypeDto>> CounsellingTypeSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string query = @"SELECT CounsellingTypeId, CounsellingTypeName, IsActive, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmCounsellingType";
        const string orderBy = "CounsellingTypeName ASC";
        return await _repository.CrmCounsellingTypes.AdoGridDataAsync<CrmCounsellingTypeDto>(query, options, orderBy, string.Empty, cancellationToken);
    }
}
