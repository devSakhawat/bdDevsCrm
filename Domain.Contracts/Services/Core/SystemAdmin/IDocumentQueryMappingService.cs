using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IDocumentQueryMappingService
{
    Task<DocumentQueryMappingDto> CreateAsync(CreateDocumentQueryMappingRecord record, CancellationToken cancellationToken = default);
    Task<DocumentQueryMappingDto> UpdateAsync(UpdateDocumentQueryMappingRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteDocumentQueryMappingRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<DocumentQueryMappingDto> DocumentQueryMappingAsync(int documentQueryId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DocumentQueryMappingDto>> DocumentQueryMappingsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DocumentQueryMappingDDLDto>> DocumentQueryMappingsForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<DocumentQueryMappingDto>> DocumentQueryMappingsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
