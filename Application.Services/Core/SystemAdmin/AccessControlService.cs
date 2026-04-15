using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;
using Domain.Contracts.Services.Core.SystemAdmin;
using Application.Services.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Domain.Exceptions;
using Application.Shared.Grid;
using Application.Services.Mappings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.Core.SystemAdmin;

/// <summary>
/// Access control service implementing business logic for access control management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class AccessControlService : IAccessControlService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<AccessControlService> _logger;
	private readonly IConfiguration _configuration;

	public AccessControlService(IRepositoryManager repository, ILogger<AccessControlService> logger, IConfiguration configuration)
	{
		_repository = repository;
		_logger = logger;
		_configuration = configuration;
	}

	/// <summary>
	/// Creates a new access control record after validating for null input and duplicate access control name.
	/// </summary>
	/// <param name="entityForCreate">The DTO containing data for the new access control.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="AccessControlDto"/> with the newly assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
	/// <exception cref="DuplicateRecordException">Thrown when an access control with the same name already exists.</exception>
	public async Task<AccessControlDto> CreateAsync(AccessControlDto entityForCreate, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(AccessControlDto));

		bool accessControlExists = await _repository.AccessControls.ExistsAsync(
				ac => ac.AccessName.Trim().ToLower() == entityForCreate.AccessName.Trim().ToLower(),
				cancellationToken: cancellationToken);

		if (accessControlExists)
			throw new DuplicateRecordException("AccessControl", "AccessName");

		AccessControl accessControlEntity = MyMapper.JsonClone<AccessControlDto, AccessControl>(entityForCreate);

		await _repository.AccessControls.CreateAsync(accessControlEntity, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("AccessControl could not be saved to the database.");
		_logger.LogInformation(
				"AccessControl created successfully. ID: {AccessId}, Name: {AccessName}, Time: {Time}",
				accessControlEntity.AccessId,
				accessControlEntity.AccessName,
				DateTime.UtcNow);

		return MyMapper.JsonClone<AccessControl, AccessControlDto>(accessControlEntity);
	}

	/// <summary>
	/// Updates an existing access control record by merging only the changed values from the provided DTO.
	/// Validates ID consistency, null input, record existence, and duplicate name constraints.
	/// </summary>
	/// <param name="accessControlId">The ID of the access control to update.</param>
	/// <param name="modelDto">The DTO containing updated field values.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="AccessControlDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no access control is found for the given ID.</exception>
	/// <exception cref="DuplicateRecordException">Thrown when another access control with the same name already exists.</exception>
	public async Task<AccessControlDto> UpdateAsync(int accessControlId, AccessControlDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(AccessControlDto));

		if (accessControlId != modelDto.AccessId)
			throw new BadRequestException(accessControlId.ToString(), nameof(AccessControlDto));

		AccessControl existingEntity = await _repository.AccessControls
				.FirstOrDefaultAsync(x => x.AccessId == accessControlId, trackChanges: false, cancellationToken)
				?? throw new NotFoundException("AccessControl", "AccessId", accessControlId.ToString());

		bool duplicateExists = await _repository.AccessControls.ExistsAsync(
				x => x.AccessName.Trim().ToLower() == modelDto.AccessName.Trim().ToLower()
					&& x.AccessId != accessControlId,
				cancellationToken: cancellationToken);

		if (duplicateExists)
			throw new DuplicateRecordException("AccessControl", "AccessName");
		AccessControl updatedEntity = MyMapper.MergeChangedValues<AccessControl, AccessControlDto>(existingEntity, modelDto);
		_repository.AccessControls.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("AccessControl", "AccessId", accessControlId.ToString());
		_logger.LogInformation(
				"AccessControl updated. ID: {AccessId}, Name: {AccessName}, Time: {Time}",
				updatedEntity.AccessId,
				updatedEntity.AccessName,
				DateTime.UtcNow);

		return MyMapper.JsonClone<AccessControl, AccessControlDto>(updatedEntity);
	}

	/// <summary>
	/// Deletes an access control record identified by the given ID.
	/// Validates that the ID is positive and that the record exists before deletion.
	/// </summary>
	/// <param name="accessControlId">The ID of the access control to delete.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <exception cref="IdParametersBadRequestException">Thrown when <paramref name="accessControlId"/> is zero or negative.</exception>
	/// <exception cref="NotFoundException">Thrown when no access control record is found for the given ID.</exception>
	public async Task<int> DeleteAsync(int accessControlId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (accessControlId <= 0)
			throw new BadRequestException(accessControlId.ToString(), nameof(AccessControlDto));

		AccessControl accessControlEntity = await _repository.AccessControls.FirstOrDefaultAsync(x => x.AccessId == accessControlId, trackChanges, cancellationToken)
				?? throw new NotFoundException("AccessControl", "AccessId", accessControlId.ToString());

		await _repository.AccessControls.DeleteAsync(x => x.AccessId == accessControlId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("AccessControl", "AccessId", accessControlId.ToString());
		_logger.LogWarning(
				"AccessControl deleted. ID: {AccessId}, Name: {AccessName}, Time: {Time}",
				accessControlEntity.AccessId,
				accessControlEntity.AccessName,
				DateTime.UtcNow);
		return affected;
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all access controls with module and parent menu information.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{AccessControlDto}"/> containing the paged access control summary data.</returns>
	public async Task<GridEntity<AccessControlDto>> SummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
	{
		string query = "SELECT AccessId, AccessName FROM AccessControl WHERE IsActive = 1";
		string orderBy = "AccessName ASC";

		var gridEntity = await _repository.AccessControls.AdoGridDataAsync<AccessControlDto>(query, options, orderBy, "", cancellationToken);
		return gridEntity;
	}

	/// <summary>
	/// Retrieves a single access control record by its ID.
	/// </summary>
	/// <param name="accessId">The ID of the access control to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="AccessControlDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no access control is found for the given ID.</exception>
	public async Task<AccessControlDto> AccessControlAsync(int accessId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (accessId <= 0)
		{
			_logger.LogWarning("AccessControlAsync called with invalid id: {AccessId}", accessId);
			throw new BadRequestException("Invalid request!");
		}

		AccessControl accessControl = await _repository.AccessControls.FirstOrDefaultAsync(x => x.AccessId == accessId, trackChanges, cancellationToken);
		if (accessControl == null)
		{
			_logger.LogWarning("Access control not found with ID: {AccessId}", accessId);
			throw new NotFoundException("AccessControl", "AccessId", accessId.ToString());
		}

		_logger.LogInformation("Access control fetched successfully. ID: {AccessId}, Name: {AccessName}, Time: {Time}", accessControl.AccessId, accessControl.AccessName, DateTime.UtcNow);
		return MyMapper.JsonClone<AccessControl, AccessControlDto>(accessControl);
	}

	/// <summary>
	/// Retrieves a lightweight list of all access controls suitable for use in dropdown lists.
	/// Returns only the access control ID and name, ordered by name.
	/// </summary>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of <see cref="AccessControlDto"/> for dropdown binding.</returns>
	public async Task<IEnumerable<AccessControlDto>> AccessControlsForDDLAsync(CancellationToken cancellationToken = default)
	{
		IEnumerable<AccessControl> accessControls = await _repository.AccessControls.ListWithSelectAsync(
				x => new AccessControl
				{
					AccessId = x.AccessId,
					AccessName = x.AccessName
				},
				orderBy: x => x.AccessName,
				trackChanges: false,
				cancellationToken: cancellationToken);

		if (!accessControls.Any())
		{
			_logger.LogWarning("No access controls found for dropdown list");
			return Enumerable.Empty<AccessControlDto>();
		}
		_logger.LogInformation("Access controls fetched successfully for dropdown list");
		return MyMapper.JsonCloneIEnumerableToIEnumerable<AccessControl, AccessControlDto>(accessControls);
	}

}
