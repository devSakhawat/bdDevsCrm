using Domain.Entities.Entities.CRM;
using Domain.Contracts.Repositories;

namespace Domain.Contracts.CRM;

public interface ICrmApplicantReferenceRepository : IRepositoryBase<CrmApplicantReference>
{
	/// <summary>
	/// Retrieves all CrmApplicantReference records asynchronously.
	/// </summary>
	Task<IEnumerable<CrmApplicantReference>> CrmApplicantReferencesAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single CrmApplicantReference record by ID asynchronously.
	/// </summary>
	Task<CrmApplicantReference?> CrmApplicantReferenceAsync(int crmapplicantreferenceid, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves CrmApplicantReference records by a collection of IDs asynchronously.
	/// </summary>
	Task<IEnumerable<CrmApplicantReference>> CrmApplicantReferencesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves CrmApplicantReference records by a applicantId asynchronously.
	/// </summary>
	Task<IEnumerable<CrmApplicantReference>> CrmApplicantReferencesByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves CrmApplicantReference records by parent ID asynchronously.
	/// </summary>
	Task<IEnumerable<CrmApplicantReference>> CrmApplicantReferencesByParentIdAsync(int parentId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates a new CrmApplicantReference record.
	/// </summary>
	Task<CrmApplicantReference> CreateCrmApplicantReference(CrmApplicantReference entity, CancellationToken cancellation = default);

	/// <summary>
	/// Updates an existing CrmApplicantReference record.
	/// </summary>
	void UpdateCrmApplicantReference(CrmApplicantReference entity) => UpdateByState(entity);

	/// <summary>
	/// Deletes a CrmApplicantReference record.
	/// </summary>
	Task DeleteCrmApplicantReference(CrmApplicantReference entity, bool trackChanges, CancellationToken cancellationToken);


	//Task<IEnumerable<CrmApplicantReference>> ActiveApplicantReferencesAsync(bool track);
	//Task<IEnumerable<CrmApplicantReference>> ApplicantReferencesAsync(bool track);
	//Task<CrmApplicantReference?> ApplicantReferenceAsync(int id, bool track);
	//Task<IEnumerable<CrmApplicantReference>> ApplicantReferencesByApplicantIdAsync(int applicantId, bool track);
}