using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmPresentAddressRepository : IRepositoryBase<CrmPresentAddress>
{

	/// <summary>
	/// Retrieves all CrmPresentAddress records asynchronously.
	/// </summary>
	Task<IEnumerable<CrmPresentAddress>> CrmPresentAddresssAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single CrmPresentAddress record by ID asynchronously.
	/// </summary>
	Task<CrmPresentAddress?> CrmPresentAddressAsync(int crmpresentaddressid, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves CrmPresentAddress records by a collection of IDs asynchronously.
	/// </summary>
	Task<IEnumerable<CrmPresentAddress>> CrmPresentAddresssByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves CrmPresentAddress records by a countryId asynchronously.
	/// </summary>
	Task<IEnumerable<CrmPresentAddress>> CrmPresentAddresssByCountryIdAsync(int countryId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves CrmPresentAddress records by parent ID asynchronously.
	/// </summary>
	Task<IEnumerable<CrmPresentAddress>> CrmPresentAddresssByParentIdAsync(int parentId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates a new CrmPresentAddress record.
	/// </summary>
	Task<CrmPresentAddress> CreateCrmPresentAddressAsync(CrmPresentAddress entity, CancellationToken cancellation = default);

	/// <summary>
	/// Updates an existing CrmPresentAddress record.
	/// </summary>
	void UpdateCrmPresentAddress(CrmPresentAddress entity);

	/// <summary>
	/// Deletes a CrmPresentAddress record.
	/// </summary>
	Task DeleteCrmPresentAddressAsync(CrmPresentAddress entity, bool trackChanges, CancellationToken cancellationToken = default);


	//Task<IEnumerable<CrmPresentAddress>> ActivePresentAddressesAsync(bool track);
	//Task<IEnumerable<CrmPresentAddress>> PresentAddressesAsync(bool track);
	//Task<CrmPresentAddress?> PresentAddressAsync(int id, bool track);
	//Task<CrmPresentAddress?> PresentAddressByApplicantIdAsync(int applicantId, bool track);
	//Task<IEnumerable<CrmPresentAddress>> PresentAddressesByCountryIdAsync(int countryId, bool track);
}