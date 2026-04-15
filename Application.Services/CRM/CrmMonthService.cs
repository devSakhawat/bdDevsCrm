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
using bdDevs.Shared.Records.CRM;
using bdDevs.Shared.Extensions;

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
	/// Creates a new month record using CRUD Record pattern.
	/// </summary>
	public async Task<CrmMonthDto> CreateAsync(CreateCrmMonthRecord record, CancellationToken cancellationToken = default)
	{
		if (record == null)
			throw new BadRequestException(nameof(CreateCrmMonthRecord));

		_logger.LogInformation("Creating new month. MonthName: {MonthName}, Time: {Time}",
						record.MonthName, DateTime.UtcNow);

		// Map Record to Entity using Mapster
		CrmMonth month = record.MapTo<CrmMonth>();
		int monthId = await _repository.CrmMonths.CreateAndIdAsync(month, cancellationToken);
		await _repository.SaveAsync(cancellationToken);

		_logger.LogInformation("Month created successfully. ID: {MonthId}, Time: {Time}",
						monthId, DateTime.UtcNow);

		// Return as DTO
		var resultDto = month.MapTo<CrmMonthDto>();
		resultDto.MonthId = monthId;
		return resultDto;
	}

	/// <summary>
	/// Updates an existing month record using CRUD Record pattern.
	/// </summary>
	public async Task<CrmMonthDto> UpdateAsync(UpdateCrmMonthRecord record, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (record == null)
			throw new BadRequestException(nameof(UpdateCrmMonthRecord));

		_logger.LogInformation("Updating month. ID: {MonthId}, Time: {Time}", record.MonthId, DateTime.UtcNow);

		// Check if month exists
		var existingMonth = await _repository.CrmMonths
						.FirstOrDefaultAsync(x => x.MonthId == record.MonthId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("Month", "MonthId", record.MonthId.ToString());

		// Map Record to Entity using Mapster
		CrmMonth month = record.MapTo<CrmMonth>();
		_repository.CrmMonths.UpdateByState(month);
		await _repository.SaveAsync(cancellationToken);

		_logger.LogInformation("Month updated successfully. ID: {MonthId}, Time: {Time}",
						record.MonthId, DateTime.UtcNow);

		return month.MapTo<CrmMonthDto>();
	}

	/// <summary>
	/// Deletes a month record using CRUD Record pattern.
	/// </summary>
	public async Task DeleteAsync(DeleteCrmMonthRecord record, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (record == null || record.MonthId <= 0)
			throw new BadRequestException("Invalid delete request!");

		_logger.LogInformation("Deleting month. ID: {MonthId}, Time: {Time}", record.MonthId, DateTime.UtcNow);

		var monthEntity = await _repository.CrmMonths
						.FirstOrDefaultAsync(x => x.MonthId == record.MonthId, trackChanges, cancellationToken)
						?? throw new NotFoundException("Month", "MonthId", record.MonthId.ToString());

		await _repository.CrmMonths.DeleteAsync(x => x.MonthId == record.MonthId, trackChanges: false, cancellationToken);
		await _repository.SaveAsync(cancellationToken);

		_logger.LogWarning("Month deleted successfully. ID: {MonthId}, Time: {Time}",
						record.MonthId, DateTime.UtcNow);
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

		return month.MapTo<CrmMonthDto>();
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

		var monthsDto = months.MapToList<CrmMonthDto>();

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

		if (!months.Any())
		{
			_logger.LogWarning("No active months found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmMonthDto>();
		}

		var monthsDto = months.MapToList<CrmMonthDto>();

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

		var monthsDto = months.MapToList<CrmMonthDto>();

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

		var monthsDto = months.MapToList<CrmMonthDto>();

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

		(sql, options, orderBy, string.Empty, cancellationToken);
		return await _repository.CrmMonths.AdoGridDataAsync<CrmMonthDto>(sql, options, orderBy, string.Empty, cancellationToken);
	}
}