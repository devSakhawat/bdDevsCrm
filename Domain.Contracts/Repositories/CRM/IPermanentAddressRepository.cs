using Domain.Entities.Entities.CRM;
using Domain.Contracts.Repositories;

namespace Domain.Contracts.CRM;

public interface ICrmPermanentAddressRepository : IRepositoryBase<CrmPermanentAddress>
{
  /// <summary>
  /// Retrieves all CrmPermanentAddress records asynchronously.
  /// </summary>
  Task<IEnumerable<CrmPermanentAddress>> CrmPermanentAddresssAsync(bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves a single CrmPermanentAddress record by ID asynchronously.
  /// </summary>
  Task<CrmPermanentAddress?> CrmPermanentAddressAsync(int crmpermanentaddressid, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmPermanentAddress records by a collection of IDs asynchronously.
  /// </summary>
  Task<IEnumerable<CrmPermanentAddress>> CrmPermanentAddresssByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves CrmPermanentAddress records by a countryId asynchronously.
  /// </summary>
  Task<IEnumerable<CrmPermanentAddress>> CrmPermanentAddresssByCountryIdAsync(int countryId, bool trackChanges, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves CrmPermanentAddress records by parent ID asynchronously.
		/// </summary>
		Task<IEnumerable<CrmPermanentAddress>> CrmPermanentAddresssByParentIdAsync(int parentId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Creates a new CrmPermanentAddress record.
  /// </summary>
  Task<CrmPermanentAddress> CreateCrmPermanentAddressAsync(CrmPermanentAddress entity, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an existing CrmPermanentAddress record.
  /// </summary>
  public void UpdateCrmPermanentAddress(CrmPermanentAddress entity) => UpdateByState(entity);

  /// <summary>
  /// Deletes a CrmPermanentAddress record.
  /// </summary>
  Task DeleteCrmPermanentAddressAsync(CrmPermanentAddress entity, bool trackChanges, CancellationToken cancellationToken = default);


  //Task<IEnumerable<CrmPermanentAddress>> ActivePermanentAddressesAsync(bool track);
  //Task<IEnumerable<CrmPermanentAddress>> PermanentAddressesAsync(bool track);
  //Task<CrmPermanentAddress?> PermanentAddressAsync(int id, bool track);
  //Task<CrmPermanentAddress?> PermanentAddressByApplicantIdAsync(int applicantId, bool track);
  //Task<IEnumerable<CrmPermanentAddress>> PermanentAddressesByCountryIdAsync(int countryId, bool track);
}