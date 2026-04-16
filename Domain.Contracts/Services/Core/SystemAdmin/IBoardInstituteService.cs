using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IBoardInstituteService
{
    Task<BoardInstituteDto> CreateAsync(CreateBoardInstituteRecord record, CancellationToken cancellationToken = default);
    Task<BoardInstituteDto> UpdateAsync(UpdateBoardInstituteRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteBoardInstituteRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<BoardInstituteDto> BoardInstituteAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<BoardInstituteDto>> BoardInstitutesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<BoardInstituteDDLDto>> BoardInstitutesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<BoardInstituteDto>> BoardInstitutesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
