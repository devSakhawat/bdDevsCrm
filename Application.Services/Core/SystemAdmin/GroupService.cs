using Domain.Entities.Entities.System;
using Domain.Contracts.Services.Core.SystemAdmin;
using bdDevCRM.s.Core.SystemAdmin;
using Application.Services.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevCRM.Shared.Exceptions;
using Application.Shared.Grid;
using bdDevCRM.Utilities.OthersLibrary;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.Core.SystemAdmin;

/// <summary>
/// Group service implementing business logic for group management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class GroupService : IGroupService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<GroupService> _logger;
	private readonly IConfiguration _configuration;

	public GroupService(IRepositoryManager repository, ILogger<GroupService> logger, IConfiguration configuration)
	{
		_repository = repository;
		_logger = logger;
		_configuration = configuration;
	}

	/// <summary>
	/// Retrieves paginated summary grid of groups asynchronously.
	/// </summary>
	public async Task<GridEntity<GroupSummaryDto>> GroupSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching group summary grid");

		string query = @"SELECT g.GroupId, g.ModuleId, g.GroupName, g.IsDefault, g.IsActive, m.ModuleName,
            (SELECT COUNT(*) FROM GroupMember gm WHERE gm.GroupId = g.GroupId) as MemberCount
            FROM Groups g
            LEFT JOIN Module m ON m.ModuleId = g.ModuleId
            WHERE g.IsActive = 1";
		string orderBy = "g.GroupName ASC";

		var gridEntity = await _repository.Groups.AdoGridDataAsync<GroupSummaryDto>(query, options, orderBy, "", cancellationToken);
		return gridEntity;
	}

	/// <summary>
	/// Retrieves group permissions by group ID asynchronously.
	/// </summary>
	public async Task<IEnumerable<GroupPermissionDto>> GroupPermissionsByGroupIdAsync(int groupId, CancellationToken cancellationToken = default)
	{
		if (groupId <= 0)
		{
			_logger.LogWarning("GroupPermissionsByGroupIdAsync called with invalid groupId: {GroupId}", groupId);
			throw new BadRequestException("Invalid request!");
		}

		_logger.LogInformation("Fetching permissions for group ID: {GroupId}", groupId);

		var permissions = await _repository.Groups.GroupPermissionsByGroupIdAsync(groupId, cancellationToken);

		if (!permissions.Any())
		{
			_logger.LogWarning("No permissions found for group ID: {GroupId}", groupId);
			return Enumerable.Empty<GroupPermissionDto>();
		}

		var permissionDtos = MyMapper.JsonCloneIEnumerableToList<GroupPermission, GroupPermissionDto>(permissions);
		return permissionDtos;
	}

	// (For MenuManagement Only) Check menu permission by path and user
	public async Task<MenuDto> CheckMenuPermissionAsync(string rawPath, UsersDto objUser, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrWhiteSpace(rawPath)) throw new BadRequestException("Invalid URL.");
		Users userEntity = MyMapper.JsonClone<UsersDto, Users>(objUser);
		Menu data = await _repository.Groups.CheckMenuPermission(rawPath, userEntity, cancellationToken);
		if (data == null)
			return new MenuDto();
		else
			return MyMapper.JsonClone<Menu, MenuDto>(data);
	}

	/// <summary>
	/// Retrieves all access controls asynchronously.
	/// </summary>
	public async Task<IEnumerable<AccessControlDto>> AccessControlsAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all access controls");

		var accessControls = await _repository.Groups.AccessControlsAsync(cancellationToken);
		if (!accessControls.Any())
		{
			_logger.LogWarning("No access controls found");
			return Enumerable.Empty<AccessControlDto>();
		}

		var accessControlDtos = MyMapper.JsonCloneIEnumerableToList<AccessControl, AccessControlDto>(accessControls);
		return accessControlDtos;
	}

	/// <summary>
	/// Creates a new group asynchronously.
	/// </summary>
	public async Task<GroupDto> CreateAsync(GroupDto modelDto, CancellationToken cancellationToken = default)
	{
		if (modelDto == null)
			throw new BadRequestException(nameof(GroupDto));

		_logger.LogInformation("Creating new group: {GroupName}", modelDto.GroupName);

		// Check for duplicate group name
		bool groupExists = await _repository.Groups.ExistsAsync(
				m => m.GroupName.Trim().ToLower() == modelDto.GroupName.Trim().ToLower(), cancellationToken: cancellationToken);

		if (groupExists)
			throw new DuplicateRecordException("Group", "GroupName");

		// Handle default group logic
		if (modelDto.IsDefault == 1)
		{
			string updateIsDefaultZero = "UPDATE Groups SET IsDefault = 0";
			string result = await _repository.Groups.EfCoreExecuteNonQueryAsync(updateIsDefaultZero, cancellationToken);
			if (result != "Success")
				throw new Exception("Error updating IsDefault flag");
		}

		try
		{
			await _repository.GroupPermissiones.TransactionBeginAsync();

			// Create group
			Groups entity = MyMapper.JsonClone<GroupDto, Groups>(modelDto);
			int groupId = await _repository.Groups.CreateAndIdAsync(entity);
			modelDto.GroupId = groupId;

			// Delete any existing permissions for this group
			await DeleteExistingPermissionsForGroupAsync(groupId);

			// Insert permissions
			await InsertPermissionsIfNotEmptyAsync(groupId, modelDto.ModuleList, "Module", cancellationToken);
			await InsertPermissionsIfNotEmptyAsync(groupId, modelDto.MenuList, "Menu", cancellationToken);
			await InsertPermissionsIfNotEmptyAsync(groupId, modelDto.AccessList, "AccessControl", cancellationToken);
			await InsertPermissionsIfNotEmptyAsync(groupId, modelDto.StatusList, "Status", cancellationToken);
			await InsertPermissionsIfNotEmptyAsync(groupId, modelDto.ActionList, "Action", cancellationToken);
			await InsertPermissionsIfNotEmptyAsync(groupId, modelDto.ReportList, "Report", cancellationToken);

			await _repository.GroupPermissiones.TransactionCommitAsync();

			_logger.LogInformation("Group created successfully with ID: {GroupId}", groupId);
			return modelDto;
		}
		catch (Exception ex)
		{
			await _repository.GroupPermissiones.TransactionRollbackAsync();
			_logger.LogError(ex, "Error creating group: {GroupName}", modelDto.GroupName);
			throw;
		}
		finally
		{
			await _repository.GroupPermissiones.TransactionDisposeAsync();
		}
	}

	/// <summary>
	/// Updates an existing group asynchronously.
	/// </summary>
	public async Task<GroupDto> UpdateAsync(int key, GroupDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto == null)
			throw new BadRequestException(nameof(GroupDto));

		if (key != modelDto.GroupId)
			throw new BadRequestException(key.ToString(), nameof(GroupDto));

		_logger.LogInformation("Updating group with ID: {GroupId}", key);

		// Check if group exists
		var existingGroup = await _repository.Groups.ByIdAsync(
				g => g.GroupId == key, trackChanges: false, cancellationToken);

		if (existingGroup == null)
			throw new NotFoundException("Group", "GroupId", key.ToString());

		// Handle default group logic
		if (modelDto.IsDefault == 1)
		{
			string updateIsDefaultZero = "UPDATE Groups SET IsDefault = 0 WHERE GroupId <> " + key;
			string result = await _repository.Groups.EfCoreExecuteNonQueryAsync(updateIsDefaultZero, cancellationToken);
			if (result != "Success")
				throw new Exception("Error updating IsDefault flag");
		}

		try
		{
			await _repository.GroupPermissiones.TransactionBeginAsync(cancellationToken);

			// Update group
			Groups entity = MyMapper.JsonClone<GroupDto, Groups>(modelDto);
			_repository.Groups.UpdateByState(entity);

			// Delete existing permissions
			await DeleteExistingPermissionsForGroupAsync(key);

			// Insert new permissions
			await InsertPermissionsIfNotEmptyAsync(key, modelDto.ModuleList, "Module", cancellationToken);
			await InsertPermissionsIfNotEmptyAsync(key, modelDto.MenuList, "Menu", cancellationToken);
			await InsertPermissionsIfNotEmptyAsync(key, modelDto.AccessList, "AccessControl", cancellationToken);
			await InsertPermissionsIfNotEmptyAsync(key, modelDto.StatusList, "Status", cancellationToken);
			await InsertPermissionsIfNotEmptyAsync(key, modelDto.ActionList, "Action", cancellationToken);
			await InsertPermissionsIfNotEmptyAsync(key, modelDto.ReportList, "Report", cancellationToken);

			await _repository.GroupPermissiones.TransactionCommitAsync(cancellationToken);
			await _repository.SaveAsync(cancellationToken);

			_logger.LogInformation("Group updated successfully: {GroupId}", key);
			return modelDto;
		}
		catch (Exception ex)
		{
			await _repository.GroupPermissiones.TransactionRollbackAsync(cancellationToken);
			_logger.LogError(ex, "Error updating group: {GroupId}", key);
			throw;
		}
		finally
		{
			await _repository.GroupPermissiones.TransactionDisposeAsync();
		}
	}

	/// <summary>
	/// Deletes a group by ID asynchronously.
	/// </summary>
	public async Task DeleteAsync(int key, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (key <= 0)
			throw new BadRequestException("Invalid request!");

		_logger.LogInformation("Deleting group with ID: {GroupId}", key);

		var group = await _repository.Groups.ByIdAsync(
				g => g.GroupId == key, trackChanges: trackChanges, cancellationToken: cancellationToken);

		if (group == null)
			throw new NotFoundException("Group", "GroupId", key.ToString());

		try
		{
			await _repository.GroupPermissiones.TransactionBeginAsync(cancellationToken);

			// Delete permissions first
			await DeleteExistingPermissionsForGroupAsync(key, cancellationToken);

			// Delete group
			await _repository.Groups.DeleteAsync(g => g.GroupId == key, trackChanges: false, cancellationToken);
			await _repository.SaveAsync(cancellationToken);

			await _repository.GroupPermissiones.TransactionCommitAsync(cancellationToken);

			_logger.LogInformation("Group deleted successfully: {GroupId}", key);
		}
		catch (Exception ex)
		{
			await _repository.GroupPermissiones.TransactionRollbackAsync(cancellationToken);
			_logger.LogError(ex, "Error deleting group: {GroupId}", key);
			throw;
		}
		finally
		{
			await _repository.GroupPermissiones.TransactionDisposeAsync();
		}
	}


	/// <summary>
	/// Retrieves a single Group record by its ID.
	/// </summary>
	public async Task<GroupDto> GroupAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		Groups group = await _repository.Groups.FirstOrDefaultAsync(x => x.GroupId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("Group", "GroupId", id.ToString());

		_logger.LogInformation("Group fetched successfully. ID: {GroupId}, Name: {GroupName}, Time: {Time}", group.GroupId, group.GroupName, DateTime.UtcNow);
		return MyMapper.JsonClone<Groups, GroupDto>(group);
	}


	/// <summary>
	/// Retrieves groups for dropdown list asynchronously.
	/// </summary>
	public async Task<IEnumerable<GroupDDLDto>> GroupsForDDLAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching groups for dropdown list");

		var groups = await _repository.Groups.ListWithSelectAsync(
				x => new Groups
				{
					GroupId = x.GroupId,
					GroupName = x.GroupName
				},
				orderBy: x => x.GroupName,
				trackChanges: false,
				cancellationToken: cancellationToken
		);

		if (!groups.Any())
			return new List<GroupDDLDto>();

		var groupsForDDLDto = MyMapper.JsonCloneIEnumerableToList<Groups, GroupDDLDto>(groups);
		return groupsForDDLDto;
	}

	/// <summary>
	/// Helper method to delete existing permissions for a group.
	/// </summary>
	private async Task DeleteExistingPermissionsForGroupAsync(int groupId, CancellationToken cancellationToken = default)
	{
		string deleteQuery = $"DELETE FROM GroupPermission WHERE GroupId = {groupId}";
		await _repository.GroupPermissiones.EfCoreExecuteNonQueryAsync(deleteQuery, cancellationToken);
	}

	/// <summary>
	/// Helper method to insert permissions if the list is not empty.
	/// </summary>
	private async Task InsertPermissionsIfNotEmptyAsync(int groupId, List<GroupPermissionDto> permissionList, string permissionTableName, CancellationToken cancellationToken = default)
	{
		if (permissionList == null || !permissionList.Any())
			return;

		foreach (var permissionId in permissionList)
		{
			string insertQuery = $@"INSERT INTO GroupPermission (GroupId, PermissionTableName, ReferenceID) 
                VALUES ({groupId}, '{permissionTableName}', {permissionId})";
			await _repository.GroupPermissiones.EfCoreExecuteNonQueryAsync(insertQuery, cancellationToken);
		}

		await Task.CompletedTask;
	}



}
