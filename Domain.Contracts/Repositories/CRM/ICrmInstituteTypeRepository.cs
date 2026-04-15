using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmInstituteTypeRepository : IRepositoryBase<CrmInstituteType>
{
  /// <summary>
  /// Retrieves all CrmInstituteType records asynchronously.
  /// </summary>
  Task<IEnumerable<CrmInstituteType>> CrmInstituteTypesAsync(bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves a single CrmInstituteType record by ID asynchronously.
  /// </summary>
  Task<CrmInstituteType?> CrmInstituteTypeAsync(int crminstitutetypeid, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmInstituteType records by a collection of IDs asynchronously.
  /// </summary>
  Task<IEnumerable<CrmInstituteType>> CrmInstituteTypesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmInstituteType records by parent ID asynchronously.
  /// </summary>
  Task<IEnumerable<CrmInstituteType>> CrmInstituteTypesByParentIdAsync(int parentId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Creates a new CrmInstituteType record.
  /// </summary>
  Task<CrmInstituteType> CreateCrmInstituteTypeAsync(CrmInstituteType entity, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an existing CrmInstituteType record.
  /// </summary>
  void UpdateCrmInstituteType(CrmInstituteType entity);

  /// <summary>
  /// Deletes a CrmInstituteType record.
  /// </summary>
  Task DeleteCrmInstituteTypeAsync(CrmInstituteType entity, bool trackChanges, CancellationToken cancellationToken = default);


  //Task<IEnumerable<CrmInstituteType>> InstituteTypesAsync(bool trackChanges, CancellationToken cancellationToken = default);
  //Task<CrmInstituteType?> InstituteTypeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
}



//using Domain.Entities.Entities.CRM;

//namespace Domain.Contracts.Core.SystemAdmin;

//public interface ICrmInstituteTypeRepository : IRepositoryBase<CrmInstituteType>
//{
//  Task<IEnumerable<CrmInstituteType>> InstituteTypesAsync(bool trackChanges);
//  Task<CrmInstituteType?> InstituteTypeAsync(int id, bool trackChanges);
//}
