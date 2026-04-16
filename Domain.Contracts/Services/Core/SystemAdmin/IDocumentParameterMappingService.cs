using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IDocumentParameterMappingService
{
    Task<DocumentParameterMappingDto> CreateAsync(CreateDocumentParameterMappingRecord record, CancellationToken cancellationToken = default);
    Task<DocumentParameterMappingDto> UpdateAsync(UpdateDocumentParameterMappingRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteDocumentParameterMappingRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<DocumentParameterMappingDto> DocumentParameterMappingAsync(int mappingId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DocumentParameterMappingDto>> DocumentParameterMappingsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DocumentParameterMappingDDLDto>> DocumentParameterMappingsForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<DocumentParameterMappingDto>> DocumentParameterMappingsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
