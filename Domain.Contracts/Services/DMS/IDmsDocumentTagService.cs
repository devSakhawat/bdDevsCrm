// IDmsDocumentTagService.cs
using bdDevs.Shared.DataTransferObjects.DMS;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.DMS;

/// <summary>
/// Service contract for DMS document tag management operations.
/// </summary>
public interface IDmsDocumentTagService
{
	Task<IEnumerable<DmsDocumentTagDDL>> TagsDDLAsync(bool trackChanges, CancellationToken cancellationToken = default);
	Task<GridEntity<DmsDocumentTagDto>> TagsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
	Task<DmsDocumentTagDto> CreateTagAsync(DmsDocumentTagDto entityForCreate, CancellationToken cancellationToken = default);
	Task<DmsDocumentTagDto> UpdateTagAsync(int tagId, DmsDocumentTagDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);
	Task<int> DeleteTagAsync(int tagId, bool trackChanges, CancellationToken cancellationToken = default);
	Task<DmsDocumentTagDto> TagAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
	Task<IEnumerable<DmsDocumentTagDto>> TagsAsync(bool trackChanges, CancellationToken cancellationToken = default);
}



//using Application.Shared.Grid;
//using bdDevs.Shared.DataTransferObjects.DMS;

//namespace Domain.Contracts.Services.DMS;


//public interface IDmsDocumentTagService
//{
//  Task<IEnumerable<DmsDocumentTagDDL>> TagsDDLAsync(bool trackChanges);
//  Task<GridEntity<DmsDocumentTagDto>> SummaryGrid(GridOptions options);
//  Task<string> CreateNewRecordAsync(DmsDocumentTagDto modelDto);
//  Task<string> UpdateNewRecordAsync(int key, DmsDocumentTagDto modelDto, bool trackChanges);
//  Task<string> DeleteRecordAsync(int key, DmsDocumentTagDto modelDto);
//  Task<string> SaveOrUpdate(int key, DmsDocumentTagDto modelDto);
//}
