using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface ICompanyService
{
  /// <summary>
  /// Retrieves all companies asynchronously.
  /// </summary>
  Task<IEnumerable<CompanyDto>> CompaniesAsync(bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves a single company by ID asynchronously.
  /// </summary>
  Task<CompanyDto> CompanyAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Creates a new company asynchronously.
  /// </summary>
  Task<CompanyDto> CreateAsync(CompanyDto modelDto, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an existing company asynchronously.
  /// </summary>
  Task<CompanyDto> UpdateAsync(int companyId, CompanyDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Deletes a company by ID asynchronously.
  /// </summary>
  Task<int> DeleteAsync(int companyId, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves companies by a collection of IDs asynchronously.
  /// </summary>
  Task<IEnumerable<CompanyDto>> CompaniesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves mother company for the current user asynchronously.
  /// </summary>
  Task<IEnumerable<CompanyDto>> MotherCompanyAsync(int companyId, UsersDto users, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves companies for dropdown list asynchronously.
  /// </summary>
  Task<IEnumerable<CompanyDDLDto>> CompaniesForDDLAsync(CancellationToken cancellationToken = default);


  //IEnumerable<CompanyDto> AllCompanies(bool trackChanges);
  //CompanyDto Company(int companyId, bool trackChanges);
  //CompanyDto CreateCompany(CompanyDto company);
  //IEnumerable<CompanyDto> ByIds(IEnumerable<int> ids, bool trackChanges);
  //Task<IEnumerable<CompanyDto>> MotherCompanyForEditCompanyCombo(int companyId, int seastionCompnayId);
  //Task<IEnumerable<CompanyDto>> MotherCompany(int companyId, UsersDto users);

  //Task<IEnumerable<CompanyDto>> CompaniesAsync(bool trackChanges);
  //Task<CompanyDto> CompanyAsync(int companyId, bool trackChanges);
  //Task<IEnumerable<CompanyDto>> ByIdsAsync(IEnumerable<int> ids, bool trackChanges);
  //Task<(IEnumerable<CompanyDto> Companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyDto> CompanyCollection);
  //Task<CompanyDto> CreateCompanyAsync(CompanyDto entityForCreate);
  //Task DeleteCompanyAsync(int companyId, bool trackChanges);
  //Task UpdateCompanyAsync(int companyId, CompanyDto companyForUpdate, bool trackChanges);

  //Task<IEnumerable<CompanyDDLDto>> CompaniesForDDLAsync(CancellationToken cancellationToken = default);
}
