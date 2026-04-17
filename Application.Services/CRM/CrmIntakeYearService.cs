using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;
using Domain.Contracts.Services.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.CRM;
using Domain.Contracts.Services.CRM;
using Domain.Exceptions;
using Application.Shared.Grid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using bdDevs.Shared.Records.CRM;
using bdDevs.Shared.Extensions;

namespace Application.Services.CRM;

/// <summary>
/// CrmIntakeYear service implementing business logic for CrmIntakeYear management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmIntakeYearService : ICrmIntakeYearService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<CrmIntakeYearService> _logger;
	private readonly IConfiguration _config;

	/// <summary>
	/// Initializes a new instance of <see cref="CrmIntakeYearService"/> with required dependencies.
	/// </summary>
	public CrmIntakeYearService(IRepositoryManager repository, ILogger<CrmIntakeYearService> logger, IConfiguration configuration)
	{
		_repository = repository;
		_logger = logger;
		_config = configuration;
	}

	/// <summary>
	/// Creates a new intake year record using CRUD Record pattern.
	/// </summary>
	public async Task<CrmIntakeYearDto> CreateAsync(CreateCrmIntakeYearRecord record, CancellationToken cancellationToken = default)
	{
		if (record == null)
			throw new BadRequestException(nameof(CreateCrmIntakeYearRecord));

		_logger.LogInformation("Creating new intake year. YearName: {YearName}, Time: {Time}",
						record.YearName, DateTime.UtcNow);

		// Map Record to Entity using Mapster
		CrmIntakeYear intakeYear = record.MapTo<CrmIntakeYear>();
		int intakeYearId = await _repository.CrmIntakeYears.CreateAndIdAsync(intakeYear, cancellationToken);
		await _repository.SaveAsync(cancellationToken);

		_logger.LogInformation("Intake year created successfully. ID: {IntakeYearId}, Time: {Time}",
						intakeYearId, DateTime.UtcNow);

		// Return as DTO
		var resultDto = intakeYear.MapTo<CrmIntakeYearDto>() with { IntakeYearId = intakeYearId };
		return resultDto;
	}

	/// <summary>
	/// Updates an existing intake year record using CRUD Record pattern.
	/// </summary>
	public async Task<CrmIntakeYearDto> UpdateAsync(UpdateCrmIntakeYearRecord record, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (record == null)
			throw new BadRequestException(nameof(UpdateCrmIntakeYearRecord));

		_logger.LogInformation("Updating intake year. ID: {IntakeYearId}, Time: {Time}", record.IntakeYearId, DateTime.UtcNow);

		// Check if intake year exists
		var existingIntakeYear = await _repository.CrmIntakeYears
						.FirstOrDefaultAsync(x => x.IntakeYearId == record.IntakeYearId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("IntakeYear", "IntakeYearId", record.IntakeYearId.ToString());

		// Map Record to Entity using Mapster
		CrmIntakeYear intakeYear = record.MapTo<CrmIntakeYear>();
		_repository.CrmIntakeYears.UpdateByState(intakeYear);
		await _repository.SaveAsync(cancellationToken);

		_logger.LogInformation("Intake year updated successfully. ID: {IntakeYearId}, Time: {Time}",
						record.IntakeYearId, DateTime.UtcNow);

		return intakeYear.MapTo<CrmIntakeYearDto>();
	}

	/// <summary>
	/// Deletes an intake year record using CRUD Record pattern.
	/// </summary>
	public async Task DeleteAsync(DeleteCrmIntakeYearRecord record, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (record == null || record.IntakeYearId <= 0)
			throw new BadRequestException("Invalid delete request!");

		_logger.LogInformation("Deleting intake year. ID: {IntakeYearId}, Time: {Time}", record.IntakeYearId, DateTime.UtcNow);

		var intakeYearEntity = await _repository.CrmIntakeYears
						.FirstOrDefaultAsync(x => x.IntakeYearId == record.IntakeYearId, trackChanges, cancellationToken)
						?? throw new NotFoundException("IntakeYear", "IntakeYearId", record.IntakeYearId.ToString());

		await _repository.CrmIntakeYears.DeleteAsync(x => x.IntakeYearId == record.IntakeYearId, trackChanges: false, cancellationToken);
		await _repository.SaveAsync(cancellationToken);

		_logger.LogWarning("Intake year deleted successfully. ID: {IntakeYearId}, Time: {Time}",
						record.IntakeYearId, DateTime.UtcNow);
	}

	/// <summary>
	/// Retrieves a single intake year record by its ID.
	/// </summary>
	public async Task<CrmIntakeYearDto> IntakeYearAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching intake year. ID: {IntakeYearId}, Time: {Time}", id, DateTime.UtcNow);

		var intakeYear = await _repository.CrmIntakeYears
						.FirstOrDefaultAsync(x => x.IntakeYearId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("IntakeYear", "IntakeYearId", id.ToString());

		_logger.LogInformation("Intake year fetched successfully. ID: {IntakeYearId}, Time: {Time}",
						id, DateTime.UtcNow);

		return intakeYear.MapTo<CrmIntakeYearDto>();
	}

	/// <summary>
	/// Retrieves all intake year records from the database.
	/// </summary>
	public async Task<IEnumerable<CrmIntakeYearDto>> IntakeYearsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all intake years. Time: {Time}", DateTime.UtcNow);

		var intakeYears = await _repository.CrmIntakeYears.ListAsync(x => x.IntakeYearId, trackChanges, cancellationToken);

		if (!intakeYears.Any())
		{
			_logger.LogWarning("No intake years found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmIntakeYearDto>();
		}

		var intakeYearsDto = intakeYears.MapToList<CrmIntakeYearDto>();

		_logger.LogInformation("Intake years fetched successfully. Count: {Count}, Time: {Time}",
						intakeYearsDto.Count(), DateTime.UtcNow);

		return intakeYearsDto;
	}

	/// <summary>
	/// Retrieves active intake year records from the database.
	/// </summary>
	public async Task<IEnumerable<CrmIntakeYearDto>> ActiveIntakeYearsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching active intake years. Time: {Time}", DateTime.UtcNow);

		var intakeYears = await _repository.CrmIntakeYears.CrmIntakeYearsAsync(trackChanges, cancellationToken);

		if (!intakeYears.Any())
		{
			_logger.LogWarning("No active intake years found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmIntakeYearDto>();
		}

		var intakeYearsDto = intakeYears.MapToList<CrmIntakeYearDto>();

		_logger.LogInformation("Active intake years fetched successfully. Count: {Count}, Time: {Time}",
						intakeYearsDto.Count(), DateTime.UtcNow);

		return intakeYearsDto;
	}

	/// <summary>
	/// Retrieves a lightweight list of all intake years suitable for use in dropdown lists.
	/// </summary>
	public async Task<IEnumerable<CrmIntakeYearDto>> IntakeYearForDDLAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching intake years for dropdown list. Time: {Time}", DateTime.UtcNow);

		var intakeYears = await _repository.CrmIntakeYears.CrmIntakeYearsAsync(false, cancellationToken);

		if (!intakeYears.Any())
		{
			_logger.LogWarning("No intake years found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmIntakeYearDto>();
		}

		var intakeYearsDto = intakeYears.MapToList<CrmIntakeYearDto>();

		_logger.LogInformation("Intake years fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						intakeYearsDto.Count(), DateTime.UtcNow);

		return intakeYearsDto;
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all intake years.
	/// </summary>
	public async Task<GridEntity<CrmIntakeYearDto>> IntakeYearsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching intake years summary grid. Time: {Time}", DateTime.UtcNow);

		const string sql = @"
SELECT IntakeYearId, YearName, YearCode, YearValue, Description, IsActive, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy
FROM CrmIntakeYear";

		const string orderBy = "YearValue DESC";

		return await _repository.CrmIntakeYears.AdoGridDataAsync<CrmIntakeYearDto>(sql, options, orderBy, string.Empty, cancellationToken);
	}
}
