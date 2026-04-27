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

internal sealed class CrmMasterDataSuggestionService : ICrmMasterDataSuggestionService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmMasterDataSuggestionService> _logger;
    private readonly IConfiguration _config;

    public CrmMasterDataSuggestionService(IRepositoryManager repository, ILogger<CrmMasterDataSuggestionService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    public async Task<CrmMasterDataSuggestionDto> CreateAsync(CreateCrmMasterDataSuggestionRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmMasterDataSuggestionRecord));

        CrmMasterDataSuggestion entity = record.MapTo<CrmMasterDataSuggestion>();
        int newId = await _repository.CrmMasterDataSuggestions.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("MasterDataSuggestion created. ID: {Id}", newId);
        return entity.MapTo<CrmMasterDataSuggestionDto>() with { SuggestionId = newId };
    }

    public async Task<CrmMasterDataSuggestionDto> UpdateAsync(UpdateCrmMasterDataSuggestionRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmMasterDataSuggestionRecord));

        _ = await _repository.CrmMasterDataSuggestions
            .FirstOrDefaultAsync(x => x.SuggestionId == record.SuggestionId, false, cancellationToken)
            ?? throw new NotFoundException("MasterDataSuggestion", "SuggestionId", record.SuggestionId.ToString());

        CrmMasterDataSuggestion entity = record.MapTo<CrmMasterDataSuggestion>();
        _repository.CrmMasterDataSuggestions.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmMasterDataSuggestionDto>();
    }

    public async Task DeleteAsync(DeleteCrmMasterDataSuggestionRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.SuggestionId <= 0) throw new BadRequestException("Invalid delete request!");

        _ = await _repository.CrmMasterDataSuggestions
            .FirstOrDefaultAsync(x => x.SuggestionId == record.SuggestionId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("MasterDataSuggestion", "SuggestionId", record.SuggestionId.ToString());

        await _repository.CrmMasterDataSuggestions.DeleteAsync(x => x.SuggestionId == record.SuggestionId, false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmMasterDataSuggestionDto> MasterDataSuggestionAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.CrmMasterDataSuggestions
            .FirstOrDefaultAsync(x => x.SuggestionId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("MasterDataSuggestion", "SuggestionId", id.ToString());
        return entity.MapTo<CrmMasterDataSuggestionDto>();
    }

    public async Task<IEnumerable<CrmMasterDataSuggestionDto>> MasterDataSuggestionsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmMasterDataSuggestions.CrmMasterDataSuggestionsAsync(trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmMasterDataSuggestionDto>() : Enumerable.Empty<CrmMasterDataSuggestionDto>();
    }

    public async Task<GridEntity<CrmMasterDataSuggestionDto>> MasterDataSuggestionsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT SuggestionId, EntityType, SuggestedValue, Status, CreatedDate, CreatedBy FROM CrmMasterDataSuggestion";
        const string orderBy = "SuggestionId ASC";
        return await _repository.CrmMasterDataSuggestions.AdoGridDataAsync<CrmMasterDataSuggestionDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }
}
