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

/// <summary>CrmBranchTransfer service implementing business logic for branch transfer management.</summary>
internal sealed class CrmBranchTransferService : ICrmBranchTransferService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmBranchTransferService> _logger;
    private readonly IConfiguration _config;

    public CrmBranchTransferService(IRepositoryManager repository, ILogger<CrmBranchTransferService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    /// <summary>Creates a new branch transfer request.</summary>
    public async Task<CrmBranchTransferDto> CreateAsync(CreateCrmBranchTransferRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateCrmBranchTransferRecord));

        if (record.FromBranchId == record.ToBranchId)
            throw new BadRequestException("From Branch and To Branch cannot be the same.");

        _logger.LogInformation("Creating branch transfer. EntityType: {EntityType}, EntityId: {EntityId}, From: {From} -> To: {To}, Time: {Time}",
            record.EntityType, record.EntityId, record.FromBranchId, record.ToBranchId, DateTime.UtcNow);

        CrmBranchTransfer entity = record.MapTo<CrmBranchTransfer>();
        int newId = await _repository.CrmBranchTransfers.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Branch transfer created. ID: {TransferId}, Time: {Time}", newId, DateTime.UtcNow);

        return entity.MapTo<CrmBranchTransferDto>() with { TransferId = newId };
    }

    /// <summary>Updates an existing branch transfer record.</summary>
    public async Task<CrmBranchTransferDto> UpdateAsync(UpdateCrmBranchTransferRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateCrmBranchTransferRecord));

        _ = await _repository.CrmBranchTransfers
            .FirstOrDefaultAsync(x => x.TransferId == record.TransferId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("BranchTransfer", "TransferId", record.TransferId.ToString());

        _logger.LogInformation("Updating branch transfer. ID: {TransferId}, Time: {Time}", record.TransferId, DateTime.UtcNow);

        CrmBranchTransfer entity = record.MapTo<CrmBranchTransfer>();
        _repository.CrmBranchTransfers.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Branch transfer updated. ID: {TransferId}, Time: {Time}", record.TransferId, DateTime.UtcNow);

        return entity.MapTo<CrmBranchTransferDto>();
    }

    /// <summary>Deletes a branch transfer record.</summary>
    public async Task DeleteAsync(DeleteCrmBranchTransferRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.TransferId <= 0)
            throw new BadRequestException("Invalid delete request!");

        _ = await _repository.CrmBranchTransfers
            .FirstOrDefaultAsync(x => x.TransferId == record.TransferId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("BranchTransfer", "TransferId", record.TransferId.ToString());

        await _repository.CrmBranchTransfers.DeleteAsync(x => x.TransferId == record.TransferId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogWarning("Branch transfer deleted. ID: {TransferId}, Time: {Time}", record.TransferId, DateTime.UtcNow);
    }

    /// <summary>Retrieves a single branch transfer record by ID.</summary>
    public async Task<CrmBranchTransferDto> BranchTransferAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.CrmBranchTransfers
            .FirstOrDefaultAsync(x => x.TransferId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("BranchTransfer", "TransferId", id.ToString());

        return entity.MapTo<CrmBranchTransferDto>();
    }

    /// <summary>Retrieves all branch transfer records.</summary>
    public async Task<IEnumerable<CrmBranchTransferDto>> BranchTransfersAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmBranchTransfers.CrmBranchTransfersAsync(trackChanges, cancellationToken);

        if (!entities.Any())
            return Enumerable.Empty<CrmBranchTransferDto>();

        return entities.MapToList<CrmBranchTransferDto>();
    }

    /// <summary>Retrieves a paginated summary grid of branch transfers.</summary>
    public async Task<GridEntity<CrmBranchTransferDto>> BranchTransfersSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT TransferId, EntityType, EntityId, FromBranchId, ToBranchId, TransferReason, TransferStatus, RequestedBy, ApprovedBy, RequestedDate, ApprovedDate, Notes, IsDeleted, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmBranchTransfer";
        const string orderBy = "RequestedDate DESC";

        return await _repository.CrmBranchTransfers.AdoGridDataAsync<CrmBranchTransferDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }

    /// <summary>Retrieves branch transfers by entity type and entity ID.</summary>
    public async Task<IEnumerable<CrmBranchTransferDto>> BranchTransfersByEntityAsync(byte entityType, int entityId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmBranchTransfers.CrmBranchTransfersByEntityAsync(entityType, entityId, trackChanges, cancellationToken);

        if (!entities.Any())
            return Enumerable.Empty<CrmBranchTransferDto>();

        return entities.MapToList<CrmBranchTransferDto>();
    }

    /// <summary>Approves a pending branch transfer.</summary>
    public async Task<CrmBranchTransferDto> ApproveAsync(ApproveCrmBranchTransferRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(ApproveCrmBranchTransferRecord));

        var existing = await _repository.CrmBranchTransfers
            .FirstOrDefaultAsync(x => x.TransferId == record.TransferId, trackChanges: true, cancellationToken)
            ?? throw new NotFoundException("BranchTransfer", "TransferId", record.TransferId.ToString());

        if (existing.TransferStatus != 1)
            throw new BadRequestException("Only pending transfers can be approved.");

        existing.TransferStatus = 2;
        existing.ApprovedBy = record.ApprovedBy;
        existing.ApprovedDate = record.ApprovedDate;
        existing.Notes = record.Notes ?? existing.Notes;
        existing.UpdatedDate = DateTime.UtcNow;

        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Branch transfer approved. ID: {TransferId}, ApprovedBy: {ApprovedBy}, Time: {Time}",
            record.TransferId, record.ApprovedBy, DateTime.UtcNow);

        return existing.MapTo<CrmBranchTransferDto>();
    }
}
