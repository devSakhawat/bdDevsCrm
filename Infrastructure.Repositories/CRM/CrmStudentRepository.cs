//using Domain.Entities.Entities.CRM;
//using Domain.Contracts.CRM;
//using bdDevs.Shared.DataTransferObjects.CRM;
//using Infrastructure.Sql.Context;

//namespace Infrastructure.Repositories.CRM;

///// <summary>
///// Repository for CrmStudent data access operations.
///// Implements enterprise patterns with async support and raw SQL capabilities.
///// </summary>
//public class CrmStudentRepository : RepositoryBase<stud>, ICrmStudentRepository
//{
//    public CrmStudentRepository(CrmContext context) : base(context) { }

//    /// <summary>
//    /// Retrieves all CrmStudent records asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmStudent>> CrmStudentsAsync(bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        return await ListAsync(x => x.CrmStudentId, trackChanges, cancellationToken);
//    }

//    /// <summary>
//    /// Retrieves a single CrmStudent record by ID asynchronously.
//    /// </summary>
//    public async Task<CrmStudent?> CrmStudentAsync(int crmstudentid, bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        return await FirstOrDefaultAsync(
//            x => x.CrmStudentId.Equals(crmstudentid), 
//            trackChanges, 
//            cancellationToken);
//    }

//    /// <summary>
//    /// Retrieves CrmStudent records by a collection of IDs asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmStudent>> CrmStudentsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        return await ListByIdsAsync(x => ids.Contains(x.CrmStudentId), trackChanges, cancellationToken);
//    }

//    /// <summary>
//    /// Retrieves CrmStudent records by parent ID asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmStudent>> CrmStudentsByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
//    {
//        string query = $"SELECT * FROM CrmStudent WHERE ParentId = {parentId} ORDER BY CrmStudentId";
//        return await AdoExecuteListQueryAsync<CrmStudent>(query, null, cancellationToken);
//    }

//    /// <summary>
//    /// Creates a new CrmStudent record.
//    /// </summary>
//    public void CreateCrmStudent(CrmStudent entity) => Create(entity);

//    /// <summary>
//    /// Updates an existing CrmStudent record.
//    /// </summary>
//    public void UpdateCrmStudent(CrmStudent entity) => UpdateByState(entity);

//    /// <summary>
//    /// Deletes a CrmStudent record.
//    /// </summary>
//    public void DeleteCrmStudent(CrmStudent entity) => Delete(entity);
//}
