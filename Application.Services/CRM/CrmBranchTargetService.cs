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

internal sealed class CrmBranchTargetService : ICrmBranchTargetService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmBranchTargetService> _logger;
    private readonly IConfiguration _config;

    public CrmBranchTargetService(IRepositoryManager repository, ILogger<CrmBranchTargetService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    public async Task<CrmBranchTargetDto> CreateAsync(CreateCrmBranchTargetRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmBranchTargetRecord));

        bool exists = await _repository.CrmBranchTargets.ExistsAsync(
            x => x.BranchId == record.BranchId && x.Year == record.Year && x.Month == record.Month, cancellationToken: cancellationToken);
        if (exists) throw new DuplicateRecordException("BranchTarget", "BranchId+Year+Month");

        CrmBranchTarget entity = record.MapTo<CrmBranchTarget>();
        int newId = await _repository.CrmBranchTargets.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("BranchTarget created. ID: {Id}", newId);
        return entity.MapTo<CrmBranchTargetDto>() with { BranchTargetId = newId };
    }

    public async Task<CrmBranchTargetDto> UpdateAsync(UpdateCrmBranchTargetRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmBranchTargetRecord));

        _ = await _repository.CrmBranchTargets
            .FirstOrDefaultAsync(x => x.BranchTargetId == record.BranchTargetId, false, cancellationToken)
            ?? throw new NotFoundException("BranchTarget", "BranchTargetId", record.BranchTargetId.ToString());

        CrmBranchTarget entity = record.MapTo<CrmBranchTarget>();
        _repository.CrmBranchTargets.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmBranchTargetDto>();
    }

    public async Task DeleteAsync(DeleteCrmBranchTargetRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.BranchTargetId <= 0) throw new BadRequestException("Invalid delete request!");

        _ = await _repository.CrmBranchTargets
            .FirstOrDefaultAsync(x => x.BranchTargetId == record.BranchTargetId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("BranchTarget", "BranchTargetId", record.BranchTargetId.ToString());

        await _repository.CrmBranchTargets.DeleteAsync(x => x.BranchTargetId == record.BranchTargetId, false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmBranchTargetDto> BranchTargetAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.CrmBranchTargets
            .FirstOrDefaultAsync(x => x.BranchTargetId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("BranchTarget", "BranchTargetId", id.ToString());
        return entity.MapTo<CrmBranchTargetDto>();
    }

    public async Task<IEnumerable<CrmBranchTargetDto>> BranchTargetsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmBranchTargets.CrmBranchTargetsAsync(trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmBranchTargetDto>() : Enumerable.Empty<CrmBranchTargetDto>();
    }

    public async Task<IEnumerable<CrmBranchTargetDto>> BranchTargetsByBranchIdAsync(int branchId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmBranchTargets.CrmBranchTargetsByBranchIdAsync(branchId, trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmBranchTargetDto>() : Enumerable.Empty<CrmBranchTargetDto>();
    }

    public async Task<GridEntity<CrmBranchTargetDto>> BranchTargetsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT BranchTargetId, BranchId, Year, Month, LeadTarget, ConversionTarget, ApplicationTarget, EnrolmentTarget, RevenueTarget FROM CrmBranchTarget";
        const string orderBy = "BranchTargetId ASC";
        return await _repository.CrmBranchTargets.AdoGridDataAsync<CrmBranchTargetDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }
}
