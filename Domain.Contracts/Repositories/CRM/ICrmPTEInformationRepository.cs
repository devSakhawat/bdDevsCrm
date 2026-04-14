using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmPTEInformationRepository : IRepositoryBase<CrmPTEInformation>
{
	/// <summary>
	/// Retrieves all CrmPTEInformation records asynchronously.
	/// </summary>
	Task<IEnumerable<CrmPTEInformation>> CrmPTEInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single CrmPTEInformation record by ID asynchronously.
	/// </summary>
	Task<CrmPTEInformation?> CrmPTEInformationAsync(int PTEInformationId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves CrmPTEInformation records by a collection of IDs asynchronously.
	/// </summary>
	Task<IEnumerable<CrmPTEInformation>> CrmPTEInformationsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves CrmPTEInformation records by parent ID asynchronously.
	/// </summary>
	Task<IEnumerable<CrmPTEInformation>> CrmPTEInformationsByParentIdAsync(int parentId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates a new CrmPTEInformation record.
	/// </summary>
	Task<CrmPTEInformation> CreateCrmPTEInformationAsync(CrmPTEInformation entity, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing CrmPTEInformation record.
	/// </summary>
	void UpdateCrmPTEInformation(CrmPTEInformation entity);

	/// <summary>
	/// Deletes a CrmPTEInformation record.
	/// </summary>
	Task DeleteCrmPTEInformationAsync(CrmPTEInformation entity, bool trackChanges, CancellationToken cancellationToken = default);

	//Task<IEnumerable<CrmPTEInformation>> ActivePTEInformationsAsync(bool track);
	//Task<IEnumerable<CrmPTEInformation>> PTEInformationsAsync(bool track);
	//Task<CrmPTEInformation?> PTEInformationAsync(int id, bool track);
	//Task<IEnumerable<CrmPTEInformation>> PTEInformationsByApplicantIdAsync(int applicantId, bool track);
	//Task<CrmPTEInformation?> PTEInformationByApplicantIdAsync(int applicantId, bool track);
}