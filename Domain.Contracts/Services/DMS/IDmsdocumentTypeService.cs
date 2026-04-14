// IDmsDocumentTypeService.cs
using bdDevCRM.Shared.DataTransferObjects.DMS;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.DMS;

/// <summary>
/// Service contract for DMS document type management operations.
/// </summary>
public interface IDmsDocumentTypeService
{
	Task<IEnumerable<DmsDocumentTypeDDL>> TypesDDLAsync(bool trackChanges, CancellationToken cancellationToken = default);
	Task<GridEntity<DmsDocumentTypeDto>> TypesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
	Task<DmsDocumentTypeDto> CreateDocumentTypeAsync(DmsDocumentTypeDto entityForCreate, CancellationToken cancellationToken = default);
	Task<DmsDocumentTypeDto> UpdateDocumentTypeAsync(int documentTypeId, DmsDocumentTypeDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);
	Task<int> DeleteDocumentTypeAsync(int documentTypeId, bool trackChanges, CancellationToken cancellationToken = default);
	Task<DmsDocumentTypeDto> DocumentTypeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
	Task<IEnumerable<DmsDocumentTypeDto>> DocumentTypesAsync(bool trackChanges, CancellationToken cancellationToken = default);
}


//using Application.Shared.Grid;
//using bdDevCRM.Shared.DataTransferObjects.DMS;

//namespace bdDevCRM.ServiceContract.DMS;

//public interface IDmsDocumentTypeService
//{
//  Task<IEnumerable<DmsDocumentTypeDDL>> TypesDDLAsync(bool trackChanges);
//  Task<GridEntity<DmsDocumentTypeDto>> SummaryGrid(GridOptions options);
//  Task<string> CreateNewRecordAsync(DmsDocumentTypeDto modelDto);
//  Task<string> UpdateNewRecordAsync(int key, DmsDocumentTypeDto modelDto, bool trackChanges);
//  Task<string> DeleteRecordAsync(int key, DmsDocumentTypeDto modelDto);
//  Task<string> SaveOrUpdate(int key, DmsDocumentTypeDto modelDto);
//}
