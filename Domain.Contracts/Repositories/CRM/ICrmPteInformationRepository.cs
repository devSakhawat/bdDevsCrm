using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmPteInformationRepository : IRepositoryBase<CrmPteInformation>
{
	/// <summary>
	/// Retrieves all CrmPteInformation records asynchronously.
	/// </summary>
	Task<IEnumerable<CrmPteInformation>> CrmPteInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single CrmPteInformation record by ID asynchronously.
	/// </summary>
	Task<CrmPteInformation?> CrmPteInformationAsync(int PTEInformationId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves CrmPteInformation records by a collection of IDs asynchronously.
	/// </summary>
	Task<IEnumerable<CrmPteInformation>> CrmPteInformationsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves CrmPteInformation records by parent ID asynchronously.
	/// </summary>
	Task<IEnumerable<CrmPteInformation>> CrmPteInformationsByParentIdAsync(int parentId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates a new CrmPteInformation record.
	/// </summary>
	Task<CrmPteInformation> CreateCrmPTEInformationAsync(CrmPteInformation entity, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing CrmPteInformation record.
	/// </summary>
	void UpdateCrmPTEInformation(CrmPteInformation entity);

	/// <summary>
	/// Deletes a CrmPteInformation record.
	/// </summary>
	Task DeleteCrmPTEInformationAsync(CrmPteInformation entity, bool trackChanges, CancellationToken cancellationToken = default);

	//Task<IEnumerable<CrmPteInformation>> ActivePTEInformationsAsync(bool track);
	//Task<IEnumerable<CrmPteInformation>> PTEInformationsAsync(bool track);
	//Task<CrmPteInformation?> PTEInformationAsync(int id, bool track);
	//Task<IEnumerable<CrmPteInformation>> PTEInformationsByApplicantIdAsync(int applicantId, bool track);
	//Task<CrmPteInformation?> PTEInformationByApplicantIdAsync(int applicantId, bool track);
}