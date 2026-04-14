using Domain.Entities.Entities.System;
using Domain.Contracts.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.Core.SystemAdmin;

/// <summary>
/// Repository for company data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
	private const string MotherCompanyQueryForEditCompanySql = @"WITH hierarchy AS (
        SELECT ROW_NUMBER() OVER (ORDER BY t.CompanyName) as RowIndex, t.CompanyId, t.CompanyName, t.PrimaryContact, t.Email, t.Fax, t.Phone, t.Address, t.FullLogoPath, t.FullLogoPathForReport, t.LetterHeader, t.LetterFooter, t.MotherId, t.CompanyCode
        FROM dbo.Company t
        WHERE t.CompanyId={0}
        UNION ALL
        SELECT ROW_NUMBER() OVER (ORDER BY x.CompanyName) as RowIndex, x.CompanyId, x.CompanyName, x.PrimaryContact, x.Email, x.Fax, x.Phone, x.Address, x.FullLogoPath, x.FullLogoPathForReport, x.LetterHeader, x.LetterFooter, x.MotherId, x.CompanyCode
        FROM dbo.Company x
        JOIN hierarchy y ON x.CompanyId = y.MotherId)
        SELECT * FROM hierarchy s where CompanyId <> {0} ORDER BY CompanyName";

	public CompanyRepository(CrmContext context) : base(context) { }

	/// <summary>
	/// Retrieves all companies asynchronously.
	/// </summary>
	public async Task<IEnumerable<Company>> CompaniesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(x => x.CompanyName, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a single company by ID asynchronously.
	/// </summary>
	public async Task<Company?> CompanyAsync(int companyId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(c => c.CompanyId.Equals(companyId), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves companies by a collection of IDs asynchronously.
	/// </summary>
	public async Task<IEnumerable<Company>> ByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(c => ids.Contains(c.CompanyId), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves mother companies for dropdown combo asynchronously.
	/// </summary>
	public async Task<IEnumerable<Company>> MotherCompanyComboAsync(int companyId, int sessionCompanyId, CancellationToken cancellationToken = default)
	{
		var sql = string.Format(MotherCompanyQueryForEditCompanySql, companyId, sessionCompanyId);
		return await AdoExecuteListQueryAsync<Company>(sql, null, cancellationToken);
	}

	/// <summary>
	/// Retrieves mother companies with additional condition asynchronously.
	/// </summary>
	public async Task<IEnumerable<Company>> MotherCompaniesAsync(int companyId, string additionalCondition, CancellationToken cancellationToken = default)
	{
		string sql = await BuildMotherCompanyQueryAsync(companyId, additionalCondition);
		return await AdoExecuteListQueryAsync<Company>(sql, null, cancellationToken);
	}

	/// <summary>
	/// Builds the SQL query for mother company retrieval.
	/// </summary>
	private async Task<string> BuildMotherCompanyQueryAsync(int companyId, string additionalCondition)
	{
		string sql = string.Empty;
		var companyOrder = "CompanyName";
		var assemblyQuery = "Select * from AssemblyInfo";
		var objAssembly = await AdoExecuteSingleDataAsync<AssemblyInfo>(assemblyQuery);

		if (objAssembly?.AssemblyInfoId == 1)
			companyOrder = "CompanyCode";

		if (companyId == 0)
		{
			sql = $@"SELECT [CompanyId],[CompanyCode],[CompanyName],[Address],[Phone],[Fax],[Email],[FullLogoPath],[PrimaryContact],[Flag],[FiscalYearStart],[MotherId],[IsCostCentre]
                ,[IsActive],[GratuityStartDate],[FullLogoPathForReport],[LetterHeader],[LetterFooter],[CompanyTin],ISNULL(IsPfApplicable,0) as IsPfApplicable,ISNULL(IsEwfApplicable,0) as IsEwfApplicable,ISNULL(IsPfApplicabe,0) as IsPfApplicabe,ISNULL(IsEwfApplicabe,0) as IsEwfApplicabe
                ,[CompanyAlias],[CompanyZone],[CompanyCircle],[IsCompanyContributionDisable] 
                FROM Company 
                WHERE IsActive = 1 {additionalCondition} 
                ORDER BY {companyOrder}";
		}
		else
		{
			sql = $@"WITH hierarchy (CompanyId, CompanyName, PrimaryContact, Email, Fax, Phone, Address, FullLogoPath, MotherId, Flag, FiscalYearStart, IsActive, CompanyCode) AS (
                SELECT CompanyId, CompanyName, PrimaryContact, Email, Fax, Phone, Address, FullLogoPath, MotherId, Flag, FiscalYearStart, IsActive, CompanyCode 
                FROM Company 
                WHERE CompanyId = {companyId} AND IsActive = 1 
                UNION ALL 
                SELECT x.CompanyId, x.CompanyName, x.PrimaryContact, x.Email, x.Fax, x.Phone, x.Address, x.FullLogoPath, x.MotherId, x.Flag, x.FiscalYearStart, x.IsActive, x.CompanyCode 
                FROM Company x 
                JOIN hierarchy y ON (y.CompanyId = x.MotherId AND x.IsActive = 1)) 
                SELECT * FROM hierarchy 
                WHERE IsActive = 1 {additionalCondition} 
                ORDER BY {companyOrder} ASC";
		}

		return sql;
	}

	/// <summary>
	/// Retrieves companies by company ID with additional condition asynchronously.
	/// </summary>
	public async Task<IEnumerable<Company>> CompaniesByCompanyIdAsync(int companyId, string additionalCondition, CancellationToken cancellationToken = default)
	{
		var sql = $"SELECT * FROM Company WHERE CompanyId = {companyId} {additionalCondition}";
		return await AdoExecuteListQueryAsync<Company>(sql, null, cancellationToken);
	}

	/// <summary>
	/// Retrieves a company by ID with additional condition asynchronously.
	/// </summary>
	public async Task<Company?> CompanyByIdWithConditionAsync(int companyId, string additionalCondition, CancellationToken cancellationToken = default)
	{
		var query = $"SELECT * FROM Company WHERE CompanyId = {companyId} {additionalCondition}";
		return await AdoExecuteSingleDataAsync<Company>(query, null, cancellationToken);
	}

	/// <summary>
	/// Creates a new company.
	/// </summary>
	public async Task<Company> CreateCompanyAsync(Company company ,CancellationToken cancellationToken = default)
	{
		int companyId = await CreateAndIdAsync(company, cancellationToken);
		company.CompanyId = companyId;
		return company;
	}

	/// <summary>
	/// Updates an existing company.
	/// </summary>
	public void UpdateCompany(Company company) => UpdateByState(company);

	/// <summary>
	/// Deletes a company.
	/// </summary>
	public async Task DeleteCompanyAsync(Company company, bool trackChanges ,CancellationToken cancellationToken = default)
		=> await DeleteAsync(x => x.CompanyId == company.CompanyId, true, cancellationToken);
}
