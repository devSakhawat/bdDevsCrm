using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmApplicantCourse data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmApplicantCourseRepository : RepositoryBase<CrmApplicantCourse>, ICrmApplicantCourseRepository
{
	public CrmApplicantCourseRepository(CRMContext context) : base(context) { }

	/// <summary>
	/// Retrieves all CrmApplicantCourse records asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmApplicantCourse>> CrmApplicantCoursesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(x => x.ApplicantCourseId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a single CrmApplicantCourse record by ID asynchronously.
	/// </summary>
	public async Task<CrmApplicantCourse?> CrmApplicantCourseAsync(int crmapplicantcourseid, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(
				x => x.ApplicantCourseId.Equals(crmapplicantcourseid),
				trackChanges,
				cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmApplicantCourse records by a collection of IDs asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmApplicantCourse>> CrmApplicantCoursesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(x => ids.Contains(x.ApplicantCourseId), trackChanges, cancellationToken);
	}


	/// <summary>
	/// Retrieves CrmApplicantCourse records by a applicationId asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmApplicantCourse>> CrmApplicantCoursesByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByConditionAsync( expression: x => x.ApplicantId == applicantId, orderBy: x=>x.CourseId, trackChanges ,descending: false, cancellationToken: cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmApplicantCourse records by parent ID asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmApplicantCourse>> CrmApplicantCoursesByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
	{
		string query = $"SELECT * FROM CrmApplicantCourse WHERE ParentId = {parentId} ORDER BY ApplicantCourseId";
		return await AdoExecuteListQueryAsync<CrmApplicantCourse>(query, null, cancellationToken);
	}

	/// <summary>
	/// Creates a new CrmApplicantCourse record.
	/// </summary>
	public async Task<CrmApplicantCourse> CreateCrmApplicantCourse(CrmApplicantCourse entity, CancellationToken cancellationToken = default)
	{
		var newId = await CreateAndIdAsync(entity, cancellationToken);
		entity.ApplicantCourseId = newId;
		return entity;
	}

	/// <summary>
	/// Updates an existing CrmApplicantCourse record.
	/// </summary>
	public void UpdateCrmApplicantCourse(CrmApplicantCourse entity) => UpdateByState(entity);

	/// <summary>
	/// Deletes a CrmApplicantCourse record.
	/// </summary>
	public async Task DeleteCrmApplicantCourse(CrmApplicantCourse entity, bool trackChanges, CancellationToken cancellationToken = default)
	=> await DeleteAsync(x => x.ApplicantId.Equals(entity.ApplicantId));
}
