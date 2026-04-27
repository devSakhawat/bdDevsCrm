using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>Repository for CrmStudent data access operations.</summary>
public class CrmStudentRepository : RepositoryBase<CrmStudent>, ICrmStudentRepository
{
    public CrmStudentRepository(CrmContext context) : base(context) { }

    /// <summary>Retrieves all CrmStudent records asynchronously.</summary>
    public async Task<IEnumerable<CrmStudent>> CrmStudentsAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.StudentId, trackChanges, cancellationToken);

    /// <summary>Retrieves a single CrmStudent record by ID asynchronously.</summary>
    public async Task<CrmStudent?> CrmStudentAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.StudentId.Equals(studentId), trackChanges, cancellationToken);

    /// <summary>Retrieves CrmStudent records by a collection of IDs asynchronously.</summary>
    public async Task<IEnumerable<CrmStudent>> CrmStudentsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByIdsAsync(x => ids.Contains(x.StudentId), trackChanges, cancellationToken);
}
