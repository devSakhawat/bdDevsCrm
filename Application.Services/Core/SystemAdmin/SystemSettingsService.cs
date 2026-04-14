using Application.Services.Mappings;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.Core.SystemAdmin;

/// <summary>
/// System settings service implementing business logic for system configuration and assembly information management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class SystemSettingsService : ISystemSettingsService
{
  private readonly IRepositoryManager _repository;
  private readonly ILogger<SystemSettingsService> _logger;
  private readonly IConfiguration _configuration;

  /// <summary>
  /// Initializes a new instance of <see cref="SystemSettingsService"/> with required dependencies.
  /// </summary>
  /// <param name="repository">The repository manager for data access operations.</param>
  /// <param name="logger">The logger for capturing service-level events.</param>
  /// <param name="configuration">The application configuration accessor.</param>
  public SystemSettingsService(IRepositoryManager repository, ILogger<SystemSettingsService> logger, IConfiguration configuration)
  {
    _repository = repository;
    _logger = logger;
    _configuration = configuration;
  }

  /// <summary>
  /// Retrieves system settings data by the specified company ID.
  /// </summary>
  public async Task<SystemSettingsDto> SystemSettingsByCompanyIdAsync(int companyId, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (companyId <= 0)
    {
      _logger.LogWarning("SystemSettingsByCompanyIdAsync called with invalid companyId: {CompanyId}", companyId);
      throw new BadRequestException("Invalid request!");
    }

    _logger.LogInformation("Fetching system settings for company ID: {CompanyId}, Time: {Time}", companyId, DateTime.UtcNow);

    SystemSettings systemSettings = await _repository.SystemSettings
            .FirstOrDefaultAsync(x => x.CompanyId == companyId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Data not found!");

    SystemSettingsDto systemSettingsDto = MyMapper.JsonClone<SystemSettings, SystemSettingsDto>(systemSettings);

    _logger.LogInformation(
            "System settings fetched successfully. CompanyId: {CompanyId}, SettingsId: {SettingsId}, Time: {Time}",
            companyId,
            systemSettingsDto.SettingsId,
            DateTime.UtcNow);

    return systemSettingsDto;
  }

  /// <summary>
  /// Retrieves assembly information for the application.
  /// </summary>
  public async Task<AssemblyInfoDto> AssemblyInfoAsync(bool trackChanges, CancellationToken cancellationToken = default)
  {
    _logger.LogInformation("Fetching assembly information. Time: {Time}", DateTime.UtcNow);

    //AssemblyInfo assemblyInfo = await _repository.SystemSettings.FirstOrDefaultAsync(x => x.AssemblyInfoId == 1, trackChanges, cancellationToken);
    AssemblyInfo assemblyInfo = await _repository.SystemSettings.AssemblyInfoResultAsync();

    AssemblyInfoDto assemblyInfoDto;

    if (assemblyInfo is null)
    {
      _logger.LogWarning("No assembly information found in database. Returning default values. Time: {Time}", DateTime.UtcNow);

      assemblyInfoDto = new AssemblyInfoDto
      {
        AssemblyInfoId = 1,
        AssemblyTitle = "bdDevsCRM",
        AssemblyCompany = "bdDevs Software Solution Ltd.",
        AssemblyProduct = "bdDevsCRM",
        AssemblyCopyright = "Copyright © bdDev 2025. All right reserved",
        AssemblyVersion = "Version: 1.0.0",
        PoweredBy = "bdDevs",
        PoweredByUrl = "http://www.bdDevs.com",
        ProductStyleSheet = "~/Assets/Css/Common_demo.css"
      };
    }
    else
    {
      assemblyInfoDto = MyMapper.JsonClone<AssemblyInfo, AssemblyInfoDto>(assemblyInfo);

      _logger.LogInformation(
              "Assembly information fetched successfully. Title: {AssemblyTitle}, Version: {AssemblyVersion}, Time: {Time}",
              assemblyInfoDto.AssemblyTitle,
              assemblyInfoDto.AssemblyVersion,
              DateTime.UtcNow);
    }

    return assemblyInfoDto;
  }

  /// <summary>
  /// Updates system settings with the provided data.
  /// </summary>
  public async Task<SystemSettingsDto> UpdateSystemSettingsAsync(SystemSettingsDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (modelDto is null)
      throw new BadRequestException(nameof(SystemSettingsDto));

    _logger.LogInformation("Updating system settings. SettingsId: {SettingsId}, Time: {Time}",
            modelDto.SettingsId, DateTime.UtcNow);

    SystemSettings existingEntity = await _repository.SystemSettings.FirstOrDefaultAsync(x => x.SettingsId == modelDto.SettingsId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("Data not found!");
    SystemSettings updatedEntity = MyMapper.MergeChangedValues<SystemSettings, SystemSettingsDto>(existingEntity, modelDto);
    _repository.SystemSettings.UpdateByState(updatedEntity);

    int affected = await _repository.SaveChangesAsync(cancellationToken);
    if (affected <= 0)
      throw new NotFoundException("Data not found!");

    _logger.LogInformation(
            "System settings updated successfully. ID: {SettingsId}, Time: {Time}",
            updatedEntity.SettingsId,
            updatedEntity.SettingsId,
            DateTime.UtcNow);

    return MyMapper.JsonClone<SystemSettings, SystemSettingsDto>(updatedEntity);
  }

  /// <summary>
  /// Retrieves all system settings records from the database.
  /// </summary>
  public async Task<IEnumerable<SystemSettingsDto>> SystemSettingsAsync(bool trackChanges, CancellationToken cancellationToken = default)
  {
    _logger.LogInformation("Fetching all system settings. Time: {Time}", DateTime.UtcNow);

    IEnumerable<SystemSettings> systemSettings = await _repository.SystemSettings.SystemSettingsAsync(trackChanges, cancellationToken);

    if (!systemSettings.Any())
    {
      _logger.LogWarning("No system settings found. Time: {Time}", DateTime.UtcNow);
      return Enumerable.Empty<SystemSettingsDto>();
    }

    IEnumerable<SystemSettingsDto> systemSettingsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<SystemSettings, SystemSettingsDto>(systemSettings);

    _logger.LogInformation(
            "System settings fetched successfully. Count: {Count}, Time: {Time}",
            systemSettingsDto.Count(),
            DateTime.UtcNow);

    return systemSettingsDto;
  }
}



//using Domain.Entities.Entities.System;

//using Domain.Contracts.Services.Core.SystemAdmin;
//using Domain.Contracts.Services.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using Application.Services.Mappings;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;

//namespace Domain.Contracts.Services.Core.SystemAdmin;

//internal sealed class SystemSettingsService : ISystemSettingsService
//{
//  private readonly IRepositoryManager _repository;
//  private readonly ILogger<SystemSettingsService> _logger;
//  private readonly IConfiguration _configuration;

//  public SystemSettingsService(IRepositoryManager repository, ILogger<SystemSettingsService> logger, IConfiguration configuration)
//  {
//    _repository = repository;
//    _logger = logger;
//    _configuration = configuration;
//  }

//  public async Task<SystemSettings?> SystemSettingsDataByCompanyId(int companyId)
//  {
//    return await _repository.SystemSettings.SystemSettingsByCompanyIdAsync(companyId);
//  }

//  public async Task<AssemblyInfoDto> AssemblyInfoResult()
//  {
//    var assemblyInfo = await _repository.SystemSettings.AssemblyInfoResultAsync();

//    var assemblyInfoDto = new AssemblyInfoDto();
//    if (assemblyInfo == null)
//    {
//      assemblyInfoDto.AssemblyInfoId = 1;
//      assemblyInfoDto.AssemblyTitle = "bdDevsCRM";
//      assemblyInfoDto.AssemblyCompany = "bdDevs Software Solution Ltd.";
//      assemblyInfoDto.AssemblyProduct = "bdDevsCRM";
//      assemblyInfoDto.AssemblyCopyright = "Copyright © bdDev 2025. All right reserved";
//      assemblyInfoDto.AssemblyVersion = "Version: 1.0.0";
//      assemblyInfoDto.AssemblyVersion = "Version: 1.0.0";
//      assemblyInfoDto.PoweredBy = "bdDevs";
//      assemblyInfoDto.PoweredByUrl = "http://www.bdDevs.com";
//      assemblyInfoDto.ProductStyleSheet = "~/Assets/Css/Common_demo.css";
//    }
//    else
//    {
//      assemblyInfoDto = MyMapper.JsonClone<AssemblyInfo, AssemblyInfoDto>(assemblyInfo);
//    }

//    return assemblyInfoDto;
//  }

//  //public string SaveSystemSettings(SystemSettings objSystemSettings)
//  //{
//  //  return _systemSettingsDataService.SaveSystemSettings(objSystemSettings);
//  //}

//  //public DataTable SystemSettingsData()
//  //{
//  //  return _systemSettingsDataService.SystemSettingsData();
//  //}
//  //public bool CheckPaddingExistOrNot(int hrRecordId)
//  //{
//  //  var res = false;
//  //  var objSystemSettings = _systemSettingsDataService.CheckPaddingExistOrNot(hrRecordId);
//  //  if (objSystemSettings != null)
//  //  {
//  //    if (objSystemSettings.IsPaddingApplicable == 1)
//  //    {
//  //      res = true;
//  //    }
//  //  }
//  //  return res;
//  //}

//  //public bool CheckPaddingExistOrNotByCompanyId(int companyId)
//  //{
//  //  var res = false;
//  //  var objSystemSettings = _systemSettingsDataService.CheckPaddingExistOrNotByCompanyId(companyId);
//  //  if (objSystemSettings != null)
//  //  {
//  //    if (objSystemSettings.IsPaddingApplicable == 1)
//  //    {
//  //      res = true;
//  //    }
//  //  }
//  //  return res;
//  //}

//  //public bool CheckPadding()
//  //{
//  //  var res = false;
//  //  var objSystemSettings = _systemSettingsDataService.CheckPadding();
//  //  if (objSystemSettings != null)
//  //  {
//  //    if (objSystemSettings.IsPaddingApplicable == 1)
//  //    {
//  //      res = true;
//  //    }
//  //  }
//  //  return res;
//  //}

//  //public SystemSettings SystemSettingsDataByUserId(int userId)
//  //{
//  //  return _systemSettingsDataService.SystemSettingsDataByUserId(userId);
//  //}

//  //public SystemSettings SystemSettingsDataByHrRecordId(int hrRecordId)
//  //{
//  //  return _systemSettingsDataService.SystemSettingsDataByHrRecordId(hrRecordId);
//  //}

//  //public SystemSettings SystemSettingsDataByEmployeeId(string employeeId)
//  //{
//  //  return _systemSettingsDataService.SystemSettingsDataByEmployeeId(employeeId);
//  //}

//  //public SystemSettings SystemSettingsDataByCostCentreSalaryMappingCompanyId(int costCentreId)
//  //{
//  //  return _systemSettingsDataService.SystemSettingsDataByCostCentreSalaryMappingCompanyId(costCentreId);
//  //}
//}

