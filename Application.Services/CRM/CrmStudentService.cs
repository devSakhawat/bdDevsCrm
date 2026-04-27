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

/// <summary>CrmStudent service implementing business logic for student management.</summary>
internal sealed class CrmStudentService : ICrmStudentService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmStudentService> _logger;
    private readonly IConfiguration _config;

    public CrmStudentService(IRepositoryManager repository, ILogger<CrmStudentService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    /// <summary>Creates a new student record.</summary>
    public async Task<CrmStudentDto> CreateAsync(CreateCrmStudentRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateCrmStudentRecord));

        _logger.LogInformation("Creating new Student. Time: {Time}", DateTime.UtcNow);

        CrmStudent entity = record.MapTo<CrmStudent>();
        int newId = await _repository.CrmStudents.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Student created successfully. ID: {Id}, Time: {Time}", newId, DateTime.UtcNow);
        return entity.MapTo<CrmStudentDto>() with { StudentId = newId };
    }

    /// <summary>Updates an existing student record.</summary>
    public async Task<CrmStudentDto> UpdateAsync(UpdateCrmStudentRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateCrmStudentRecord));

        _ = await _repository.CrmStudents
            .FirstOrDefaultAsync(x => x.StudentId == record.StudentId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("Student", "StudentId", record.StudentId.ToString());

        _logger.LogInformation("Updating Student. ID: {Id}, Time: {Time}", record.StudentId, DateTime.UtcNow);

        CrmStudent entity = record.MapTo<CrmStudent>();
        _repository.CrmStudents.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Student updated successfully. ID: {Id}, Time: {Time}", record.StudentId, DateTime.UtcNow);
        return entity.MapTo<CrmStudentDto>();
    }

    /// <summary>Deletes a student record.</summary>
    public async Task DeleteAsync(DeleteCrmStudentRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.StudentId <= 0)
            throw new BadRequestException("Invalid delete request!");

        _ = await _repository.CrmStudents
            .FirstOrDefaultAsync(x => x.StudentId == record.StudentId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Student", "StudentId", record.StudentId.ToString());

        _logger.LogInformation("Deleting Student. ID: {Id}, Time: {Time}", record.StudentId, DateTime.UtcNow);
        await _repository.CrmStudents.DeleteAsync(x => x.StudentId == record.StudentId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        _logger.LogWarning("Student deleted successfully. ID: {Id}, Time: {Time}", record.StudentId, DateTime.UtcNow);
    }

    /// <summary>Retrieves a single student record by ID.</summary>
    public async Task<CrmStudentDto> StudentAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Student. ID: {Id}, Time: {Time}", id, DateTime.UtcNow);
        var entity = await _repository.CrmStudents
            .FirstOrDefaultAsync(x => x.StudentId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Student", "StudentId", id.ToString());
        return entity.MapTo<CrmStudentDto>();
    }

    /// <summary>Retrieves all student records.</summary>
    public async Task<IEnumerable<CrmStudentDto>> StudentsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all Students. Time: {Time}", DateTime.UtcNow);
        var entities = await _repository.CrmStudents.CrmStudentsAsync(trackChanges, cancellationToken);
        if (!entities.Any())
        {
            _logger.LogWarning("No Students found. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmStudentDto>();
        }
        _logger.LogInformation("Students fetched. Count: {Count}, Time: {Time}", entities.Count(), DateTime.UtcNow);
        return entities.MapToList<CrmStudentDto>();
    }

    /// <summary>Retrieves a lightweight list of students for dropdown binding.</summary>
    public async Task<IEnumerable<CrmStudentDto>> StudentForDDLAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Students for DDL. Time: {Time}", DateTime.UtcNow);
        var entities = await _repository.CrmStudents.CrmStudentsAsync(false, cancellationToken);
        if (!entities.Any())
        {
            _logger.LogWarning("No Students found for DDL. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmStudentDto>();
        }
        return entities.MapToList<CrmStudentDto>();
    }

    /// <summary>Retrieves a paginated summary grid of students.</summary>
    public async Task<GridEntity<CrmStudentDto>> StudentsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Students summary grid. Time: {Time}", DateTime.UtcNow);
        const string sql = @"SELECT StudentId, StudentName, StudentCode, Email, Phone, LeadId, StudentStatusId, AgentId, CounselorId, DateOfBirth, PassportNumber, VisaTypeId, Nationality, IsActive, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmStudent";
        const string orderBy = "StudentName ASC";
        return await _repository.CrmStudents.AdoGridDataAsync<CrmStudentDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }
}
