using Domain.Entities.Entities.CRM;
using Domain.Contracts.Repositories;

namespace Domain.Contracts.CRM;

/// <summary>
/// Repository interface for CRMYear entity operations.
/// </summary>
public interface ICRMYearRepository : IRepositoryBase<CrmYear>
{
	/// <summary>
	/// Retrieves all CrmYear records asynchronously.
	/// </summary>
	Task<IEnumerable<CrmYear>> CrmYearsAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single CrmYear record by ID asynchronously.
	/// </summary>
	Task<CrmYear?> CrmYearAsync(int crmyearid, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves CrmYear records by a collection of IDs asynchronously.
	/// </summary>
	Task<IEnumerable<CrmYear>> CrmYearsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves CrmYear records by parent ID asynchronously.
	/// </summary>
	Task<IEnumerable<CrmYear>> CrmYearsByParentIdAsync(int parentId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates a new CrmYear record.
	/// </summary>
	Task<CrmYear> CreateCrmYearAsync(CrmYear entity, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing CrmYear record.
	/// </summary>
	void UpdateCrmYear(CrmYear entity);

	/// <summary>
	/// Deletes a CrmYear record.
	/// </summary>
	Task DeleteCrmYearAsync(CrmYear entity, bool trackChanges, CancellationToken cancellationToken = default);
}











//using Domain.Entities.Entities.CRM;

//namespace bdDevCRM.RepositoriesContracts.CRM;

//public interface ICrmYearRepository : IRepositoryBase<CrmYear>
//{
//  Task<IEnumerable<CrmYear>> ActiveYearAsync(bool trackChanges);
//}
