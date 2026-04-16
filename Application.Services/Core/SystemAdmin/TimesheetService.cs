using Application.Services.Caching;
using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Extensions;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Domain.Exceptions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.Core.SystemAdmin;

internal sealed class TimesheetService : ITimesheetService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<TimesheetService> _logger;
    private readonly IConfiguration _configuration;

    public TimesheetService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<TimesheetService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<TimesheetDto> CreateAsync(CreateTimesheetRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateTimesheetRecord));

        var validator = new CreateTimesheetRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new timesheet. Date: {WorkingLogDate}, Time: {Time}",
            record.WorkingLogDate, DateTime.UtcNow);

        Timesheet timesheet = record.MapTo<Timesheet>();
        timesheet.LogEntryDate = DateTime.UtcNow;
        
        int timesheetId = await _repository.Timesheets.CreateAndIdAsync(timesheet, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Timesheet created successfully. ID: {TimesheetId}, Time: {Time}",
            timesheetId, DateTime.UtcNow);

        await _cache.RemoveAsync("Timesheet:All");

        var resultDto = timesheet.MapTo<TimesheetDto>();
        resultDto.Timesheetid = timesheetId;
        return resultDto;
    }

    public async Task<TimesheetDto> UpdateAsync(UpdateTimesheetRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateTimesheetRecord));

        var validator = new UpdateTimesheetRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Updating timesheet. ID: {TimesheetId}, Time: {Time}", 
            record.Timesheetid, DateTime.UtcNow);

        var existingTimesheet = await _repository.Timesheets.TimesheetAsync(
            record.Timesheetid, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("Timesheet", "Timesheetid", record.Timesheetid.ToString());

        Timesheet timesheet = record.MapTo<Timesheet>();
        _repository.Timesheets.UpdateByState(timesheet);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Timesheet updated successfully. ID: {TimesheetId}, Time: {Time}",
            record.Timesheetid, DateTime.UtcNow);

        await _cache.RemoveAsync("Timesheet:All");
        await _cache.RemoveAsync($"Timesheet:{record.Timesheetid}");

        return timesheet.MapTo<TimesheetDto>();
    }

    public async Task DeleteAsync(DeleteTimesheetRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.Timesheetid <= 0)
            throw new BadRequestException("Invalid delete request!");

        var validator = new DeleteTimesheetRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Deleting timesheet. ID: {TimesheetId}, Time: {Time}", 
            record.Timesheetid, DateTime.UtcNow);

        var timesheetEntity = await _repository.Timesheets.TimesheetAsync(
            record.Timesheetid, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Timesheet", "Timesheetid", record.Timesheetid.ToString());

        await _repository.Timesheets.DeleteAsync(t => t.Timesheetid == record.Timesheetid, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Timesheet deleted successfully. ID: {TimesheetId}, Time: {Time}",
            record.Timesheetid, DateTime.UtcNow);

        await _cache.RemoveAsync("Timesheet:All");
        await _cache.RemoveAsync($"Timesheet:{record.Timesheetid}");
    }

    public async Task<TimesheetDto> TimesheetAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching timesheet. ID: {TimesheetId}, Time: {Time}", id, DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: $"Timesheet:{id}",
            factory: async () =>
            {
                var timesheet = await _repository.Timesheets.TimesheetAsync(id, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("Timesheet", "Timesheetid", id.ToString());

                return timesheet.MapTo<TimesheetDto>();
            },
            profile: CacheProfile.Dynamic
        ) ?? throw new NotFoundException("Timesheet", "Timesheetid", id.ToString());
    }

    public async Task<IEnumerable<TimesheetDto>> TimesheetsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all timesheets. Time: {Time}", DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: "Timesheet:All",
            factory: async () =>
            {
                var timesheets = await _repository.Timesheets.TimesheetsAsync(trackChanges, cancellationToken);

                if (!timesheets.Any())
                {
                    _logger.LogWarning("No timesheets found. Time: {Time}", DateTime.UtcNow);
                    return Enumerable.Empty<TimesheetDto>();
                }

                return timesheets.MapToList<TimesheetDto>();
            },
            profile: CacheProfile.Dynamic
        ) ?? Enumerable.Empty<TimesheetDto>();
    }

    public async Task<IEnumerable<TimesheetDDLDto>> TimesheetsForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching timesheets for dropdown list");

        return await _cache.OrSetAsync(
            key: "Timesheet:DDL",
            factory: async () =>
            {
                var timesheets = await _repository.Timesheets.TimesheetsAsync(trackChanges, cancellationToken);

                if (!timesheets.Any())
                {
                    _logger.LogWarning("No timesheets found for dropdown");
                    return Enumerable.Empty<TimesheetDDLDto>();
                }

                return timesheets.MapToList<TimesheetDDLDto>();
            },
            profile: CacheProfile.Dynamic
        ) ?? Enumerable.Empty<TimesheetDDLDto>();
    }

    public async Task<IEnumerable<TimesheetDto>> TimesheetsByEmployeeAsync(int hrRecordId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching timesheets for employee: {HrRecordId}", hrRecordId);

        return await _cache.OrSetAsync(
            key: $"Timesheet:Employee:{hrRecordId}",
            factory: async () =>
            {
                var timesheets = await _repository.Timesheets.TimesheetsByEmployeeAsync(hrRecordId, trackChanges, cancellationToken);
                return timesheets.MapToList<TimesheetDto>();
            },
            profile: CacheProfile.Dynamic
        ) ?? Enumerable.Empty<TimesheetDto>();
    }

    public async Task<GridEntity<TimesheetDto>> TimesheetsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT * FROM Timesheet";
        const string orderBy = "WorkingLogDate DESC";

        _logger.LogInformation("Fetching timesheets summary grid. Time: {Time}", DateTime.UtcNow);

        return await _repository.Timesheets.AdoGridDataAsync<TimesheetDto>(sql, options, orderBy, "", cancellationToken);
    }
}
