using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IDocumentService
{
    Task<DocumentDto> CreateAsync(CreateDocumentRecord record, CancellationToken cancellationToken = default);
    Task<DocumentDto> UpdateAsync(UpdateDocumentRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteDocumentRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<DocumentDto> DocumentAsync(int documentid, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DocumentDto>> DocumentsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DocumentDto>> DocumentsForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<DocumentDto>> DocumentsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
