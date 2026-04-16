using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IDocumentTypeService
{
    Task<DocumentTypeDto> CreateAsync(CreateDocumentTypeRecord record, CancellationToken cancellationToken = default);
    Task<DocumentTypeDto> UpdateAsync(UpdateDocumentTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteDocumentTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<DocumentTypeDto> DocumentTypeAsync(int documenttypeid, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DocumentTypeDto>> DocumentTypesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DocumentTypeDDLDto>> DocumentTypesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<DocumentTypeDto>> DocumentTypesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
