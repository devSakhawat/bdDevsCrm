// IDmsDocumentVersionService.cs
using bdDevs.Shared.DataTransferObjects.DMS;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.DMS;

/// <summary>
/// Service contract for DMS document version management operations.
/// </summary>
public interface IDmsDocumentVersionService
{
	Task<IEnumerable<DmsDocumentVersionDDL>> VersionsDDLAsync(bool trackChanges, CancellationToken cancellationToken = default);
	Task<GridEntity<DmsDocumentVersionDto>> VersionsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
	Task<DmsDocumentVersionDto> CreateDocumentVersionAsync(DmsDocumentVersionDto entityForCreate, CancellationToken cancellationToken = default);
	Task<DmsDocumentVersionDto> UpdateDocumentVersionAsync(int versionId, DmsDocumentVersionDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);
	Task<int> DeleteDocumentVersionAsync(int versionId, bool trackChanges, CancellationToken cancellationToken = default);
	Task<DmsDocumentVersionDto> DocumentVersionAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
	Task<IEnumerable<DmsDocumentVersionDto>> DocumentVersionsAsync(bool trackChanges, CancellationToken cancellationToken = default);
}



//using bdDevs.Shared.DataTransferObjects.DMS;
//using Application.Shared.Grid;

//namespace Domain.Contracts.Services.DMS;

//public interface IDmsDocumentVersionService
//{
//	Task<IEnumerable<DmsDocumentVersionDDL>> VersionsDDLAsync(bool trackChanges);
//	Task<GridEntity<DmsDocumentVersionDto>> SummaryGrid(GridOptions options);
//	Task<string> CreateNewRecordAsync(DmsDocumentVersionDto modelDto);
//	Task<string> UpdateNewRecordAsync(int key, DmsDocumentVersionDto modelDto, bool trackChanges);
//	Task<string> DeleteRecordAsync(int key, DmsDocumentVersionDto modelDto);
//	Task<string> SaveOrUpdate(int key, DmsDocumentVersionDto modelDto);
//}
