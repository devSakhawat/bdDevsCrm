using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Core.SystemAdmin;

public interface ICompanyRepository : IRepositoryBase<Company>
{
  /// <summary>
  /// Retrieves all companies asynchronously.
  /// </summary>
  Task<IEnumerable<Company>> CompaniesAsync(bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves a single company by ID asynchronously.
  /// </summary>
  Task<Company?> CompanyAsync(int companyId, bool trackChanges, CancellationToken cancellationToken = default);


  /// <summary>
  /// Retrieves companies by a collection of IDs asynchronously.
  /// </summary>
  Task<IEnumerable<Company>> ByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves mother companies for dropdown combo asynchronously.
  /// </summary>
  Task<IEnumerable<Company>> MotherCompanyComboAsync(int companyId, int sessionCompanyId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves mother companies with additional condition asynchronously.
  /// </summary>
  Task<IEnumerable<Company>> MotherCompaniesAsync(int companyId, string additionalCondition, CancellationToken cancellationToken = default);


  ///// <summary>
  ///// Builds the SQL query for mother company retrieval.
  ///// </summary>
  //Task<string> BuildMotherCompanyQueryAsync(int companyId, string additionalCondition);

  /// <summary>
  /// Retrieves companies by company ID with additional condition asynchronously.
  /// </summary>
  Task<IEnumerable<Company>> CompaniesByCompanyIdAsync(int companyId, string additionalCondition, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves a company by ID with additional condition asynchronously.
  /// </summary>
  Task<Company?> CompanyByIdWithConditionAsync(int companyId, string additionalCondition, CancellationToken cancellationToken = default);

  /// <summary>
  /// Creates a new company.
  /// </summary>
  Task<Company> CreateCompanyAsync(Company company, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an existing company.
  /// </summary>
  void UpdateCompany(Company company);

  /// <summary>
  /// Deletes a company.
  /// </summary>
  Task DeleteCompanyAsync(Company company, bool trackChanges, CancellationToken cancellationToken = default);

}

//public interface ICompanyRepository : IRepositoryBase<Company>
//{
//  //IEnumerable<Company> Companies(bool trackChanges);
//  Task<IEnumerable<Company>> CompaniesAsunc(bool trackChanges, CancellationToken cancellation);
//  //Company Company(int companyId, bool trackChanges);
//  Task<Company> CompanyAsync(int companyId, bool trackChanges ,CancellationToken cancellationToken);

//  Task<IEnumerable<Company>> ByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellation);
//  Task<IEnumerable<Company>> MotherCompanyCombo(int companyId, int seastionCompanyId, CancellationToken cancellationToken);
//  //string MotherCompanyQuary(int companyId, string additionalCondition);
//  Task<IEnumerable<Company>> MotherCompanies(int companyId, string additionalCondition, CancellationToken cancellationToken);
//  Task<IEnumerable<Company>> CompaniesByCompanyIdAsync(int companyId, string additionalCondition, CancellationToken cancellationToken);

//  Task<Company> CompanyByCompanyIdAsync(int companyId, bool trackChanges, CancellationToken cancellationToken);
//  Task<Company?> CompanyByCompanyIdWithAdditionalCondition(int companyId, string additionalCondition, CancellationToken cancellationToken);

//  Task CreateCompanyAsync(Company Company, CancellationToken cancellationToken);
//  void UpdateCompany(Company Company);
//  void DeleteCompany(Company Company);
//}
