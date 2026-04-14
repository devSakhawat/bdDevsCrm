using Domain.Entities.Entities.CRM;
using Domain.Contracts.Repositories;

namespace Domain.Contracts.CRM;

public interface ICrmWorkExperienceRepository : IRepositoryBase<CrmWorkExperience>
{
	/// <summary>
	/// Retrieves all CrmWorkExperience records asynchronously.
	/// </summary>
	Task<IEnumerable<CrmWorkExperience>> CrmWorkExperiencesAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single CrmWorkExperience record by ID asynchronously.
	/// </summary>
	Task<CrmWorkExperience?> CrmWorkExperienceAsync(int crmworkexperienceid, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves CrmWorkExperience records by a collection of IDs asynchronously.
	/// </summary>
	Task<IEnumerable<CrmWorkExperience>> CrmWorkExperiencesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves CrmWorkExperience records by a applicantId asynchronously.
	/// </summary>
	Task<IEnumerable<CrmWorkExperience>> CrmWorkExperiencesByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves CrmWorkExperience records by parent ID asynchronously.
		/// </summary>
		Task<IEnumerable<CrmWorkExperience>> CrmWorkExperiencesByParentIdAsync(int parentId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates a new CrmWorkExperience record.
	/// </summary>
	Task<CrmWorkExperience> CreateCrmWorkExperienceAsync(CrmWorkExperience entity, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing CrmWorkExperience record.
	/// </summary>
	void UpdateCrmWorkExperience(CrmWorkExperience entity);

	/// <summary>
	/// Deletes a CrmWorkExperience record.
	/// </summary>
	Task DeleteCrmWorkExperienceAsync(CrmWorkExperience entity, bool trackChanges, CancellationToken cancellationToken = default);



	//Task<IEnumerable<CrmWorkExperience>> ActiveWorkExperiencesAsync(bool track);
	//Task<IEnumerable<CrmWorkExperience>> WorkExperiencesAsync(bool track);
	//Task<CrmWorkExperience?> WorkExperienceAsync(int id, bool track);
	//Task<IEnumerable<CrmWorkExperience>> WorkExperiencesByApplicantIdAsync(int applicantId, bool track);

	//Task<IEnumerable<WorkExperienceHistory>> WorkExperiencesByApplicantId(int applicantId);
}