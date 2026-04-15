using Domain.Entities.Entities.CRM;
using Domain.Entities.Entities.System;
using Domain.Contracts.Services.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Domain.Contracts.Services.CRM;
using bdDevs.Shared.DataTransferObjects.CRM;
using Domain.Exceptions;
using Domain.Contracts.Repositories;
using Application.Shared.Grid;
using Application.Services.Mappings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using bdDevs.Shared.Records.CRM;
using bdDevs.Shared.Extensions;

namespace Application.Services.CRM;


internal sealed class CrmYearService : ICrmYearService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<CrmYearService> _logger;
	private readonly IConfiguration _config;

	/// <summary>
	/// Initializes a new instance of <see cref="CrmYearService"/> with required dependencies.
	/// </summary>
	public CrmYearService(IRepositoryManager repository, ILogger<CrmYearService> logger, IConfiguration config)
	{
		_repository = repository;
		_logger = logger;
		_config = config;
	}

	/// <summary>
	/// Creates a new year record using CRUD Record pattern.
	/// </summary>
	public async Task<CrmYearDto> CreateAsync(CreateCrmYearRecord record, CancellationToken cancellationToken = default)
	{
		if (record == null)
			throw new BadRequestException(nameof(CreateCrmYearRecord));

		_logger.LogInformation("Creating new year. YearName: {YearName}, Time: {Time}",
						record.YearName, DateTime.UtcNow);

		CrmYear year = record.MapTo<CrmYear>();
		int yearId = await _repository.CrmYears.CreateAndIdAsync(year, cancellationToken);
		await _repository.SaveAsync(cancellationToken);

		_logger.LogInformation("Year created successfully. ID: {YearId}, Time: {Time}",
						yearId, DateTime.UtcNow);

		var resultDto = year.MapTo<CrmYearDto>();
		resultDto.YearId = yearId;
		return resultDto;
	}

	/// <summary>
	/// Updates an existing year record using CRUD Record pattern.
	/// </summary>
	public async Task<CrmYearDto> UpdateAsync(UpdateCrmYearRecord record, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (record == null)
			throw new BadRequestException(nameof(UpdateCrmYearRecord));

		_logger.LogInformation("Updating year. ID: {YearId}, Time: {Time}", record.YearId, DateTime.UtcNow);

		var existingYear = await _repository.CrmYears
						.FirstOrDefaultAsync(x => x.YearId == record.YearId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("Year", "YearId", record.YearId.ToString());

		CrmYear year = record.MapTo<CrmYear>();
		_repository.CrmYears.UpdateByState(year);
		await _repository.SaveAsync(cancellationToken);

		_logger.LogInformation("Year updated successfully. ID: {YearId}, Time: {Time}",
						record.YearId, DateTime.UtcNow);

		return year.MapTo<CrmYearDto>();
	}

	/// <summary>
	/// Deletes a year record using CRUD Record pattern.
	/// </summary>
	public async Task DeleteAsync(DeleteCrmYearRecord record, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (record == null || record.YearId <= 0)
			throw new BadRequestException("Invalid delete request!");

		_logger.LogInformation("Deleting year. ID: {YearId}, Time: {Time}", record.YearId, DateTime.UtcNow);

		var yearEntity = await _repository.CrmYears
						.FirstOrDefaultAsync(x => x.YearId == record.YearId, trackChanges, cancellationToken)
						?? throw new NotFoundException("Year", "YearId", record.YearId.ToString());

		await _repository.CrmYears.DeleteAsync(x => x.YearId == record.YearId, trackChanges: false, cancellationToken);
		await _repository.SaveAsync(cancellationToken);

		_logger.LogWarning("Year deleted successfully. ID: {YearId}, Time: {Time}",
						record.YearId, DateTime.UtcNow);
	}

	/// <summary>
	/// Retrieves a single year record by its ID.
	/// </summary>
	public async Task<CrmYearDto> YearAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching year. ID: {YearId}, Time: {Time}", id, DateTime.UtcNow);
		var year = await _repository.CrmYears
						.FirstOrDefaultAsync(x => x.YearId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("Year", "YearId", id.ToString());

		_logger.LogInformation("Year fetched successfully. ID: {YearId}, Time: {Time}",
						id, DateTime.UtcNow);

		return year.MapTo<CrmYearDto>();
	}

	/// <summary>
	/// Retrieves all year records from the database.
	/// </summary>
	public async Task<IEnumerable<CrmYearDto>> YearsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all years. Time: {Time}", DateTime.UtcNow);
		var years = await _repository.CrmYears.ListAsync(x => x.YearId, trackChanges, cancellationToken);

		if (!years.Any())
		{
			_logger.LogWarning("No years found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmYearDto>();
		}

		var yearsDto = years.MapToList<CrmYearDto>();
		_logger.LogInformation("Years fetched successfully. Count: {Count}, Time: {Time}",
						yearsDto.Count(), DateTime.UtcNow);

		return yearsDto;
	}

	/// <summary>
	/// Retrieves active year records from the database.
	/// </summary>
	public async Task<IEnumerable<CrmYearDto>> ActiveYearsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching active years. Time: {Time}", DateTime.UtcNow);

		var years = await _repository.CrmYears.CrmYearsAsync(trackChanges, cancellationToken);
		//var years = await _repository.CrmYears.GetActiveYearAsync(trackChanges, cancellationToken);

		if (!years.Any())
		{
			_logger.LogWarning("No active years found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmYearDto>();
		}

		var yearsDto = years.MapToList<CrmYearDto>();

		_logger.LogInformation("Active years fetched successfully. Count: {Count}, Time: {Time}",
						yearsDto.Count(), DateTime.UtcNow);

		return yearsDto;
	}

	/// <summary>
	/// Retrieves a lightweight list of all years suitable for use in dropdown lists.
	/// </summary>
	public async Task<IEnumerable<CrmYearDto>> YearForDDLAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching years for dropdown list. Time: {Time}", DateTime.UtcNow);
		var years = await _repository.CrmYears.CrmYearsAsync(false, cancellationToken);

		if (!years.Any())
		{
			_logger.LogWarning("No years found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmYearDto>();
		}

		var yearsDto = years.MapToList<CrmYearDto>();
		_logger.LogInformation("Years fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						yearsDto.Count(), DateTime.UtcNow);

		return yearsDto;
	}

	/// <summary>
	/// Retrieves years by the specified applicant ID.
	/// </summary>
	public async Task<IEnumerable<CrmYearDto>> YearsByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (applicantId <= 0)
		{
			_logger.LogWarning("YearsByApplicantIdAsync called with invalid applicantId: {ApplicantId}", applicantId);
			throw new BadRequestException("Invalid request!");
		}

		_logger.LogInformation("Fetching years for applicant ID: {ApplicantId}, Time: {Time}", applicantId, DateTime.UtcNow);
		//var applicantCourses = await _repository.CrmApplicantCourses.GetApplicantCoursesByApplicantIdAsync(applicantId, trackChanges, cancellationToken);
		var applicantCourses = await _repository.CrmApplicantCourses.CrmApplicantCoursesByApplicantIdAsync(applicantId, trackChanges, cancellationToken);

		if (!applicantCourses.Any())
		{
			_logger.LogWarning("No applicant courses found for applicant ID: {ApplicantId}, Time: {Time}",
							applicantId, DateTime.UtcNow);
			return Enumerable.Empty<CrmYearDto>();
		}

		var intakeYearIds = applicantCourses.Where(ac => ac.IntakeYearId > 0).Select(ac => ac.IntakeYearId).Distinct().ToList();

		if (!intakeYearIds.Any())
		{
			_logger.LogWarning("No intake year IDs found for applicant ID: {ApplicantId}, Time: {Time}",
							applicantId, DateTime.UtcNow);
			return Enumerable.Empty<CrmYearDto>();
		}

		var years = await _repository.CrmYears.ListByConditionAsync(
						x => intakeYearIds.Contains(x.YearId),
						x => x.YearName,
						trackChanges: trackChanges,
						descending: false,
						cancellationToken: cancellationToken);

		if (!years.Any())
		{
			_logger.LogWarning("No years found for applicant ID: {ApplicantId}, Time: {Time}",
							applicantId, DateTime.UtcNow);
			return Enumerable.Empty<CrmYearDto>();
		}

		var yearsDto = years.MapToList<CrmYearDto>();

		_logger.LogInformation("Years fetched successfully for applicant ID: {ApplicantId}. Count: {Count}, Time: {Time}",
						applicantId, yearsDto.Count(), DateTime.UtcNow);

		return yearsDto;
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all years.
	/// </summary>
	public async Task<GridEntity<CrmYearDto>> YearsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching years summary grid. Time: {Time}", DateTime.UtcNow);

		const string sql = @"
SELECT YearId, YearName, YearCode, Description, IsActive, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy
FROM CrmYear";

		const string orderBy = "YearName ASC";

		
		return await _repository.CrmYears.AdoGridDataAsync<CrmYearDto>(sql, options, orderBy, string.Empty, cancellationToken);
	}
}
