using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>Repository for CrmEnquiry data access operations.</summary>
public class CrmEnquiryRepository : RepositoryBase<CrmEnquiry>, ICrmEnquiryRepository
{
    public CrmEnquiryRepository(CrmContext context) : base(context) { }

    /// <summary>Retrieves all CrmEnquiry records asynchronously.</summary>
    public async Task<IEnumerable<CrmEnquiry>> CrmEnquiriesAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.EnquiryId, trackChanges, cancellationToken);

    /// <summary>Retrieves a single CrmEnquiry record by ID asynchronously.</summary>
    public async Task<CrmEnquiry?> CrmEnquiryAsync(int enquiryId, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.EnquiryId.Equals(enquiryId), trackChanges, cancellationToken);

    /// <summary>Retrieves CrmEnquiry records by a collection of IDs asynchronously.</summary>
    public async Task<IEnumerable<CrmEnquiry>> CrmEnquiriesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByIdsAsync(x => ids.Contains(x.EnquiryId), trackChanges, cancellationToken);
}
