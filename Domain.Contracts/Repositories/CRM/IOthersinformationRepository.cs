using Domain.Entities.Entities.CRM;
using Domain.Contracts.Repositories;

namespace Domain.Contracts.CRM;

public interface ICrmOthersInformationRepository : IRepositoryBase<CrmOthersInformation>
{
	/// <summary>
	/// Retrieves all CrmOthersInformation records asynchronously.
	/// </summary>
	Task<IEnumerable<CrmOthersInformation>> CrmOthersInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single CrmOthersInformation record by ID asynchronously.
	/// </summary>
	Task<CrmOthersInformation?> CrmOthersInformationAsync(int OthersInformationId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves CrmOthersInformation records by a collection of IDs asynchronously.
	/// </summary>
	Task<IEnumerable<CrmOthersInformation>> CrmOthersInformationsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves CrmOthersInformation records by parent ID asynchronously.
	/// </summary>
	Task<IEnumerable<CrmOthersInformation>> CrmOthersInformationsByParentIdAsync(int parentId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates a new CrmOthersInformation record.
	/// </summary>
	Task<CrmOthersInformation> CreateCrmOthersInformationAsync(CrmOthersInformation entity, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing CrmOthersInformation record.
	/// </summary>
	void UpdateCrmOthersInformation(CrmOthersInformation entity);

	/// <summary>
	/// Deletes a CrmOthersInformation record.
	/// </summary>
	Task DeleteCrmOthersInformationAsync(CrmOthersInformation entity, bool trackChanges, CancellationToken cancellationToken = default);

	//Task<IEnumerable<CrmOthersInformation>> ActiveOthersinformationsAsync(bool track);
	//Task<IEnumerable<CrmOthersInformation>> OthersinformationsAsync(bool track);
	//Task<CrmOthersInformation?> OthersinformationAsync(int id, bool track);
	//Task<IEnumerable<CrmOthersInformation>> OthersinformationsByApplicantIdAsync(int applicantId, bool track);
	//Task<CrmOthersInformation?> OthersinformationByApplicantIdAsync(int applicantId, bool track);
}