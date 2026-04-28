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

internal sealed class CrmCountryDocumentRequirementService : ICrmCountryDocumentRequirementService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmCountryDocumentRequirementService> _logger;
    private readonly IConfiguration _config;

    public CrmCountryDocumentRequirementService(IRepositoryManager repository, ILogger<CrmCountryDocumentRequirementService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    public async Task<CrmCountryDocumentRequirementDto> CreateAsync(CreateCrmCountryDocumentRequirementRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmCountryDocumentRequirementRecord));

        CrmCountryDocumentRequirement entity = record.MapTo<CrmCountryDocumentRequirement>();
        int newId = await _repository.CrmCountryDocumentRequirements.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("CountryDocReq created. ID: {Id}", newId);
        return entity.MapTo<CrmCountryDocumentRequirementDto>() with { RequirementId = newId };
    }

    public async Task<CrmCountryDocumentRequirementDto> UpdateAsync(UpdateCrmCountryDocumentRequirementRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmCountryDocumentRequirementRecord));

        _ = await _repository.CrmCountryDocumentRequirements
            .FirstOrDefaultAsync(x => x.RequirementId == record.RequirementId, false, cancellationToken)
            ?? throw new NotFoundException("CountryDocumentRequirement", "RequirementId", record.RequirementId.ToString());

        CrmCountryDocumentRequirement entity = record.MapTo<CrmCountryDocumentRequirement>();
        _repository.CrmCountryDocumentRequirements.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmCountryDocumentRequirementDto>();
    }

    public async Task DeleteAsync(DeleteCrmCountryDocumentRequirementRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.RequirementId <= 0) throw new BadRequestException("Invalid delete request!");

        _ = await _repository.CrmCountryDocumentRequirements
            .FirstOrDefaultAsync(x => x.RequirementId == record.RequirementId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CountryDocumentRequirement", "RequirementId", record.RequirementId.ToString());

        await _repository.CrmCountryDocumentRequirements.DeleteAsync(x => x.RequirementId == record.RequirementId, false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmCountryDocumentRequirementDto> CountryDocumentRequirementAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.CrmCountryDocumentRequirements
            .FirstOrDefaultAsync(x => x.RequirementId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CountryDocumentRequirement", "RequirementId", id.ToString());
        return entity.MapTo<CrmCountryDocumentRequirementDto>();
    }

    public async Task<IEnumerable<CrmCountryDocumentRequirementDto>> CountryDocumentRequirementsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmCountryDocumentRequirements.CrmCountryDocumentRequirementsAsync(trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmCountryDocumentRequirementDto>() : Enumerable.Empty<CrmCountryDocumentRequirementDto>();
    }

    public async Task<IEnumerable<CrmCountryDocumentRequirementDto>> RequirementsByCountryIdAsync(int countryId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmCountryDocumentRequirements.RequirementsByCountryIdAsync(countryId, trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmCountryDocumentRequirementDto>() : Enumerable.Empty<CrmCountryDocumentRequirementDto>();
    }

    public async Task<GridEntity<CrmCountryDocumentRequirementDto>> CountryDocumentRequirementsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT RequirementId, CountryId, DegreeLevelId, DocumentTypeName, IsMandatory, CreatedDate FROM CrmCountryDocumentRequirement";
        const string orderBy = "RequirementId ASC";
        return await _repository.CrmCountryDocumentRequirements.AdoGridDataAsync<CrmCountryDocumentRequirementDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }
}
