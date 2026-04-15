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
/// CrmIntakeMonth service implementing business logic for CrmIntakeMonth management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmIntakeMonthService : ICrmIntakeMonthService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<CrmIntakeMonthService> _logger;
	private readonly IConfiguration _config;

	/// <summary>
	/// Initializes a new instance of <see cref="CrmIntakeMonthService"/> with required dependencies.
	/// </summary>
	public CrmIntakeMonthService(IRepositoryManager repository, ILogger<CrmIntakeMonthService> logger, IConfiguration configuration)
	{
		_repository = repository;
		_logger = logger;
		_config = configuration;
	}

	/// <summary>
	/// Creates a new intake month record using CRUD Record pattern.
	/// </summary>
	public async Task<CrmIntakeMonthDto> CreateAsync(CreateCrmIntakeMonthRecord record, CancellationToken cancellationToken = default)
	{
		if (record == null)
			throw new BadRequestException(nameof(CreateCrmIntakeMonthRecord));

		_logger.LogInformation("Creating new intake month. MonthName: {MonthName}, Time: {Time}",
						record.MonthName, DateTime.UtcNow);

		// Map Record to Entity using Mapster
		CrmIntakeMonth intakeMonth = record.MapTo<CrmIntakeMonth>();
		int intakeMonthId = await _repository.CrmIntakeMonths.CreateAndIdAsync(intakeMonth, cancellationToken);
		await _repository.SaveAsync(cancellationToken);

		_logger.LogInformation("Intake month created successfully. ID: {IntakeMonthId}, Time: {Time}",
						intakeMonthId, DateTime.UtcNow);

		// Return as DTO
		var resultDto = intakeMonth.MapTo<CrmIntakeMonthDto>();
		resultDto.IntakeMonthId = intakeMonthId;
		return resultDto;
	}

	/// <summary>
	/// Updates an existing intake month record using CRUD Record pattern.
	/// </summary>
	public async Task<CrmIntakeMonthDto> UpdateAsync(UpdateCrmIntakeMonthRecord record, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (record == null)
			throw new BadRequestException(nameof(UpdateCrmIntakeMonthRecord));

		_logger.LogInformation("Updating intake month. ID: {IntakeMonthId}, Time: {Time}", record.IntakeMonthId, DateTime.UtcNow);

		// Check if intake month exists
		var existingIntakeMonth = await _repository.CrmIntakeMonths
						.FirstOrDefaultAsync(x => x.IntakeMonthId == record.IntakeMonthId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("IntakeMonth", "IntakeMonthId", record.IntakeMonthId.ToString());

		// Map Record to Entity using Mapster
		CrmIntakeMonth intakeMonth = record.MapTo<CrmIntakeMonth>();
		_repository.CrmIntakeMonths.UpdateByState(intakeMonth);
		await _repository.SaveAsync(cancellationToken);

		_logger.LogInformation("Intake month updated successfully. ID: {IntakeMonthId}, Time: {Time}",
						record.IntakeMonthId, DateTime.UtcNow);

		return intakeMonth.MapTo<CrmIntakeMonthDto>();
	}

	/// <summary>
	/// Deletes an intake month record using CRUD Record pattern.
	/// </summary>
	public async Task DeleteAsync(DeleteCrmIntakeMonthRecord record, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (record == null || record.IntakeMonthId <= 0)
			throw new BadRequestException("Invalid delete request!");

		_logger.LogInformation("Deleting intake month. ID: {IntakeMonthId}, Time: {Time}", record.IntakeMonthId, DateTime.UtcNow);

		var intakeMonthEntity = await _repository.CrmIntakeMonths
						.FirstOrDefaultAsync(x => x.IntakeMonthId == record.IntakeMonthId, trackChanges, cancellationToken)
						?? throw new NotFoundException("IntakeMonth", "IntakeMonthId", record.IntakeMonthId.ToString());

		await _repository.CrmIntakeMonths.DeleteAsync(x => x.IntakeMonthId == record.IntakeMonthId, trackChanges: false, cancellationToken);
		await _repository.SaveAsync(cancellationToken);

		_logger.LogWarning("Intake month deleted successfully. ID: {IntakeMonthId}, Time: {Time}",
						record.IntakeMonthId, DateTime.UtcNow);
	}

	/// <summary>
	/// Retrieves a single intake month record by its ID.
	/// </summary>
	public async Task<CrmIntakeMonthDto> IntakeMonthAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching intake month. ID: {IntakeMonthId}, Time: {Time}", id, DateTime.UtcNow);

		var intakeMonth = await _repository.CrmIntakeMonths
						.FirstOrDefaultAsync(x => x.IntakeMonthId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("IntakeMonth", "IntakeMonthId", id.ToString());

		_logger.LogInformation("Intake month fetched successfully. ID: {IntakeMonthId}, Time: {Time}",
						id, DateTime.UtcNow);

		return intakeMonth.MapTo<CrmIntakeMonthDto>();
	}

	/// <summary>
	/// Retrieves all intake month records from the database.
	/// </summary>
	public async Task<IEnumerable<CrmIntakeMonthDto>> IntakeMonthsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all intake months. Time: {Time}", DateTime.UtcNow);

		var intakeMonths = await _repository.CrmIntakeMonths.ListAsync(x => x.IntakeMonthId, trackChanges, cancellationToken);

		if (!intakeMonths.Any())
		{
			_logger.LogWarning("No intake months found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmIntakeMonthDto>();
		}

		var intakeMonthsDto = intakeMonths.MapToList<CrmIntakeMonthDto>();

		_logger.LogInformation("Intake months fetched successfully. Count: {Count}, Time: {Time}",
						intakeMonthsDto.Count(), DateTime.UtcNow);

		return intakeMonthsDto;
	}

	/// <summary>
	/// Retrieves active intake month records from the database.
	/// </summary>
	public async Task<IEnumerable<CrmIntakeMonthDto>> ActiveIntakeMonthsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching active intake months. Time: {Time}", DateTime.UtcNow);

		var intakeMonths = await _repository.CrmIntakeMonths.CrmIntakeMonthsAsync(trackChanges, cancellationToken);

		if (!intakeMonths.Any())
		{
			_logger.LogWarning("No active intake months found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmIntakeMonthDto>();
		}

		var intakeMonthsDto = intakeMonths.MapToList<CrmIntakeMonthDto>();

		_logger.LogInformation("Active intake months fetched successfully. Count: {Count}, Time: {Time}",
						intakeMonthsDto.Count(), DateTime.UtcNow);

		return intakeMonthsDto;
	}

	/// <summary>
	/// Retrieves a lightweight list of all intake months suitable for use in dropdown lists.
	/// </summary>
	public async Task<IEnumerable<CrmIntakeMonthDto>> IntakeMonthForDDLAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching intake months for dropdown list. Time: {Time}", DateTime.UtcNow);

		var intakeMonths = await _repository.CrmIntakeMonths.CrmIntakeMonthsAsync(false, cancellationToken);

		if (!intakeMonths.Any())
		{
			_logger.LogWarning("No intake months found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmIntakeMonthDto>();
		}

		var intakeMonthsDto = intakeMonths.MapToList<CrmIntakeMonthDto>();

		_logger.LogInformation("Intake months fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						intakeMonthsDto.Count(), DateTime.UtcNow);

		return intakeMonthsDto;
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all intake months.
	/// </summary>
	public async Task<GridEntity<CrmIntakeMonthDto>> IntakeMonthsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching intake months summary grid. Time: {Time}", DateTime.UtcNow);

		const string sql = @"
SELECT IntakeMonthId, MonthName, MonthCode, MonthNumber, Description, IsActive, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy
FROM CrmIntakeMonth";

		const string orderBy = "MonthNumber ASC";

		return await _repository.CrmIntakeMonths.AdoGridDataAsync<CrmIntakeMonthDto>(sql, options, orderBy, string.Empty, cancellationToken);
	}
}
