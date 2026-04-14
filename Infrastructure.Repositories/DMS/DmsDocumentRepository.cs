using Domain.Entities.Entities.DMS;
using Domain.Contracts.DMS;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.DMS;

/// <summary>
/// Repository implementation for DmsDocument entity operations.
/// </summary>
public class DmsDocumentRepository : RepositoryBase<DmsDocument>, IDmsDocumentRepository
{
	public DmsDocumentRepository(CRMContext context) : base(context) { }

	/// <summary>
	/// s all documents ordered by DocumentId.
	/// </summary>
	public async Task<IEnumerable<DmsDocument>> DocumentsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(x => x.DocumentId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// s a single document by ID.
	/// </summary>
	public async Task<DmsDocument?> DocumentAsync(int documentId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(x => x.DocumentId == documentId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Creates a new document.
	/// </summary>
	public async Task<DmsDocument> CreateDocumentAsync(DmsDocument document, CancellationToken cancellationToken = default)
	{
		var newId = await CreateAndIdAsync(document, cancellationToken);
		document.DocumentId = newId;
		return document;
	}

	/// <summary>
	/// Updates an existing document.
	/// </summary>
	public void UpdateDocument(DmsDocument document) => UpdateByState(document);

	/// <summary>
	/// Deletes a document.
	/// </summary>
	public async Task DeleteDocumentAsync(DmsDocument document, bool trackChanges, CancellationToken cancellationToken = default)
	{
		await DeleteAsync(x => x.DocumentId == document.DocumentId, trackChanges, cancellationToken);
	}
}



//using Domain.Entities.Entities.DMS;
//using Domain.Contracts.DMS;
//using Infrastructure.Sql.Context;

//namespace Infrastructure.Repositories.DMS;

//public class DmsDocumentRepository : RepositoryBase<DmsDocument>, IDmsDocumentRepository
//{
//  public DmsDocumentRepository(CRMContext context) : base(context) { }

//  //  all documents ordered by DocumentId
//  public async Task<IEnumerable<DmsDocument>> AllDocumentsAsync(bool trackChanges) =>
//      await ListAsync(x => x.DocumentId, trackChanges);

//  //  a single document by DocumentId
//  public async Task<DmsDocument> DocumentByIdAsync(int documentId, bool trackChanges) =>
//      await FirstOrDefaultAsync(x => x.DocumentId == documentId, trackChanges);

//  // Add a new document
//  public void CreateDocument(DmsDocument document) => Create(document);

//  // Update an existing document
//  public void UpdateDocument(DmsDocument document) => UpdateByState(document);

//  // Delete a document
//  public void DeleteDocument(DmsDocument document) => Delete(document);
//}



