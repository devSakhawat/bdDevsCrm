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

internal sealed class CrmCourseFeeService : ICrmCourseFeeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmCourseFeeService> _logger;
    private readonly IConfiguration _config;

    public CrmCourseFeeService(IRepositoryManager repository, ILogger<CrmCourseFeeService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    public async Task<CrmCourseFeeDto> CreateAsync(CreateCrmCourseFeeRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmCourseFeeRecord));

        bool exists = await _repository.CrmCourseFees.ExistsAsync(
            x => x.CourseId == record.CourseId && x.IntakeId == record.IntakeId && x.FeeType == record.FeeType, cancellationToken: cancellationToken);
        if (exists) throw new DuplicateRecordException("CourseFee", "CourseId+IntakeId+FeeType");

        CrmCourseFee entity = record.MapTo<CrmCourseFee>();
        int newId = await _repository.CrmCourseFees.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("CourseFee created. ID: {Id}, Time: {Time}", newId, DateTime.UtcNow);
        return entity.MapTo<CrmCourseFeeDto>() with { CourseFeeId = newId };
    }

    public async Task<CrmCourseFeeDto> UpdateAsync(UpdateCrmCourseFeeRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmCourseFeeRecord));

        _ = await _repository.CrmCourseFees
            .FirstOrDefaultAsync(x => x.CourseFeeId == record.CourseFeeId, false, cancellationToken)
            ?? throw new NotFoundException("CourseFee", "CourseFeeId", record.CourseFeeId.ToString());

        CrmCourseFee entity = record.MapTo<CrmCourseFee>();
        _repository.CrmCourseFees.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmCourseFeeDto>();
    }

    public async Task DeleteAsync(DeleteCrmCourseFeeRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.CourseFeeId <= 0) throw new BadRequestException("Invalid delete request!");

        _ = await _repository.CrmCourseFees
            .FirstOrDefaultAsync(x => x.CourseFeeId == record.CourseFeeId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CourseFee", "CourseFeeId", record.CourseFeeId.ToString());

        await _repository.CrmCourseFees.DeleteAsync(x => x.CourseFeeId == record.CourseFeeId, false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmCourseFeeDto> CourseFeeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.CrmCourseFees
            .FirstOrDefaultAsync(x => x.CourseFeeId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CourseFee", "CourseFeeId", id.ToString());
        return entity.MapTo<CrmCourseFeeDto>();
    }

    public async Task<IEnumerable<CrmCourseFeeDto>> CourseFeesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmCourseFees.CrmCourseFeesAsync(trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmCourseFeeDto>() : Enumerable.Empty<CrmCourseFeeDto>();
    }

    public async Task<IEnumerable<CrmCourseFeeDto>> CourseFeesByCourseIdAsync(int courseId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmCourseFees.CrmCourseFeesByCourseIdAsync(courseId, trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmCourseFeeDto>() : Enumerable.Empty<CrmCourseFeeDto>();
    }

    public async Task<GridEntity<CrmCourseFeeDto>> CourseFeesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT CourseFeeId, CourseId, IntakeId, FeeType, Amount, Currency, PaymentSchedule, CreatedDate FROM CrmCourseFee";
        const string orderBy = "CourseFeeId ASC";
        return await _repository.CrmCourseFees.AdoGridDataAsync<CrmCourseFeeDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }
}
