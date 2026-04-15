using Application.Services.Mappings;
using Application.Services.Mappings;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevsCrm.Shared.Settings;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Services.Core.SystemAdmin;

/// <summary>
/// Company service implementing business logic for company management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CompanyService : ICompanyService
{
  private readonly IRepositoryManager _repository;
  private readonly ILogger<CompanyService> _logger;
  private readonly IConfiguration _configuration;
  private readonly AppSettings _appSettings;

  public CompanyService(IRepositoryManager repository, ILogger<CompanyService> logger, IConfiguration configuration, IOptions<AppSettings> appSettings)
  {
    _repository = repository;
    _logger = logger;
    _configuration = configuration;
    _appSettings = appSettings.Value;
  }

  /// <summary>
  /// Creates a new company record after validating for null input and duplicate company name.
  /// </summary>
  /// <param name="entityForCreate">The DTO containing data for the new company.</param>
  /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
  /// <returns>The created <see cref="CompanyDto"/> with the newly assigned ID.</returns>
  /// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
  /// <exception cref="DuplicateRecordException">Thrown when a company with the same name already exists.</exception>
  public async Task<CompanyDto> CreateAsync(CompanyDto entityForCreate, CancellationToken cancellationToken = default)
  {
    if (entityForCreate is null)
      throw new BadRequestException(nameof(CompanyDto));

    bool companyExists = await _repository.Companies.ExistsAsync(
        c => c.CompanyName.Trim().ToLower() == entityForCreate.CompanyName.Trim().ToLower(),
        cancellationToken: cancellationToken);

    if (companyExists)
      throw new ConflictException("Duplicate data found!");

    Company companyEntity = MyMapper.JsonClone<CompanyDto, Company>(entityForCreate);

    await _repository.Companies.CreateAsync(companyEntity, cancellationToken);
    int affected = await _repository.SaveChangesAsync(cancellationToken);

    if (affected <= 0)
      throw new InvalidOperationException("Company could not be saved to the database.");
    _logger.LogInformation(
        "Company created successfully. ID: {CompanyId}, Name: {CompanyName}, Time: {Time}",
        companyEntity.CompanyId,
        companyEntity.CompanyName,
        DateTime.UtcNow);

    return MyMapper.JsonClone<Company, CompanyDto>(companyEntity);
  }

  /// <summary>
  /// Updates an existing company record by merging only the changed values from the provided DTO.
  /// Validates ID consistency, null input, record existence, and duplicate name constraints.
  /// </summary>
  /// <param name="companyId">The ID of the company to update.</param>
  /// <param name="modelDto">The DTO containing updated field values.</param>
  /// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
  /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
  /// <returns>The updated <see cref="CompanyDto"/> reflecting the saved state.</returns>
  /// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
  /// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
  /// <exception cref="NotFoundException">Thrown when no company is found for the given ID.</exception>
  /// <exception cref="DuplicateRecordException">Thrown when another company with the same name already exists.</exception>
  public async Task<CompanyDto> UpdateAsync(int companyId, CompanyDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (modelDto is null)
      throw new BadRequestException(nameof(CompanyDto));

    if (companyId != modelDto.CompanyId)
      throw new BadRequestException(companyId.ToString(), nameof(CompanyDto));

    Company existingEntity = await _repository.Companies
        .FirstOrDefaultAsync(x => x.CompanyId == companyId, trackChanges: false, cancellationToken)
        ?? throw new NotFoundException("Data not found!");

    bool duplicateExists = await _repository.Companies.ExistsAsync(
        x => x.CompanyName.Trim().ToLower() == modelDto.CompanyName.Trim().ToLower()
          && x.CompanyId != companyId,
        cancellationToken: cancellationToken);

    if (duplicateExists)
      throw new ConflictException("Duplicate data found!");
    Company updatedEntity = MyMapper.MergeChangedValues<Company, CompanyDto>(existingEntity, modelDto);
    _repository.Companies.UpdateByState(updatedEntity);

    int affected = await _repository.SaveChangesAsync(cancellationToken);
    if (affected <= 0)
      throw new NotFoundException("Data not found!");
    _logger.LogInformation(
        "Company updated. ID: {CompanyId}, Name: {CompanyName}, Time: {Time}",
        updatedEntity.CompanyId,
        updatedEntity.CompanyName,
        DateTime.UtcNow);

    return MyMapper.JsonClone<Company, CompanyDto>(updatedEntity);
  }

  /// <summary>
  /// Deletes a company record identified by the given ID.
  /// Validates that the ID is positive and that the record exists before deletion.
  /// </summary>
  /// <param name="companyId">The ID of the company to delete.</param>
  /// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
  /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
  /// <exception cref="IdParametersBadRequestException">Thrown when <paramref name="accessControlId"/> is zero or negative.</exception>
  /// <exception cref="NotFoundException">Thrown when no access control record is found for the given ID.</exception>
  public async Task<int> DeleteAsync(int companyId, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (companyId <= 0)
      throw new BadRequestException(companyId.ToString(), nameof(CompanyDto));

    Company companyEntity = await _repository.Companies.FirstOrDefaultAsync(x => x.CompanyId == companyId, trackChanges, cancellationToken)
        ?? throw new NotFoundException("Data not found!");

    await _repository.Companies.DeleteAsync(x => x.CompanyId == companyId, trackChanges, cancellationToken);
    int affected = await _repository.SaveChangesAsync(cancellationToken);
    if (affected <= 0)
      throw new NotFoundException("Data not found!");
    _logger.LogWarning(
        "Company deleted. ID: {CompanyId}, Name: {CompanyName}, Time: {Time}",
        companyEntity.CompanyId,
        companyEntity.CompanyName,
        DateTime.UtcNow);
    return affected;
  }

  /// <summary>
  /// Retrieves all company records from the database.
  /// </summary>
  /// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
  /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
  /// <returns>A collection of all <see cref="MenuDto"/> records.</returns>
  public async Task<IEnumerable<CompanyDto>> CompaniesAsync(bool trackChanges, CancellationToken cancellationToken = default)
  {
    IEnumerable<Company> companies = await _repository.Companies.CompaniesAsync(trackChanges, cancellationToken);
    if (!companies.Any())
    {
      _logger.LogWarning("No companies found");
      return Enumerable.Empty<CompanyDto>();
    }
    _logger.LogInformation("Companies fetched successfully");
    return MyMapper.JsonCloneIEnumerableToIEnumerable<Company, CompanyDto>(companies);
  }

  /// <summary>
  /// Retrieves a single access control record by its ID.
  /// </summary>
  /// <param name="accessId">The ID of the access control to retrieve.</param>
  /// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
  /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
  /// <returns>The <see cref="AccessControlDto"/> matching the specified ID.</returns>
  /// <exception cref="NotFoundException">Thrown when no access control is found for the given ID.</exception>
  public async Task<CompanyDto> CompanyAsync(int companyId, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (companyId <= 0)
    {
      _logger.LogWarning("CompanyAsync called with invalid id: {CompanyId}", companyId);
      throw new BadRequestException("Invalid request!");
    }

    Company company = await _repository.Companies.FirstOrDefaultAsync(x => x.CompanyId == companyId, trackChanges, cancellationToken);
    if (company == null)
    {
      _logger.LogWarning("Company not found with ID: {CompanyId}", companyId);
      throw new NotFoundException("Data not found!");
    }

    _logger.LogInformation("Company fetched successfully. ID: {CompanyId}, Name: {CompanyName}, Time: {Time}", company.CompanyId, company.CompanyName, DateTime.UtcNow);
    return MyMapper.JsonClone<Company, CompanyDto>(company);
  }

  /// <summary>
  /// Retrieves companies by a collection of IDs asynchronously.
  /// </summary>
  public async Task<IEnumerable<CompanyDto>> CompaniesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (!ids.Any())
    {
      _logger.LogError("ByIdsAsync called with empty ID collection");
      throw new BadRequestException("Invalid request!");
    }

    IEnumerable<Company> companyEntities = await _repository.Companies.ByIdsAsync(ids, trackChanges, cancellationToken);
    if (!companyEntities.Any())
    {
      _logger.LogWarning("No companies found for company IDs: {CompanyIds}", string.Join(",", ids));
      return Enumerable.Empty<CompanyDto>();
    }
    _logger.LogInformation("Fetching companies by IDs: {CompanyIds}", string.Join(",", ids));

    if (ids.Count() != companyEntities.Count())
      throw new BadRequestException("Invalid request!");

    var companiesToReturn = MyMapper.JsonCloneIEnumerableToIEnumerable<Company, CompanyDto>(companyEntities);
    return companiesToReturn;
  }

  /// <summary>
  /// Retrieves mother company for the current user asynchronously.
  /// </summary>
  public async Task<IEnumerable<CompanyDto>> MotherCompanyAsync(int companyId, UsersDto users, CancellationToken cancellationToken = default)
  {
    //_logger.LogInformation("Fetching mother company for user: {UserId}", users.UserId);

    //string additionalCondition = await _repository.AccessRestrictions.GenerateAccessRestrictionConditionForCompany((int)users.EmployeeId);
    string additionalCondition = await _repository.AccessRestrictions.GenerateAccessRestrictionConditionAsync((int)users.EmployeeId);

    if (!string.IsNullOrEmpty(additionalCondition))
      additionalCondition = " or " + additionalCondition;

    if (users.AccessParentCompany == 1 && string.IsNullOrEmpty(additionalCondition))
    {
      companyId = 0;
      var companyRepositoriesDto = await _repository.Companies.MotherCompaniesAsync(companyId, additionalCondition, cancellationToken);
      return MyMapper.JsonCloneIEnumerableToIEnumerable<Company, CompanyDto>(companyRepositoriesDto);
    }
    else
    {
      //int controlPanelModuleId = Convert.ToInt32(_configuration["AppSettings:controlPanelModuleId"]);
      //var res = await _repository.GroupPermissiones.AccessPermisionForCurrentUser(controlPanelModuleId, (int)users.UserId);

      int controlPanelModuleId = _appSettings.ControlPanelModuleId;

      var res = await _repository.GroupPermissiones.AccessPermissionsForCurrentUserAsync(controlPanelModuleId, (int)users.UserId);
      var isHr = res.Any(groupPermission => groupPermission.Referenceid == 22);

      if (isHr && string.IsNullOrEmpty(additionalCondition))
      {
        var motherCompanies = await _repository.Companies.MotherCompaniesAsync(companyId, additionalCondition, cancellationToken);
        return MyMapper.JsonCloneIEnumerableToList<Company, CompanyDto>(motherCompanies);
      }
      else
      {
        var getCompanyList = await _repository.Companies.CompaniesByCompanyIdAsync(companyId, additionalCondition, cancellationToken);
        return MyMapper.JsonCloneIEnumerableToList<Company, CompanyDto>(getCompanyList);
      }
    }
  }


  /// <summary>
  /// Retrieves a lightweight list of all access controls suitable for use in dropdown lists.
  /// Returns only the access control ID and name, ordered by name.
  /// </summary>
  /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
  /// <returns>A collection of <see cref="AccessControlDto"/> for dropdown binding.</returns>
  public async Task<IEnumerable<CompanyDDLDto>> CompaniesForDDLAsync(CancellationToken cancellationToken = default)
  {
    IEnumerable<CompanyDDLDto> companies = await _repository.Companies.ListWithSelectAsync(
        x => new CompanyDDLDto
        {
          CompanyId = x.CompanyId,
          CompanyName = x.CompanyName
        },
        orderBy: x => x.CompanyName,
        trackChanges: false,
        cancellationToken: cancellationToken);

    if (!companies.Any())
    {
      _logger.LogWarning("No companies found for dropdown list");
      return Enumerable.Empty<CompanyDDLDto>();
    }
    _logger.LogInformation("Companies fetched successfully for dropdown list");
    return companies;
  }




  /// <summary>
  /// Creates a new company record after validating for null input and duplicate company name.
  /// </summary>
  /// <param name="entityForCreate">The DTO containing data for the new company.</param>
  /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>





  /// <summary>
  /// Creates a new company record after validating for null input and duplicate company name.
  /// </summary>
  /// <param name="entityForCreate">The DTO containing data for the new company.</param>
  /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
  /// <returns>The created <see cref="CompanyDto"/> with the newly assigned ID.</returns>
  /// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
  /// <exception cref="DuplicateRecordException">Thrown when a company with the same name already exists.</exception>
  public async Task<CompanyDto> CreateAsync2(CompanyDto entityForCreate, CancellationToken cancellationToken = default)
  {
    if (entityForCreate is null)
      throw new BadRequestException(nameof(CompanyDto));

    bool companyExists = await _repository.Companies.ExistsAsync(
        c => c.CompanyName.Trim().ToLower() == entityForCreate.CompanyName.Trim().ToLower(),
        cancellationToken: cancellationToken);

    if (companyExists)
      throw new ConflictException("Duplicate data found!");

    Company companyEntity = MyMapper.JsonClone<CompanyDto, Company>(entityForCreate);

    await _repository.Companies.CreateAsync(companyEntity, cancellationToken);
    int affected = await _repository.SaveChangesAsync(cancellationToken);

    if (affected <= 0)
      throw new InvalidOperationException("Company could not be saved to the database.");
    _logger.LogInformation(
        "Company created successfully. ID: {CompanyId}, Name: {CompanyName}, Time: {Time}",
        companyEntity.CompanyId,
        companyEntity.CompanyName,
        DateTime.UtcNow);

    return MyMapper.JsonClone<Company, CompanyDto>(companyEntity);
  }

  /// <summary>
  /// Updates an existing company record by merging only the changed values from the provided DTO.
  /// Validates ID consistency, null input, record existence, and duplicate name constraints.
  /// </summary>
  /// <param name="companyId">The ID of the company to update.</param>
  /// <param name="modelDto">The DTO containing updated field values.</param>
  /// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
  /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
  /// <returns>The updated <see cref="CompanyDto"/> reflecting the saved state.</returns>
  /// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
  /// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
  /// <exception cref="NotFoundException">Thrown when no company is found for the given ID.</exception>
  /// <exception cref="DuplicateRecordException">Thrown when another company with the same name already exists.</exception>
  public async Task<CompanyDto> UpdateAsync2(int companyId, CompanyDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (modelDto is null)
      throw new BadRequestException(nameof(CompanyDto));

    if (companyId != modelDto.CompanyId)
      throw new BadRequestException(companyId.ToString(), nameof(CompanyDto));

    Company existingEntity = await _repository.Companies
        .FirstOrDefaultAsync(x => x.CompanyId == companyId, trackChanges: false, cancellationToken)
        ?? throw new NotFoundException("Data not found!");

    bool duplicateExists = await _repository.Companies.ExistsAsync(
        x => x.CompanyName.Trim().ToLower() == modelDto.CompanyName.Trim().ToLower()
          && x.CompanyId != companyId,
        cancellationToken: cancellationToken);

    if (duplicateExists)
      throw new ConflictException("Duplicate data found!");
    Company updatedEntity = MyMapper.MergeChangedValues<Company, CompanyDto>(existingEntity, modelDto);
    _repository.Companies.UpdateByState(updatedEntity);

    int affected = await _repository.SaveChangesAsync(cancellationToken);
    if (affected <= 0)
      throw new NotFoundException("Data not found!");
    _logger.LogInformation(
        "Company updated. ID: {CompanyId}, Name: {CompanyName}, Time: {Time}",
        updatedEntity.CompanyId,
        updatedEntity.CompanyName,
        DateTime.UtcNow);

    return MyMapper.JsonClone<Company, CompanyDto>(updatedEntity);
  }

  /// <summary>
  /// Deletes a company record identified by the given ID.
  /// Validates that the ID is positive and that the record exists before deletion.
  /// </summary>
  /// <param name="companyId">The ID of the company to delete.</param>
  /// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
  /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
  /// <exception cref="IdParametersBadRequestException">Thrown when <paramref name="companyId"/> is zero or negative.</exception>
  /// <exception cref="NotFoundException">Thrown when no company record is found for the given ID.</exception>
  public async Task<int> DeleteAsync2(int companyId, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (companyId <= 0)
      throw new BadRequestException(companyId.ToString(), nameof(CompanyDto));

    Company companyEntity = await _repository.Companies
        .FirstOrDefaultAsync(x => x.CompanyId == companyId, trackChanges, cancellationToken)
        ?? throw new NotFoundException("Data not found!");
    await _repository.Companies.DeleteAsync(x => x.CompanyId == companyId, trackChanges, cancellationToken);
    int affected = await _repository.SaveChangesAsync(cancellationToken);
    if (affected <= 0)
      throw new NotFoundException("Data not found!");
    _logger.LogWarning(
        "Company deleted. ID: {CompanyId}, Name: {CompanyName}, Time: {Time}",
        companyEntity.CompanyId,
        companyEntity.CompanyName,
        DateTime.UtcNow);
    return affected;
  }

  /// <summary>
  /// Retrieves all companies asynchronously.
  /// </summary>
  public async Task<IEnumerable<CompanyDto>> CompaniesAsync2(bool trackChanges, CancellationToken cancellationToken = default)
  {
    _logger.LogInformation("Fetching all companies");

    var companies = await _repository.Companies.CompaniesAsync(trackChanges, cancellationToken);

    if (!companies.Any())
    {
      _logger.LogWarning("No companies found");
      return Enumerable.Empty<CompanyDto>();
    }

    var companyDtos = MyMapper.JsonCloneIEnumerableToList<Company, CompanyDto>(companies);
    return companyDtos;
  }

  /// <summary>
  /// Retrieves all company records from the database.
  /// </summary>
  /// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
  /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
  /// <returns>A collection of all <see cref="MenuDto"/> records.</returns>
  public async Task<IEnumerable<CompanyDto>> CompaniesAsync3(bool trackChanges, CancellationToken cancellationToken = default)
  {
    IEnumerable<Company> companies = await _repository.Companies.CompaniesAsync(trackChanges, cancellationToken); if (!companies.Any())
    {
      _logger.LogWarning("No companies found");
      return Enumerable.Empty<CompanyDto>();
    }
    return MyMapper.JsonCloneIEnumerableToIEnumerable<Company, CompanyDto>(companies);
  }

  /// <summary>
  /// Retrieves a single company by ID asynchronously.
  /// </summary>
  public async Task<CompanyDto> CompanyAsync2(int id, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (id <= 0)
    {
      _logger.LogWarning("CompanyAsync called with invalid id: {CompanyId}", id);
      throw new BadRequestException("Invalid request!");
    }

    _logger.LogInformation("Fetching company with ID: {CompanyId}", id);

    var company = await _repository.Companies.CompanyAsync(id, trackChanges, cancellationToken);

    if (company == null)
    {
      _logger.LogWarning("Company not found with ID: {CompanyId}", id);
      throw new NotFoundException("Data not found!");
    }

    var companyDto = MyMapper.JsonClone<Company, CompanyDto>(company);
    return companyDto;
  }

  /// <summary>
  /// Retrieves companies by a collection of IDs asynchronously.
  /// </summary>
  public async Task<IEnumerable<CompanyDto>> ByIdsAsync2(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (ids == null)
      throw new BadRequestException("Invalid request!");

    _logger.LogInformation("Fetching companies by IDs: {CompanyIds}", string.Join(",", ids));

    var companyEntities = await _repository.Companies.ByIdsAsync(ids, trackChanges, cancellationToken);

    if (ids.Count() != companyEntities.Count())
      throw new BadRequestException("Invalid request!");

    var companiesToReturn = MyMapper.JsonCloneIEnumerableToList<Company, CompanyDto>(companyEntities);
    return companiesToReturn;
  }

  /// <summary>
  /// Retrieves mother company for the current user asynchronously.
  /// </summary>
  public async Task<IEnumerable<CompanyDto>> MotherCompanyAsync2(int companyId, UsersDto users, CancellationToken cancellationToken = default)
  {
    _logger.LogInformation("Fetching mother company for user: {UserId}", users.UserId);

    //string additionalCondition = await _repository.AccessRestrictions.GenerateAccessRestrictionConditionForCompany((int)users.EmployeeId);
    string additionalCondition = await _repository.AccessRestrictions.GenerateAccessRestrictionConditionAsync((int)users.EmployeeId);

    if (!string.IsNullOrEmpty(additionalCondition))
      additionalCondition = " or " + additionalCondition;

    if (users.AccessParentCompany == 1 && string.IsNullOrEmpty(additionalCondition))
    {
      companyId = 0;
      var companyRepositoriesDto = await _repository.Companies.MotherCompaniesAsync(companyId, additionalCondition, cancellationToken);
      return MyMapper.JsonCloneIEnumerableToList<Company, CompanyDto>(companyRepositoriesDto);
    }
    else
    {
      int controlPanelModuleId = Convert.ToInt32(_configuration["AppSettings:controlPanelModuleId"]);
      //var res = await _repository.GroupPermissiones.AccessPermisionForCurrentUser(controlPanelModuleId, (int)users.UserId);
      var res = await _repository.GroupPermissiones.AccessPermissionsForCurrentUserAsync(controlPanelModuleId, (int)users.UserId);
      var isHr = res.Any(groupPermission => groupPermission.Referenceid == 22);

      if (isHr && string.IsNullOrEmpty(additionalCondition))
      {
        var motherCompanies = await _repository.Companies.MotherCompaniesAsync(companyId, additionalCondition, cancellationToken);
        return MyMapper.JsonCloneIEnumerableToList<Company, CompanyDto>(motherCompanies);
      }
      else
      {
        var getCompanyList = await _repository.Companies.CompaniesByCompanyIdAsync(companyId, additionalCondition, cancellationToken);
        return MyMapper.JsonCloneIEnumerableToList<Company, CompanyDto>(getCompanyList);
      }
    }
  }

  /// <summary>
  /// Retrieves companies for dropdown list asynchronously.
  /// </summary>
  public async Task<IEnumerable<CompanyDDLDto>> CompaniesForDDLAsync2(CancellationToken cancellationToken = default)
  {
    _logger.LogInformation("Fetching companies for dropdown list");

    var companies = await _repository.Companies.ListWithSelectAsync(
        x => new Company
        {
          CompanyId = x.CompanyId,
          CompanyName = x.CompanyName
        },
        orderBy: x => x.CompanyName,
        trackChanges: false
    );

    if (!companies.Any())
      return new List<CompanyDDLDto>();

    var companiesForDDLDto = MyMapper.JsonCloneIEnumerableToList<Company, CompanyDDLDto>(companies);
    return companiesForDDLDto;
  }
}
