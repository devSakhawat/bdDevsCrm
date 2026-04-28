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

internal sealed class CrmStudentStatusHistoryService : ICrmStudentStatusHistoryService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmStudentStatusHistoryService> _logger;
    private readonly IConfiguration _config;

    public CrmStudentStatusHistoryService(IRepositoryManager repository, ILogger<CrmStudentStatusHistoryService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    public async Task<CrmStudentStatusHistoryDto> CreateAsync(CreateCrmStudentStatusHistoryRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmStudentStatusHistoryRecord));

        var entity = record.MapTo<CrmStudentStatusHistory>();
        int newId = await _repository.CrmStudentStatusHistories.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        _logger.LogInformation("CrmStudentStatusHistory created. ID: {Id}", newId);
        return entity.MapTo<CrmStudentStatusHistoryDto>() with { StudentStatusHistoryId = newId };
    }

    public async Task<CrmStudentStatusHistoryDto> UpdateAsync(UpdateCrmStudentStatusHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmStudentStatusHistoryRecord));
        _ = await _repository.CrmStudentStatusHistories.CrmStudentStatusHistoryAsync(record.StudentStatusHistoryId, false, cancellationToken)
            ?? throw new NotFoundException("CrmStudentStatusHistory", "StudentStatusHistoryId", record.StudentStatusHistoryId.ToString());

        var entity = record.MapTo<CrmStudentStatusHistory>();
        _repository.CrmStudentStatusHistories.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmStudentStatusHistoryDto>();
    }

    public async Task DeleteAsync(DeleteCrmStudentStatusHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.StudentStatusHistoryId <= 0) throw new BadRequestException("Invalid delete request!");
        _ = await _repository.CrmStudentStatusHistories.CrmStudentStatusHistoryAsync(record.StudentStatusHistoryId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CrmStudentStatusHistory", "StudentStatusHistoryId", record.StudentStatusHistoryId.ToString());
        await _repository.CrmStudentStatusHistories.DeleteAsync(x => x.StudentStatusHistoryId == record.StudentStatusHistoryId, false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmStudentStatusHistoryDto> StudentStatusHistoryAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.CrmStudentStatusHistories.CrmStudentStatusHistoryAsync(id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CrmStudentStatusHistory", "StudentStatusHistoryId", id.ToString());
        return entity.MapTo<CrmStudentStatusHistoryDto>();
    }

    public async Task<IEnumerable<CrmStudentStatusHistoryDto>> StudentStatusHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmStudentStatusHistories.StudentStatusHistoriesAsync(trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmStudentStatusHistoryDto>() : Enumerable.Empty<CrmStudentStatusHistoryDto>();
    }

    public async Task<IEnumerable<CrmStudentStatusHistoryDto>> StudentStatusHistoriesByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmStudentStatusHistories.StudentStatusHistoriesByStudentIdAsync(studentId, trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmStudentStatusHistoryDto>() : Enumerable.Empty<CrmStudentStatusHistoryDto>();
    }

    public async Task<GridEntity<CrmStudentStatusHistoryDto>> StudentStatusHistoriesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT StudentStatusHistoryId, StudentId, OldStatus, NewStatus, ChangedBy, ChangedDate, Notes FROM CrmStudentStatusHistory";
        const string orderBy = "ChangedDate DESC";
        return await _repository.CrmStudentStatusHistories.AdoGridDataAsync<CrmStudentStatusHistoryDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }

}
