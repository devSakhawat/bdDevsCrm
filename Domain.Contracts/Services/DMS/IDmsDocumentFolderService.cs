// IDmsDocumentFolderService.cs
using bdDevs.Shared.DataTransferObjects.DMS;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.DMS;

/// <summary>
/// Service contract for DMS document folder management operations.
/// Defines methods for creating, updating, deleting, and retrieving document folder data.
/// </summary>
public interface IDmsDocumentFolderService
{
	/// <summary>
	/// Retrieves all document folder records from the database.
	/// </summary>
	Task<IEnumerable<DmsDocumentFolderDto>> FoldersAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a lightweight list of all document folders suitable for use in dropdown lists.
	/// </summary>
	Task<IEnumerable<DmsDocumentFolderDDL>> FoldersDDLAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all document folders.
	/// </summary>
	Task<GridEntity<DmsDocumentFolderDto>> FoldersSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single document folder record by its ID.
	/// </summary>
	Task<DmsDocumentFolderDto> FolderAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates a new document folder record.
	/// </summary>
	Task<DmsDocumentFolderDto> CreateFolderAsync(DmsDocumentFolderDto entityForCreate, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing document folder record.
	/// </summary>
	Task<DmsDocumentFolderDto> UpdateFolderAsync(int folderId, DmsDocumentFolderDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes a document folder record identified by the given ID.
	/// </summary>
	Task<int> DeleteFolderAsync(int folderId, bool trackChanges, CancellationToken cancellationToken = default);
}



//using bdDevs.Shared.DataTransferObjects.DMS;
//using Application.Shared.Grid;

//namespace Domain.Contracts.Services.DMS;

//public interface IDmsDocumentFolderService
//{
//	Task<IEnumerable<DmsDocumentFolderDDL>> FoldersDDLAsync(bool trackChanges);
//	Task<GridEntity<DmsDocumentFolderDto>> SummaryGrid(GridOptions options);
//	Task<string> CreateNewRecordAsync(DmsDocumentFolderDto modelDto);
//	Task<string> UpdateNewRecordAsync(int key, DmsDocumentFolderDto modelDto, bool trackChanges);
//	Task<string> DeleteRecordAsync(int key, DmsDocumentFolderDto modelDto);
//	Task<string> SaveOrUpdate(int key, DmsDocumentFolderDto modelDto);
//}
