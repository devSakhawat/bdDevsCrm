using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmStatementOfPurposeRepository : IRepositoryBase<CrmStatementOfPurpose>
{
	/// <summary>
	/// Retrieves all CrmStatementOfPurpose records asynchronously.
	/// </summary>
	Task<IEnumerable<CrmStatementOfPurpose>> CrmStatementOfPurposesAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single CrmStatementOfPurpose record by ID asynchronously.
	/// </summary>
	Task<CrmStatementOfPurpose?> CrmStatementOfPurposeAsync(int StatementOfPurposeId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves CrmStatementOfPurpose records by a collection of IDs asynchronously.
	/// </summary>
	Task<IEnumerable<CrmStatementOfPurpose>> CrmStatementOfPurposesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves CrmStatementOfPurpose records by parent ID asynchronously.
	/// </summary>
	Task<IEnumerable<CrmStatementOfPurpose>> CrmStatementOfPurposesByParentIdAsync(int parentId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates a new CrmStatementOfPurpose record.
	/// </summary>
	Task<CrmStatementOfPurpose> CreateCrmStatementOfPurposeAsync(CrmStatementOfPurpose entity, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing CrmStatementOfPurpose record.
	/// </summary>
	void UpdateCrmStatementOfPurpose(CrmStatementOfPurpose entity);

	/// <summary>
	/// Deletes a CrmStatementOfPurpose record.
	/// </summary>
	Task DeleteCrmStatementOfPurposeAsync(CrmStatementOfPurpose entity, bool trackChanges, CancellationToken cancellationToken = default);


	//Task<IEnumerable<CrmStatementOfPurpose>> ActiveStatementOfPurposesAsync(bool track);
	//Task<IEnumerable<CrmStatementOfPurpose>> StatementOfPurposesAsync(bool track);
	//Task<CrmStatementOfPurpose?> StatementOfPurposeAsync(int id, bool track);
	//Task<CrmStatementOfPurpose?> StatementOfPurposeByApplicantIdAsync(int applicantId, bool track);
}