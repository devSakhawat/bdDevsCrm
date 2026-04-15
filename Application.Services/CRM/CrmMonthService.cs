using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;
using Domain.Contracts.Services.Core.SystemAdmin;
using Domain.Contracts.Services.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.CRM;
using Domain.Exceptions;
using Application.Shared.Grid;
using Application.Services.Mappings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.CRM;

/// <summary>
/// CRM Month service implementing business logic for month management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmMonthService : ICrmMonthService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<CrmMonthService> _logger;
	private readonly IConfiguration _config;

	/// <summary>
	/// Initializes a new instance of <see cref="CrmMonthService"/> with required dependencies.
	/// </summary>
	public CrmMonthService(IRepositoryManager repository, ILogger<CrmMonthService> logger, IConfiguration config)
	{
		_repository = repository;
		_logger = logger;
		_config = config;
	}

	/// <summary>
	/// Creates a new month record.
	/// </summary>
	public async Task<CrmMonthDto> CreateMonthAsync(CrmMonthDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(CrmMonthDto));

		if (entityForCreate.MonthId != 0)
			throw new InvalidCreateOperationException("MonthId must be 0 for new record.");

		_logger.LogInformation("Creating new month. MonthName: {MonthName}, Time: {Time}",
						entityForCreate.MonthName, DateTime.UtcNow);

		var monthEntity = MyMapper.JsonClone<CrmMonthDto, CrmMonth>(entityForCreate);
		//monthEntity.CreatedDate = DateTime.UtcNow;
		//monthEntity.CreatedBy = currentUser.UserId ?? 0;
		//monthEntity.IsActive = true;

		await _repository.CrmMonths.CreateAsync(monthEntity, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("Month could not be saved to the database.");

		_logger.LogInformation("Month created successfully. ID: {MonthId}, Time: {Time}",
						monthEntity.MonthId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmMonth, CrmMonthDto>(monthEntity);
	}

	/// <summary>
	/// Updates an existing month record.
	/// </summary>
	public async Task<CrmMonthDto> UpdateMonthAsync(int monthId, CrmMonthDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(CrmMonthDto));

		if (monthId != modelDto.MonthId)
			throw new BadRequestException(monthId.ToString(), nameof(CrmMonthDto));

		_logger.LogInformation("Updating month. ID: {MonthId}, Time: {Time}", monthId, DateTime.UtcNow);

		var monthEntity = await _repository.CrmMonths
						.FirstOrDefaultAsync(x => x.MonthId == monthId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("Month", "MonthId", monthId.ToString());

		var updatedEntity = MyMapper.MergeChangedValues<CrmMonth, CrmMonthDto>(monthEntity, modelDto);
		//updatedEntity.UpdatedDate = DateTime.UtcNow;

		_repository.CrmMonths.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("Month", "MonthId", monthId.ToString());

		_logger.LogInformation("Month updated successfully. ID: {MonthId}, Time: {Time}",
						monthId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmMonth, CrmMonthDto>(updatedEntity);
	}

	/// <summary>
	/// Deletes a month record identified by the given ID.
	/// </summary>
	public async Task<int> DeleteMonthAsync(int monthId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (monthId <= 0)
			throw new BadRequestException(monthId.ToString(), nameof(CrmMonthDto));

		_logger.LogInformation("Deleting month. ID: {MonthId}, Time: {Time}", monthId, DateTime.UtcNow);

		var monthEntity = await _repository.CrmMonths
						.FirstOrDefaultAsync(x => x.MonthId == monthId, trackChanges, cancellationToken)
						?? throw new NotFoundException("Month", "MonthId", monthId.ToString());

		await _repository.CrmMonths.DeleteAsync(x => x.MonthId == monthId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("Month", "MonthId", monthId.ToString());

		_logger.LogWarning("Month deleted successfully. ID: {MonthId}, Time: {Time}",
						monthId, DateTime.UtcNow);

		return affected;
	}

	/// <summary>
	/// Retrieves a single month record by its ID.
	/// </summary>
	public async Task<CrmMonthDto> MonthAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching month. ID: {MonthId}, Time: {Time}", id, DateTime.UtcNow);

		var month = await _repository.CrmMonths
						.FirstOrDefaultAsync(x => x.MonthId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("Month", "MonthId", id.ToString());

		_logger.LogInformation("Month fetched successfully. ID: {MonthId}, Time: {Time}",
						id, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmMonth, CrmMonthDto>(month);
	}

	/// <summary>
	/// Retrieves all month records from the database.
	/// </summary>
	public async Task<IEnumerable<CrmMonthDto>> MonthsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all months. Time: {Time}", DateTime.UtcNow);

		var months = await _repository.CrmMonths.ListAsync(x => x.MonthId, trackChanges, cancellationToken);

		if (!months.Any())
		{
			_logger.LogWarning("No months found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmMonthDto>();
		}

		var monthsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmMonth, CrmMonthDto>(months);

		_logger.LogInformation("Months fetched successfully. Count: {Count}, Time: {Time}",
						monthsDto.Count(), DateTime.UtcNow);

		return monthsDto;
	}

	/// <summary>
	/// Retrieves active month records from the database.
	/// </summary>
	public async Task<IEnumerable<CrmMonthDto>> ActiveMonthsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching active months. Time: {Time}", DateTime.UtcNow);

		var months = await _repository.CrmMonths.CrmMonthsAsync(trackChanges, cancellationToken);
		//var months = await _repository.CrmMonths.GetActiveMonthAsync(trackChanges, cancellationToken);

		if (!months.Any())
		{
			_logger.LogWarning("No active months found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmMonthDto>();
		}

		var monthsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmMonth, CrmMonthDto>(months);

		_logger.LogInformation("Active months fetched successfully. Count: {Count}, Time: {Time}",
						monthsDto.Count(), DateTime.UtcNow);

		return monthsDto;
	}

	/// <summary>
	/// Retrieves a lightweight list of all months suitable for use in dropdown lists.
	/// </summary>
	public async Task<IEnumerable<CrmMonthDto>> MonthForDDLAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching months for dropdown list. Time: {Time}", DateTime.UtcNow);

		var months = await _repository.CrmMonths.CrmMonthsAsync(false, cancellationToken);

		if (!months.Any())
		{
			_logger.LogWarning("No months found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmMonthDto>();
		}

		var monthsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmMonth, CrmMonthDto>(months);

		_logger.LogInformation("Months fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						monthsDto.Count(), DateTime.UtcNow);

		return monthsDto;
	}

	/// <summary>
	/// Retrieves months by the specified applicant ID.
	/// </summary>
	public async Task<IEnumerable<CrmMonthDto>> MonthsByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (applicantId <= 0)
		{
			_logger.LogWarning("MonthsByApplicantIdAsync called with invalid applicantId: {ApplicantId}", applicantId);
			throw new BadRequestException("Invalid request!");
		}

		_logger.LogInformation("Fetching months for applicant ID: {ApplicantId}, Time: {Time}", applicantId, DateTime.UtcNow);

		//var applicantCourses = await _repository.CrmApplicantCourses.GetApplicantCoursesByApplicantIdAsync(applicantId, trackChanges, cancellationToken);
		var applicantCourses = await _repository.CrmApplicantCourses.CrmApplicantCoursesByApplicantIdAsync(applicantId, trackChanges, cancellationToken);

		if (!applicantCourses.Any())
		{
			_logger.LogWarning("No applicant courses found for applicant ID: {ApplicantId}, Time: {Time}",
							applicantId, DateTime.UtcNow);
			return Enumerable.Empty<CrmMonthDto>();
		}

		var intakeMonthIds = applicantCourses
						.Where(ac => ac.IntakeMonthId > 0)
						.Select(ac => ac.IntakeMonthId)
						.Distinct()
						.ToList();

		if (!intakeMonthIds.Any())
		{
			_logger.LogWarning("No intake month IDs found for applicant ID: {ApplicantId}, Time: {Time}",
							applicantId, DateTime.UtcNow);
			return Enumerable.Empty<CrmMonthDto>();
		}

		var months = await _repository.CrmMonths.ListByConditionAsync(
						x => intakeMonthIds.Contains(x.MonthId),
						x => x.MonthName,
						trackChanges: trackChanges,
						descending: false,
						cancellationToken: cancellationToken);

		if (!months.Any())
		{
			_logger.LogWarning("No months found for applicant ID: {ApplicantId}, Time: {Time}",
							applicantId, DateTime.UtcNow);
			return Enumerable.Empty<CrmMonthDto>();
		}

		var monthsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmMonth, CrmMonthDto>(months);

		_logger.LogInformation("Months fetched successfully for applicant ID: {ApplicantId}. Count: {Count}, Time: {Time}",
						applicantId, monthsDto.Count(), DateTime.UtcNow);

		return monthsDto;
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all months.
	/// </summary>
	public async Task<GridEntity<CrmMonthDto>> MonthsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching months summary grid. Time: {Time}", DateTime.UtcNow);

		const string sql = @"
SELECT MonthId, MonthName, MonthCode, MonthNumber, Description, IsActive, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy
FROM CrmMonth";

		const string orderBy = "MonthNumber ASC";

		//return await _repository.CrmMonths.GridData<CrmMonthDto>(sql, options, orderBy, string.Empty, cancellationToken);
		return await _repository.CrmMonths.AdoGridDataAsync<CrmMonthDto>(sql, options, orderBy, string.Empty, cancellationToken);
	}
}