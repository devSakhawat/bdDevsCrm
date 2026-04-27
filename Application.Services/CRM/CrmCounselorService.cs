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

/// <summary>CrmCounselor service implementing business logic for counselor management.</summary>
internal sealed class CrmCounselorService : ICrmCounselorService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmCounselorService> _logger;
    private readonly IConfiguration _config;

    public CrmCounselorService(IRepositoryManager repository, ILogger<CrmCounselorService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    /// <summary>Creates a new counselor record.</summary>
    public async Task<CrmCounselorDto> CreateAsync(CreateCrmCounselorRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateCrmCounselorRecord));

        bool exists = await _repository.CrmCounselors.ExistsAsync(
            x => x.CounselorName.Trim().ToLower() == record.CounselorName.Trim().ToLower(),
            cancellationToken: cancellationToken);

        if (exists)
            throw new DuplicateRecordException("Counselor", "CounselorName");

        _logger.LogInformation("Creating new Counselor. Time: {Time}", DateTime.UtcNow);

        CrmCounselor entity = record.MapTo<CrmCounselor>();
        int newId = await _repository.CrmCounselors.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Counselor created successfully. ID: {Id}, Time: {Time}", newId, DateTime.UtcNow);
        return entity.MapTo<CrmCounselorDto>() with { CounselorId = newId };
    }

    /// <summary>Updates an existing counselor record.</summary>
    public async Task<CrmCounselorDto> UpdateAsync(UpdateCrmCounselorRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateCrmCounselorRecord));

        _ = await _repository.CrmCounselors
            .FirstOrDefaultAsync(x => x.CounselorId == record.CounselorId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("Counselor", "CounselorId", record.CounselorId.ToString());

        bool duplicateExists = await _repository.CrmCounselors.ExistsAsync(
            x => x.CounselorName.Trim().ToLower() == record.CounselorName.Trim().ToLower()
                 && x.CounselorId != record.CounselorId,
            cancellationToken: cancellationToken);

        if (duplicateExists)
            throw new DuplicateRecordException("Counselor", "CounselorName");

        _logger.LogInformation("Updating Counselor. ID: {Id}, Time: {Time}", record.CounselorId, DateTime.UtcNow);

        CrmCounselor entity = record.MapTo<CrmCounselor>();
        _repository.CrmCounselors.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Counselor updated successfully. ID: {Id}, Time: {Time}", record.CounselorId, DateTime.UtcNow);
        return entity.MapTo<CrmCounselorDto>();
    }

    /// <summary>Deletes a counselor record.</summary>
    public async Task DeleteAsync(DeleteCrmCounselorRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.CounselorId <= 0)
            throw new BadRequestException("Invalid delete request!");

        _ = await _repository.CrmCounselors
            .FirstOrDefaultAsync(x => x.CounselorId == record.CounselorId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Counselor", "CounselorId", record.CounselorId.ToString());

        _logger.LogInformation("Deleting Counselor. ID: {Id}, Time: {Time}", record.CounselorId, DateTime.UtcNow);
        await _repository.CrmCounselors.DeleteAsync(x => x.CounselorId == record.CounselorId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        _logger.LogWarning("Counselor deleted successfully. ID: {Id}, Time: {Time}", record.CounselorId, DateTime.UtcNow);
    }

    /// <summary>Retrieves a single counselor record by ID.</summary>
    public async Task<CrmCounselorDto> CounselorAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Counselor. ID: {Id}, Time: {Time}", id, DateTime.UtcNow);
        var entity = await _repository.CrmCounselors
            .FirstOrDefaultAsync(x => x.CounselorId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Counselor", "CounselorId", id.ToString());
        return entity.MapTo<CrmCounselorDto>();
    }

    /// <summary>Retrieves all counselor records.</summary>
    public async Task<IEnumerable<CrmCounselorDto>> CounselorsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all Counselors. Time: {Time}", DateTime.UtcNow);
        var entities = await _repository.CrmCounselors.CrmCounselorsAsync(trackChanges, cancellationToken);
        if (!entities.Any())
        {
            _logger.LogWarning("No Counselors found. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmCounselorDto>();
        }
        _logger.LogInformation("Counselors fetched. Count: {Count}, Time: {Time}", entities.Count(), DateTime.UtcNow);
        return entities.MapToList<CrmCounselorDto>();
    }

    /// <summary>Retrieves a lightweight list of counselors for dropdown binding.</summary>
    public async Task<IEnumerable<CrmCounselorDto>> CounselorForDDLAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Counselors for DDL. Time: {Time}", DateTime.UtcNow);
        var entities = await _repository.CrmCounselors.CrmCounselorsAsync(false, cancellationToken);
        if (!entities.Any())
        {
            _logger.LogWarning("No Counselors found for DDL. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmCounselorDto>();
        }
        return entities.MapToList<CrmCounselorDto>();
    }

    /// <summary>Retrieves a paginated summary grid of counselors.</summary>
    public async Task<GridEntity<CrmCounselorDto>> CounselorsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Counselors summary grid. Time: {Time}", DateTime.UtcNow);
        const string sql = @"SELECT CounselorId, CounselorName, CounselorCode, Email, Phone, OfficeId, UserId, IsActive, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmCounselor";
        const string orderBy = "CounselorName ASC";
        return await _repository.CrmCounselors.AdoGridDataAsync<CrmCounselorDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }
}
