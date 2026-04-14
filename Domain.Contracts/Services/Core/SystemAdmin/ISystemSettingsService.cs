using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

namespace Domain.Contracts.Services.Core.SystemAdmin;

/// <summary>
/// Service contract for system settings and assembly information management operations.
/// Defines methods for retrieving system configuration and application metadata.
/// </summary>
public interface ISystemSettingsService
{
	/// <summary>
	/// Retrieves system settings data by the specified company ID.
	/// </summary>
	/// <param name="companyId">The ID of the company whose settings are to be retrieved.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="SystemSettingsDto"/> matching the specified company ID.</returns>
	/// <exception cref="IdParametersBadRequestException">Thrown when <paramref name="companyId"/> is zero or negative.</exception>
	/// <exception cref="NotFoundException">Thrown when no system settings are found for the given company ID.</exception>
	Task<SystemSettingsDto> SystemSettingsByCompanyIdAsync(int companyId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves assembly information for the application.
	/// Returns default assembly info if no record exists in the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="AssemblyInfoDto"/> containing application metadata.</returns>
	Task<AssemblyInfoDto> AssemblyInfoAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates assembly information with the provided data.
	/// </summary>
	/// <param name="modelDto">The DTO containing updated assembly information.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="AssemblyInfoDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	Task<SystemSettingsDto> UpdateSystemSettingsAsync(SystemSettingsDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all system settings records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="SystemSettingsDto"/> records.</returns>
	Task<IEnumerable<SystemSettingsDto>> SystemSettingsAsync(bool trackChanges, CancellationToken cancellationToken = default);
}


//using bdDevCRM.Entities.Entities;
//using Domain.Entities.Entities.System;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

//namespace bdDevCRM.ServiceContract.Core.SystemAdmin;

//public interface ISystemSettingsService
//{
//  Task<SystemSettings> SystemSettingsDataByCompanyId(int companyId);

//  Task<AssemblyInfoDto> AssemblyInfoResult();

//  //string SaveSystemSettings(SystemSettings objSystemSettings);

//  //System.Data.DataTable SystemSettingsData();



//  //bool CheckPaddingExistOrNot(int hrRecordId);

//  //bool CheckPaddingExistOrNotByCompanyId(int companyId);

//  //bool CheckPadding();

//  //SystemSettings SystemSettingsDataByUserId(int userId);

//  //SystemSettings SystemSettingsDataByHrRecordId(int hrRecordId);

//  //SystemSettings SystemSettingsDataByEmployeeId(string employeeId);

//  //SystemSettings SystemSettingsDataByCostCentreSalaryMappingCompanyId(int costCentreId);
//}
