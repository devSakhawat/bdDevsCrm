// IDmsDocumentService.cs
using bdDevs.Shared.DataTransferObjects.DMS;
using Application.Shared.Grid;
using Microsoft.AspNetCore.Http;

namespace Domain.Contracts.Services.DMS;

/// <summary>
/// Service contract for DMS document management operations.
/// </summary>
public interface IDmsDocumentService
{
	Task<IEnumerable<DmsDocumentDDL>> DocumentsDDLAsync(bool trackChanges, CancellationToken cancellationToken = default);
	Task<GridEntity<DmsDocumentDto>> DocumentsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
	Task<DmsDocumentDto> CreateDocumentAsync(DmsDocumentDto entityForCreate, CancellationToken cancellationToken = default);
	Task<DmsDocumentDto> UpdateDocumentAsync(int documentId, DmsDocumentDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);
	Task<int> DeleteDocumentAsync(int documentId, bool trackChanges, CancellationToken cancellationToken = default);
	Task<DmsDocumentDto> DocumentAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
	Task<IEnumerable<DmsDocumentDto>> DocumentsAsync(bool trackChanges, CancellationToken cancellationToken = default);
	Task<string> SaveFileAndDocumentWithAllDmsAsync(IFormFile file, string allAboutDMS, CancellationToken cancellationToken = default);
}


//using bdDevs.Shared.DataTransferObjects.DMS;
//using Application.Shared.Grid;
//using Microsoft.AspNetCore.Http;

//namespace Domain.Contracts.Services.DMS;

//public interface IDmsDocumentService
//{
//	Task<IEnumerable<DmsDocumentDDL>> DocumentsDDLAsync(bool trackChanges);
//	Task<GridEntity<DmsDocumentDto>> SummaryGrid(GridOptions options);
//	Task<string> CreateNewRecordAsync(DmsDocumentDto modelDto);
//	Task<string> UpdateNewRecordAsync(int key, DmsDocumentDto modelDto, bool trackChanges);
//	Task<string> DeleteRecordAsync(int key, DmsDocumentDto modelDto);
//	Task<string> SaveOrUpdate(int key, DmsDocumentDto modelDto);
//	//// single record
//	//Task<DmsDocument> ExistingDocumentAsync(string entityId, string entityType, string documentType);

//	Task<string> SaveFileAndDocumentWithAllDmsAsync(IFormFile file, string allAboutDMS);
//}