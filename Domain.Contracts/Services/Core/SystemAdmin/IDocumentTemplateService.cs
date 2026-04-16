using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IDocumentTemplateService
{
    Task<DocumentTemplateDto> CreateAsync(CreateDocumentTemplateRecord record, CancellationToken cancellationToken = default);
    Task<DocumentTemplateDto> UpdateAsync(UpdateDocumentTemplateRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteDocumentTemplateRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<DocumentTemplateDto> DocumentTemplateAsync(int documentId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DocumentTemplateDto>> DocumentTemplatesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DocumentTemplateDDLDto>> DocumentTemplatesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<DocumentTemplateDto>> DocumentTemplatesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
