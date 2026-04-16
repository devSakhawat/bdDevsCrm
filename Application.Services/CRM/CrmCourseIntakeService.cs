using Application.Services.Caching;
using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Extensions;
using bdDevs.Shared.Records.CRM;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services.CRM;
using Domain.Entities.Entities.CRM;
using Domain.Exceptions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.CRM;

internal sealed class CrmCourseIntakeService : ICrmCourseIntakeService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<CrmCourseIntakeService> _logger;
    private readonly IConfiguration _configuration;

    public CrmCourseIntakeService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<CrmCourseIntakeService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<CrmCourseIntakeDto> CreateAsync(CreateCrmCourseIntakeRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateCrmCourseIntakeRecord));

        var validator = new CreateCrmCourseIntakeRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new course intake. CourseId: {CourseId}, Time: {Time}",
            record.CourseId, DateTime.UtcNow);

        CrmCourseIntake courseIntake = record.MapTo<CrmCourseIntake>();
        
        int courseIntakeId = await _repository.CrmCourseIntakes.CreateAndIdAsync(courseIntake, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Course intake created successfully. ID: {CourseIntakeId}, Time: {Time}",
            courseIntakeId, DateTime.UtcNow);

        await _cache.RemoveAsync("CourseIntake:All");
        await _cache.RemoveAsync($"CourseIntake:Course:{record.CourseId}");

        var resultDto = courseIntake.MapTo<CrmCourseIntakeDto>();
        resultDto.CourseIntakeId = courseIntakeId;
        return resultDto;
    }

    public async Task<CrmCourseIntakeDto> UpdateAsync(UpdateCrmCourseIntakeRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateCrmCourseIntakeRecord));

        var validator = new UpdateCrmCourseIntakeRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Updating course intake. ID: {CourseIntakeId}, Time: {Time}", 
            record.CourseIntakeId, DateTime.UtcNow);

        var existing = await _repository.CrmCourseIntakes.CourseIntakeAsync(
            record.CourseIntakeId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("CrmCourseIntake", "CourseIntakeId", record.CourseIntakeId.ToString());

        CrmCourseIntake courseIntake = record.MapTo<CrmCourseIntake>();
        _repository.CrmCourseIntakes.UpdateByState(courseIntake);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Course intake updated successfully. ID: {CourseIntakeId}, Time: {Time}",
            record.CourseIntakeId, DateTime.UtcNow);

        await _cache.RemoveAsync("CourseIntake:All");
        await _cache.RemoveAsync($"CourseIntake:{record.CourseIntakeId}");
        await _cache.RemoveAsync($"CourseIntake:Course:{record.CourseId}");

        return courseIntake.MapTo<CrmCourseIntakeDto>();
    }

    public async Task DeleteAsync(DeleteCrmCourseIntakeRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.CourseIntakeId <= 0)
            throw new BadRequestException("Invalid delete request!");

        var validator = new DeleteCrmCourseIntakeRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Deleting course intake. ID: {CourseIntakeId}, Time: {Time}", 
            record.CourseIntakeId, DateTime.UtcNow);

        var entity = await _repository.CrmCourseIntakes.CourseIntakeAsync(
            record.CourseIntakeId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CrmCourseIntake", "CourseIntakeId", record.CourseIntakeId.ToString());

        await _repository.CrmCourseIntakes.DeleteAsync(ci => ci.CourseIntakeId == record.CourseIntakeId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Course intake deleted successfully. ID: {CourseIntakeId}, Time: {Time}",
            record.CourseIntakeId, DateTime.UtcNow);

        await _cache.RemoveAsync("CourseIntake:All");
        await _cache.RemoveAsync($"CourseIntake:{record.CourseIntakeId}");
    }

    public async Task<CrmCourseIntakeDto> CourseIntakeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching course intake. ID: {CourseIntakeId}, Time: {Time}", id, DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: $"CourseIntake:{id}",
            factory: async () =>
            {
                var courseIntake = await _repository.CrmCourseIntakes.CourseIntakeAsync(id, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("CrmCourseIntake", "CourseIntakeId", id.ToString());

                return courseIntake.MapTo<CrmCourseIntakeDto>();
            },
            profile: CacheProfile.Static
        ) ?? throw new NotFoundException("CrmCourseIntake", "CourseIntakeId", id.ToString());
    }

    public async Task<IEnumerable<CrmCourseIntakeDto>> CourseIntakesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all course intakes. Time: {Time}", DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: "CourseIntake:All",
            factory: async () =>
            {
                var courseIntakes = await _repository.CrmCourseIntakes.CourseIntakesAsync(trackChanges, cancellationToken);

                if (!courseIntakes.Any())
                {
                    _logger.LogWarning("No course intakes found. Time: {Time}", DateTime.UtcNow);
                    return Enumerable.Empty<CrmCourseIntakeDto>();
                }

                return courseIntakes.MapToList<CrmCourseIntakeDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<CrmCourseIntakeDto>();
    }

    public async Task<IEnumerable<CrmCourseIntakeDDLDto>> CourseIntakesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching course intakes for dropdown list");

        return await _cache.OrSetAsync(
            key: "CourseIntake:DDL",
            factory: async () =>
            {
                var courseIntakes = await _repository.CrmCourseIntakes.CourseIntakesAsync(trackChanges, cancellationToken);

                if (!courseIntakes.Any())
                {
                    _logger.LogWarning("No course intakes found for dropdown");
                    return Enumerable.Empty<CrmCourseIntakeDDLDto>();
                }

                return courseIntakes.MapToList<CrmCourseIntakeDDLDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<CrmCourseIntakeDDLDto>();
    }

    public async Task<IEnumerable<CrmCourseIntakeDto>> CourseIntakesByCourseIdAsync(int courseId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching course intakes for course: {CourseId}", courseId);

        return await _cache.OrSetAsync(
            key: $"CourseIntake:Course:{courseId}",
            factory: async () =>
            {
                var intakes = await _repository.CrmCourseIntakes.CourseIntakesByCourseIdAsync(courseId, trackChanges, cancellationToken);
                return intakes.MapToList<CrmCourseIntakeDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<CrmCourseIntakeDto>();
    }

    public async Task<GridEntity<CrmCourseIntakeDto>> CourseIntakesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT * FROM CrmCourseIntake";
        const string orderBy = "IntakeTitile ASC";

        _logger.LogInformation("Fetching course intakes summary grid. Time: {Time}", DateTime.UtcNow);

        return await _repository.CrmCourseIntakes.AdoGridDataAsync<CrmCourseIntakeDto>(sql, options, orderBy, "", cancellationToken);
    }
}
