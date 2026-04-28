using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmCountryDocumentRequirementService
{
    Task<CrmCountryDocumentRequirementDto> CreateAsync(CreateCrmCountryDocumentRequirementRecord record, CancellationToken cancellationToken = default);
    Task<CrmCountryDocumentRequirementDto> UpdateAsync(UpdateCrmCountryDocumentRequirementRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmCountryDocumentRequirementRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmCountryDocumentRequirementDto> CountryDocumentRequirementAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCountryDocumentRequirementDto>> CountryDocumentRequirementsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCountryDocumentRequirementDto>> RequirementsByCountryIdAsync(int countryId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<CrmCountryDocumentRequirementDto>> CountryDocumentRequirementsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
