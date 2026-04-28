using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmCountryDocumentRequirementRepository : IRepositoryBase<CrmCountryDocumentRequirement>
{
    Task<IEnumerable<CrmCountryDocumentRequirement>> CrmCountryDocumentRequirementsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmCountryDocumentRequirement?> CrmCountryDocumentRequirementAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCountryDocumentRequirement>> RequirementsByCountryIdAsync(int countryId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCountryDocumentRequirement>> CrmCountryDocumentRequirementsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
