using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using bdDevs.Shared.DataTransferObjects.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmWorkExperience data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmWorkExperienceRepository : RepositoryBase<CrmWorkExperience>, ICrmWorkExperienceRepository
{
    public CrmWorkExperienceRepository(CrmContext context) : base(context) { }

    /// <summary>
    /// Retrieves all CrmWorkExperience records asynchronously.
    /// </summary>
    public async Task<IEnumerable<CrmWorkExperience>> CrmWorkExperiencesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(x => x.WorkExperienceId, trackChanges, cancellationToken);
    }

    /// <summary>
    /// Retrieves a single CrmWorkExperience record by ID asynchronously.
    /// </summary>
    public async Task<CrmWorkExperience?> CrmWorkExperienceAsync(int crmworkexperienceid, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(
            x => x.WorkExperienceId.Equals(crmworkexperienceid), 
            trackChanges, 
            cancellationToken);
    }

    /// <summary>
    /// Retrieves CrmWorkExperience records by a collection of IDs asynchronously.
    /// </summary>
    public async Task<IEnumerable<CrmWorkExperience>> CrmWorkExperiencesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByIdsAsync(x => ids.Contains(x.WorkExperienceId), trackChanges, cancellationToken);
    }

    /// <summary>
    /// Retrieves CrmWorkExperience records by a applicantId asynchronously.
    /// </summary>
    public async Task<IEnumerable<CrmWorkExperience>> CrmWorkExperiencesByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByIdsAsync(x => x.ApplicantId == applicantId, trackChanges, cancellationToken);
    }

    /// <summary>
    /// Retrieves CrmWorkExperience records by parent ID asynchronously.
    /// </summary>
    public async Task<IEnumerable<CrmWorkExperience>> CrmWorkExperiencesByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
    {
        string query = $"SELECT * FROM CrmWorkExperience WHERE ParentId = {parentId} ORDER BY WorkExperienceId";
        return await AdoExecuteListQueryAsync<CrmWorkExperience>(query, null, cancellationToken);
    }

    /// <summary>
    /// Creates a new CrmWorkExperience record.
    /// </summary>
    public async Task<CrmWorkExperience> CreateCrmWorkExperienceAsync(CrmWorkExperience entity, CancellationToken cancellationToken = default)
    {
        var newId = await CreateAndIdAsync(entity, cancellationToken);
        entity.WorkExperienceId = newId;
        return entity;
    }

    /// <summary>
    /// Updates an existing CrmWorkExperience record.
    /// </summary>
    public void UpdateCrmWorkExperience(CrmWorkExperience entity) => UpdateByState(entity);

    /// <summary>
    /// Deletes a CrmWorkExperience record.
    /// </summary>
    public async Task DeleteCrmWorkExperienceAsync(CrmWorkExperience entity, bool trackChanges, CancellationToken cancellationToken = default)
    {
        await DeleteAsync(x => x.WorkExperienceId == entity.WorkExperienceId, trackChanges, cancellationToken);
    }
}
