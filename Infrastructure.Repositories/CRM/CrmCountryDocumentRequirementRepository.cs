using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmCountryDocumentRequirementRepository : RepositoryBase<CrmCountryDocumentRequirement>, ICrmCountryDocumentRequirementRepository
{
    public CrmCountryDocumentRequirementRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmCountryDocumentRequirement>> CrmCountryDocumentRequirementsAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.RequirementId, trackChanges, cancellationToken);

    public async Task<CrmCountryDocumentRequirement?> CrmCountryDocumentRequirementAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.RequirementId == id, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmCountryDocumentRequirement>> RequirementsByCountryIdAsync(int countryId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByIdsAsync(x => x.CountryId == countryId, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmCountryDocumentRequirement>> CrmCountryDocumentRequirementsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByIdsAsync(x => ids.Contains(x.RequirementId), trackChanges, cancellationToken);
}
