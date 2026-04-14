// Interface: ICrmIntakeYearRepository

using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM
{
  public interface ICrmIntakeYearRepository : IRepositoryBase<CrmIntakeYear>
  {
    /// <summary>
    /// Retrieves all CrmIntakeYear records asynchronously.
    /// </summary>
    Task<IEnumerable<CrmIntakeYear>> CrmIntakeYearsAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single CrmIntakeYear record by ID asynchronously.
    /// </summary>
    Task<CrmIntakeYear?> CrmIntakeYearAsync(int crmintakeyearid, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves CrmIntakeYear records by a collection of IDs asynchronously.
    /// </summary>
    Task<IEnumerable<CrmIntakeYear>> CrmIntakeYearsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves CrmIntakeYear records by parent ID asynchronously.
    /// </summary>
    Task<IEnumerable<CrmIntakeYear>> CrmIntakeYearsByParentIdAsync(int parentId, CancellationToken cancellationToken = default);

		/// <summary>
		/// Creates a new CrmIntakeYear record.
		/// </summary>
		Task<CrmIntakeYear> CreateCrmIntakeYearAsync(CrmIntakeYear entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing CrmIntakeYear record.
    /// </summary>
    void UpdateCrmIntakeYear(CrmIntakeYear entity);

    /// <summary>
    /// Deletes a CrmIntakeYear record.
    /// </summary>
    Task DeleteCrmIntakeYearAsync(CrmIntakeYear entity, bool trackChanges, CancellationToken cancellationToken = default);


    //Task<IEnumerable<CrmIntakeYear>> IntakeYearsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    //Task<IEnumerable<CrmIntakeYear>> ActiveIntakeYearsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    //Task<CrmIntakeYear?> IntakeYearAsync(int intakeYearId, bool trackChanges, CancellationToken cancellationToken = default);
    //void CreateIntakeYear(CrmIntakeYear intakeYear);
    //void UpdateIntakeYear(CrmIntakeYear intakeYear);
    //void DeleteIntakeYear(CrmIntakeYear intakeYear);
  }
}






//using Domain.Entities.Entities.CRM;

//namespace bdDevCRM.RepositoriesContracts.Core.SystemAdmin;

//public interface ICrmIntakeYearRepository : IRepositoryBase<CrmIntakeYear>
//{
//  Task<IEnumerable<CrmIntakeYear>> ActiveIntakeYearsAsync(bool trackChanges);
//  Task<CrmIntakeYear?> IntakeYearByIdAsync(int intakeYearId, bool trackChanges);
//  void CreateIntakeYear(CrmIntakeYear intakeYear);
//  void UpdateIntakeYear(CrmIntakeYear intakeYear);
//  void DeleteIntakeYear(CrmIntakeYear intakeYear);
//}