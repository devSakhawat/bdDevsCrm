using Domain.Contracts.CRM;
using Domain.Entities.Entities.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmCourse data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmCourseRepository : RepositoryBase<CrmCourse>, ICrmCourseRepository
{
	public CrmCourseRepository(CrmContext context) : base(context) { }

	/// <summary>
	/// Retrieves all CrmCourse records asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmCourse>> CrmCoursesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(x => x.CourseId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a single CrmCourse record by ID asynchronously.
	/// </summary>
	public async Task<CrmCourse?> CrmCourseAsync(int CourseId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(
				x => x.CourseId.Equals(CourseId),
				trackChanges,
				cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmCourse records by a collection of IDs asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmCourse>> CrmCoursesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(x => ids.Contains(x.CourseId), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmCourse records by parent ID asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmCourse>> CrmCoursesByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
	{
		string query = $"SELECT * FROM CrmCourse WHERE ParentId = {parentId} ORDER BY CourseId";
		return await AdoExecuteListQueryAsync<CrmCourse>(query, null, cancellationToken);
	}

	/// <summary>
	/// Creates a new CrmCourse record.
	/// </summary>
	public async Task<CrmCourse> CreateCrmCourse(CrmCourse entity, CancellationToken cancellationToken = default)
	{
		var newId = await CreateAndIdAsync(entity,cancellationToken);
		entity.CourseId = newId;
		return entity;
	}
	/// <summary>
	/// Updates an existing CrmCourse record.
	/// </summary>
	public void UpdateCrmCourse(CrmCourse entity) => UpdateByState(entity);

	/// <summary>
	/// Deletes a CrmCourse record.
	/// </summary>
	public async Task DeleteCrmCourse(CrmCourse entity, bool trackChanges, CancellationToken cancellationToken = default)
		=> await DeleteAsync(x => x.CourseId.Equals(entity.CourseId), trackChanges, cancellationToken);
}
