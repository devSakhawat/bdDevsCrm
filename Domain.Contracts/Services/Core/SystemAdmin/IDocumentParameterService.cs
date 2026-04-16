using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IDocumentParameterService
{
    Task<DocumentParameterDto> CreateAsync(CreateDocumentParameterRecord record, CancellationToken cancellationToken = default);
    Task<DocumentParameterDto> UpdateAsync(UpdateDocumentParameterRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteDocumentParameterRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<DocumentParameterDto> DocumentParameterAsync(int parameterId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DocumentParameterDto>> DocumentParametersAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DocumentParameterDDLDto>> DocumentParametersForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<DocumentParameterDto>> DocumentParametersSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
