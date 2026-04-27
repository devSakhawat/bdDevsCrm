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
/// CrmStudentStatus service implementing business logic for student status management.
/// </summary>
internal sealed class CrmStudentStatusService : ICrmStudentStatusService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmStudentStatusService> _logger;
    private readonly IConfiguration _config;

    public CrmStudentStatusService(IRepositoryManager repository, ILogger<CrmStudentStatusService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    /// <summary>Creates a new student status record.</summary>
    public async Task<CrmStudentStatusDto> CreateAsync(CreateCrmStudentStatusRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateCrmStudentStatusRecord));

        var safeName = record.StatusName?.Replace("\r", "\\r").Replace("\n", "\\n") ?? string.Empty;
        _logger.LogInformation("Creating new student status. StatusName: {StatusName}, Time: {Time}", safeName, DateTime.UtcNow);

        bool exists = await _repository.CrmStudentStatuses.ExistsAsync(
            x => x.StatusName.Trim().ToLower() == record.StatusName.Trim().ToLower(),
            cancellationToken: cancellationToken);

        if (exists)
            throw new DuplicateRecordException("StudentStatus", "StatusName");

        CrmStudentStatus entity = record.MapTo<CrmStudentStatus>();
        int newId = await _repository.CrmStudentStatuses.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Student status created successfully. ID: {StudentStatusId}, Time: {Time}", newId, DateTime.UtcNow);

        return entity.MapTo<CrmStudentStatusDto>() with { StudentStatusId = newId };
    }

    /// <summary>Updates an existing student status record.</summary>
    public async Task<CrmStudentStatusDto> UpdateAsync(UpdateCrmStudentStatusRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateCrmStudentStatusRecord));

        _logger.LogInformation("Updating student status. ID: {StudentStatusId}, Time: {Time}", record.StudentStatusId, DateTime.UtcNow);

        _ = await _repository.CrmStudentStatuses
            .FirstOrDefaultAsync(x => x.StudentStatusId == record.StudentStatusId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("StudentStatus", "StudentStatusId", record.StudentStatusId.ToString());

        bool duplicateExists = await _repository.CrmStudentStatuses.ExistsAsync(
            x => x.StatusName.Trim().ToLower() == record.StatusName.Trim().ToLower()
                 && x.StudentStatusId != record.StudentStatusId,
            cancellationToken: cancellationToken);

        if (duplicateExists)
            throw new DuplicateRecordException("StudentStatus", "StatusName");

        CrmStudentStatus entity = record.MapTo<CrmStudentStatus>();
        _repository.CrmStudentStatuses.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Student status updated successfully. ID: {StudentStatusId}, Time: {Time}", record.StudentStatusId, DateTime.UtcNow);

        return entity.MapTo<CrmStudentStatusDto>();
    }

    /// <summary>Deletes a student status record.</summary>
    public async Task DeleteAsync(DeleteCrmStudentStatusRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.StudentStatusId <= 0)
            throw new BadRequestException("Invalid delete request!");

        _logger.LogInformation("Deleting student status. ID: {StudentStatusId}, Time: {Time}", record.StudentStatusId, DateTime.UtcNow);

        _ = await _repository.CrmStudentStatuses
            .FirstOrDefaultAsync(x => x.StudentStatusId == record.StudentStatusId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("StudentStatus", "StudentStatusId", record.StudentStatusId.ToString());

        await _repository.CrmStudentStatuses.DeleteAsync(x => x.StudentStatusId == record.StudentStatusId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogWarning("Student status deleted successfully. ID: {StudentStatusId}, Time: {Time}", record.StudentStatusId, DateTime.UtcNow);
    }

    /// <summary>Retrieves a single student status record by ID.</summary>
    public async Task<CrmStudentStatusDto> StudentStatusAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching student status. ID: {StudentStatusId}, Time: {Time}", id, DateTime.UtcNow);

        var entity = await _repository.CrmStudentStatuses
            .FirstOrDefaultAsync(x => x.StudentStatusId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("StudentStatus", "StudentStatusId", id.ToString());

        return entity.MapTo<CrmStudentStatusDto>();
    }

    /// <summary>Retrieves all student status records.</summary>
    public async Task<IEnumerable<CrmStudentStatusDto>> StudentStatusesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all student statuses. Time: {Time}", DateTime.UtcNow);

        var entities = await _repository.CrmStudentStatuses.CrmStudentStatusesAsync(trackChanges, cancellationToken);

        if (!entities.Any())
        {
            _logger.LogWarning("No student statuses found. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmStudentStatusDto>();
        }

        _logger.LogInformation("Student statuses fetched successfully. Count: {Count}, Time: {Time}", entities.Count(), DateTime.UtcNow);
        return entities.MapToList<CrmStudentStatusDto>();
    }

    /// <summary>Retrieves a lightweight list of student statuses for dropdown binding.</summary>
    public async Task<IEnumerable<CrmStudentStatusDto>> StudentStatusForDDLAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching student statuses for DDL. Time: {Time}", DateTime.UtcNow);

        var entities = await _repository.CrmStudentStatuses.CrmStudentStatusesAsync(false, cancellationToken);

        if (!entities.Any())
        {
            _logger.LogWarning("No student statuses found for DDL. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmStudentStatusDto>();
        }

        return entities.MapToList<CrmStudentStatusDto>();
    }

    /// <summary>Retrieves a paginated summary grid of student statuses.</summary>
    public async Task<GridEntity<CrmStudentStatusDto>> StudentStatusesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching student statuses summary grid. Time: {Time}", DateTime.UtcNow);

        const string sql = @"SELECT StudentStatusId, StatusName, StatusCode, ColorCode, IsActive, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmStudentStatus";
        const string orderBy = "StatusName ASC";

        return await _repository.CrmStudentStatuses.AdoGridDataAsync<CrmStudentStatusDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }
}
