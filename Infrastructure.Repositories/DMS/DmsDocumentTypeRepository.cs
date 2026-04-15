using Domain.Entities.Entities.DMS;
using Domain.Contracts.DMS;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.DMS;

/// <summary>
/// Repository implementation for DmsDocumentType entity operations.
/// </summary>
public class DmsDocumentTypeRepository : RepositoryBase<DmsDocumentType>, IDmsDocumentTypeRepository
{
	public DmsDocumentTypeRepository(CrmContext context) : base(context) { }

	/// <summary>
	/// s all document types ordered by DocumentTypeId.
	/// </summary>
	public async Task<IEnumerable<DmsDocumentType>> DocumentTypesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(x => x.DocumentTypeId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// s a single document type by ID.
	/// </summary>
	public async Task<DmsDocumentType?> DocumentTypeAsync(int documentTypeId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(x => x.DocumentTypeId == documentTypeId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Creates a new document type.
	/// </summary>
	public async Task<DmsDocumentType> CreateDocumentTypeAsync(DmsDocumentType documentType, CancellationToken cancellationToken = default)
	{
		var newId = await CreateAndIdAsync(documentType, cancellationToken);
		documentType.DocumentTypeId = newId;
		return documentType;
	}

	/// <summary>
	/// Updates an existing document type.
	/// </summary>
	public void UpdateDocumentType(DmsDocumentType documentType) => UpdateByState(documentType);

	/// <summary>
	/// Deletes a document type.
	/// </summary>
	public async Task DeleteDocumentTypeAsync(DmsDocumentType documentType, bool trackChanges, CancellationToken cancellationToken = default)
	{
		await DeleteAsync(x => x.DocumentTypeId == documentType.DocumentTypeId, trackChanges, cancellationToken);
	}
}

//using Domain.Entities.Entities.DMS;
//using Domain.Contracts.DMS;
//using Infrastructure.Sql.Context;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Infrastructure.Repositories.DMS;

//public class DmsDocumentTypeRepository : RepositoryBase<DmsDocumentType>, IDmsDocumentTypeRepository
//{
//  public DmsDocumentTypeRepository(CrmContext context) : base(context) { }

//  //  all document types
//  public async Task<IEnumerable<DmsDocumentType>> AllDocumentTypesAsync(bool trackChanges) =>
//      await ListAsync(x => x.DocumentTypeId, trackChanges);

//  // Create document type
//  public void CreateDocumentType(DmsDocumentType type) => Create(type);

//  // Update document type
//  public void UpdateDocumentType(DmsDocumentType type) => UpdateByState(type);

//  // Delete document type
//  public void DeleteDocumentType(DmsDocumentType type) => Delete(type);
//}

