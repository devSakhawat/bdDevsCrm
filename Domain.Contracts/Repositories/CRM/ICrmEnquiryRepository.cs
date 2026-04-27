using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmEnquiryRepository : IRepositoryBase<CrmEnquiry>
{
    /// <summary>Retrieves all CrmEnquiry records asynchronously.</summary>
    Task<IEnumerable<CrmEnquiry>> CrmEnquiriesAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a single CrmEnquiry record by ID asynchronously.</summary>
    Task<CrmEnquiry?> CrmEnquiryAsync(int enquiryId, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves CrmEnquiry records by a collection of IDs asynchronously.</summary>
    Task<IEnumerable<CrmEnquiry>> CrmEnquiriesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
