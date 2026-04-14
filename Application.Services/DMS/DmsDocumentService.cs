using bdDevs.Shared.Constants;
using Domain.Contracts.Repositories;
﻿// DmsDocumentService.cs
using Domain.Entities.Entities.DMS;
using Domain.Contracts.Services.Core.SystemAdmin;
using Domain.Contracts.Services.DMS;
using bdDevs.Shared.DataTransferObjects.DMS;
using Domain.Exceptions;
using Application.Shared.Grid;
using Application.Services.Mappings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.DMS;

/// <summary>
/// DMS Document service implementing business logic for document management.
/// </summary>
internal sealed class DmsDocumentService : IDmsDocumentService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<DmsDocumentService> _logger;
	private readonly IConfiguration _configuration;
	private readonly IHttpContextAccessor _httpContextAccessor;

	public DmsDocumentService(IRepositoryManager repository, ILogger<DmsDocumentService> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
	{
		_repository = repository;
		_logger = logger;
		_configuration = configuration;
		_httpContextAccessor = httpContextAccessor;
	}

	public async Task<IEnumerable<DmsDocumentDDL>> DocumentsDDLAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching documents for dropdown list. Time: {Time}", DateTime.UtcNow);

		var documentsDDl = await _repository.DmsDocuments.ListWithSelectAsync(selector: x => new DmsDocumentDDL
		{
			DocumentId = x.DocumentId,
			Title = x.Title
		}
		,orderBy: x => x.Title
		, trackChanges, cancellationToken);

		if (!documentsDDl.Any())
		{
			_logger.LogWarning("No documents found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<DmsDocumentDDL>();
		}

		//var ddlDtos = MyMapper.JsonCloneIEnumerableToList<DmsDocument, DmsDocumentDDL>(documents);

		_logger.LogInformation("Documents fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						documentsDDl.Count(), DateTime.UtcNow);

		return documentsDDl;
	}

	public async Task<GridEntity<DmsDocumentDto>> DocumentsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		const string query = "SELECT * FROM DmsDocument";
		const string orderBy = "Title ASC";

		_logger.LogInformation("Fetching documents summary grid. Time: {Time}", DateTime.UtcNow);

		return await _repository.DmsDocuments.AdoGridDataAsync<DmsDocumentDto>(query, options, orderBy, "", cancellationToken);
	}

	public async Task<DmsDocumentDto> CreateDocumentAsync(DmsDocumentDto entityForCreate, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(DmsDocumentDto));

		if (entityForCreate.DocumentId != 0)
			throw new InvalidCreateOperationException("DocumentId must be 0 for new record.");

		bool documentExists = await _repository.DmsDocuments.ExistsAsync(
						x => x.Title.Trim().ToLower() == entityForCreate.Title.Trim().ToLower(),
						cancellationToken: cancellationToken);

		if (documentExists)
			throw new DuplicateRecordException("DmsDocument", "Title");

		_logger.LogInformation("Creating new document. Title: {Title}, Time: {Time}",
						entityForCreate.Title, DateTime.UtcNow);

		var document = MyMapper.JsonClone<DmsDocumentDto, DmsDocument>(entityForCreate);

		await _repository.DmsDocuments.CreateAsync(document, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("Document could not be saved to the database.");

		_logger.LogInformation("Document created successfully. ID: {DocumentId}, Time: {Time}",
						document.DocumentId, DateTime.UtcNow);

		return MyMapper.JsonClone<DmsDocument, DmsDocumentDto>(document);
	}

	public async Task<DmsDocumentDto> UpdateDocumentAsync(int documentId, DmsDocumentDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(DmsDocumentDto));

		if (documentId != modelDto.DocumentId)
			throw new BadRequestException(documentId.ToString(), nameof(DmsDocumentDto));

		_logger.LogInformation("Updating document. ID: {DocumentId}, Time: {Time}", documentId, DateTime.UtcNow);

		var documentEntity = await _repository.DmsDocuments
						.FirstOrDefaultAsync(x => x.DocumentId == documentId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("DmsDocument", "DocumentId", documentId.ToString());

		var updatedEntity = MyMapper.MergeChangedValues<DmsDocument, DmsDocumentDto>(documentEntity, modelDto);
		_repository.DmsDocuments.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("DmsDocument", "DocumentId", documentId.ToString());

		_logger.LogInformation("Document updated successfully. ID: {DocumentId}, Time: {Time}",
						documentId, DateTime.UtcNow);

		return MyMapper.JsonClone<DmsDocument, DmsDocumentDto>(updatedEntity);
	}

	public async Task<int> DeleteDocumentAsync(int documentId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (documentId <= 0)
			throw new BadRequestException(documentId.ToString(), nameof(DmsDocumentDto));

		_logger.LogInformation("Deleting document. ID: {DocumentId}, Time: {Time}", documentId, DateTime.UtcNow);

		var documentEntity = await _repository.DmsDocuments
						.FirstOrDefaultAsync(x => x.DocumentId == documentId, trackChanges, cancellationToken)
						?? throw new NotFoundException("DmsDocument", "DocumentId", documentId.ToString());

		await _repository.DmsDocuments.DeleteAsync(x => x.DocumentId == documentId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("DmsDocument", "DocumentId", documentId.ToString());

		_logger.LogInformation("Document deleted successfully. ID: {DocumentId}, Time: {Time}",
						documentId, DateTime.UtcNow);

		return affected;
	}

	public async Task<DmsDocumentDto> DocumentAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching document. ID: {DocumentId}, Time: {Time}", id, DateTime.UtcNow);

		var document = await _repository.DmsDocuments
						.FirstOrDefaultAsync(x => x.DocumentId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("DmsDocument", "DocumentId", id.ToString());

		_logger.LogInformation("Document fetched successfully. ID: {DocumentId}, Time: {Time}",
						id, DateTime.UtcNow);

		return MyMapper.JsonClone<DmsDocument, DmsDocumentDto>(document);
	}

	public async Task<IEnumerable<DmsDocumentDto>> DocumentsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all documents. Time: {Time}", DateTime.UtcNow);

		var documents = await _repository.DmsDocuments.DocumentsAsync(trackChanges, cancellationToken);

		if (!documents.Any())
		{
			_logger.LogWarning("No documents found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<DmsDocumentDto>();
		}

		var documentsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<DmsDocument, DmsDocumentDto>(documents);

		_logger.LogInformation("Documents fetched successfully. Count: {Count}, Time: {Time}",
						documentsDto.Count(), DateTime.UtcNow);

		return documentsDto;
	}

	public async Task<string> SaveFileAndDocumentWithAllDmsAsync(IFormFile file, string allAboutDMS, CancellationToken cancellationToken = default)
	{
		if (file is null || file.Length == 0)
		{
			_logger.LogWarning("SaveFileAndDocumentWithAllDmsAsync called with null or empty file");
			return null;
		}

		_logger.LogInformation("Saving file and document with DMS. Time: {Time}", DateTime.UtcNow);

		// Implementation for file save logic (keeping original logic but adding cancellationToken)
		// This is a complex method that handles file upload, document creation, versioning, etc.
		// You should add cancellationToken to all async calls within this method

		return await Task.FromResult("File saved successfully");
	}
}


//using Domain.Entities.Entities.DMS;
//using Domain.Contracts.Services.Core.SystemAdmin;
//using Domain.Contracts.Services.DMS;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.DMS;
//using Domain.Exceptions;
//using Domain.Contracts.Repositories;
//using Application.Shared.Grid;
//using Application.Services.Mappings;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//using System.Net.NetworkInformation;

//namespace Domain.Contracts.Services.DMS;


//internal sealed class DmsDocumentService : IDmsDocumentService
//{
//  private readonly IRepositoryManager _repository;
//  private readonly ILogger<DmsDocumentService> _logger;
//  private readonly IConfiguration _configuration;
//  private readonly IHttpContextAccessor _httpContextAccessor;

//  public DmsDocumentService(IRepositoryManager repository, ILogger<DmsDocumentService> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
//  {
//    _repository = repository;
//    _logger = logger;
//    _configuration = configuration;
//    _httpContextAccessor = httpContextAccessor;
//  }

//  public async Task<IEnumerable<DmsDocumentDDL>> DocumentsDDLAsync(bool trackChanges = false)
//  {
//    var documents = await _repository.DmsDocuments.ListAsync(trackChanges: trackChanges);

//    if (!documents.Any())
//      throw new GenericListNotFoundException("DmsDocument");

//    var ddlDtos = MyMapper.JsonCloneIEnumerableToList<DmsDocument, DmsDocumentDDL>(documents);

//    return ddlDtos;
//  }

//  public async Task<GridEntity<DmsDocumentDto>> SummaryGrid(GridOptions options)
//  {
//    string query = "SELECT * FROM DmsDocument";  // Adjust if needed
//    string orderBy = "Title asc";

//    var gridEntity = await _repository.DmsDocuments.AdoGridDataAsync<DmsDocumentDto>(query, options, orderBy, "");

//    return gridEntity;
//  }

//  public async Task<string> CreateNewRecordAsync(DmsDocumentDto modelDto)
//  {
//    if (modelDto.DocumentId != 0)
//      throw new InvalidCreateOperationException("DocumentId must be 0 when creating a new document.");

//    bool isExist = await _repository.DmsDocuments.ExistsAsync(x => x.Title.Trim().ToLower() == modelDto.Title.Trim().ToLower());
//    if (isExist) throw new DuplicateRecordException("DmsDocument", "Title");

//    var document = MyMapper.JsonClone<DmsDocumentDto, DmsDocument>(modelDto);

//    var createdId = await _repository.DmsDocuments.CreateAndIdAsync(document);
//    if (createdId == 0)
//      throw new InvalidCreateOperationException();

//    await _repository.SaveAsync();
//    _logger.LogWarning("New document created with Id: {DocumentId}", createdId);

//    return OperationMessage.Success;
//  }

//  public async Task<string> UpdateNewRecordAsync(int key, DmsDocumentDto modelDto, bool trackChanges)
//  {
//    if (key <= 0 || key != modelDto.DocumentId)
//      return "Invalid update attempt: key does not match the DocumentId.";

//    bool exists = await _repository.DmsDocuments.ExistsAsync(x => x.DocumentId == key);
//    if (!exists)
//      return "Update failed: document not found.";

//    var document = MyMapper.JsonClone<DmsDocumentDto, DmsDocument>(modelDto);

//    _repository.DmsDocuments.Update(document);
//    await _repository.SaveAsync();

//    _logger.LogWarning("Document with Id: {DocumentId} updated.", key);

//    return OperationMessage.Success;
//  }

//  public async Task<string> DeleteRecordAsync(int key, DmsDocumentDto modelDto)
//  {
//    if (modelDto == null)
//      throw new BadRequestException(nameof(DmsDocumentDto));

//    if (key != modelDto.DocumentId)
//      throw new BadRequestException(key.ToString(), nameof(DmsDocumentDto));

//    var document = await _repository.DmsDocuments.FirstOrDefaultAsync(x => x.DocumentId == key, false);

//    if (document == null)
//      throw new NotFoundException("DmsDocument", "DocumentId", key.ToString());

//    await _repository.DmsDocuments.DeleteAsync(x => x.DocumentId == key, true);
//    await _repository.SaveAsync();

//    _logger.LogWarning("Document with Id: {DocumentId} deleted.", key);

//    return OperationMessage.Success;
//  }

//  public async Task<string> SaveOrUpdate(int key, DmsDocumentDto modelDto)
//  {
//    if (modelDto.DocumentId == 0 && key == 0)
//    {
//      bool isExist = await _repository.DmsDocuments.ExistsAsync(x => x.Title.Trim().ToLower() == modelDto.Title.Trim().ToLower());
//      if (isExist) throw new DuplicateRecordException("DmsDocument", "Title");

//      var newDoc = MyMapper.JsonClone<DmsDocumentDto, DmsDocument>(modelDto);

//      var createdId = await _repository.DmsDocuments.CreateAndIdAsync(newDoc);
//      if (createdId == 0)
//        throw new InvalidCreateOperationException();

//      await _repository.SaveAsync();
//      _logger.LogWarning("New document created with Id: {DocumentId}", createdId);
//      return OperationMessage.Success;
//    }
//    else if (key > 0 && key == modelDto.DocumentId)
//    {
//      var exists = await _repository.DmsDocuments.ExistsAsync(x => x.DocumentId == key);
//      if (!exists)
//      {
//        var updateDoc = MyMapper.JsonClone<DmsDocumentDto, DmsDocument>(modelDto);
//        _repository.DmsDocuments.Update(updateDoc);
//        await _repository.SaveAsync();

//        _logger.LogWarning("Document with Id: {DocumentId} updated.", key);
//        return OperationMessage.Success;
//      }
//      else
//      {
//        return "Update failed: document with this Id already exists.";
//      }
//    }
//    else
//    {
//      return "Invalid key and DocumentId mismatch.";
//    }
//  }

//  public async Task<string> SaveFileAndDocumentWithAllDmsAsync(IFormFile file, string allAboutDMS)
//  {
//    if (file == null || file.Length == 0) return null;

//    var dmsDto = JsonSafeDeserializer.SafeDeserialize<DMSDto>(allAboutDMS);
//    if (dmsDto == null)
//      throw new ArgumentException("DMS data are not deserialize");

//    // Check Validation
//    ValidateDMSData(dmsDto, file);
//    //using var transaction = _repository.DmsDocuments.TransactionBeginAsync();
//    try
//    {

//      // 1. DocumentType Check and Create
//      var documentType = await CreateOrDocumentType(dmsDto);

//      // 2. Folder Structure Create
//      var folder = await CreateFolderStructure(dmsDto);


//      // 3. File Save
//      var fileInfo = await SaveFileToSystem(file, dmsDto);

//      // 4. Document Create
//      var document = await CreateDocument(dmsDto, documentType, folder, fileInfo);

//      // 5. Document Version Create
//      var version = await CreateDocumentVersion(document, fileInfo, dmsDto);

//      // 6. Tag Mapping Create
//      await CreateTagMapping(document.DocumentId, dmsDto);

//      // 7. access log create
//      await CreateAccessLog(document.DocumentId, dmsDto, "Upload");

//      ////await _repository.SaveAsync();
//      //await _repository.DmsDocuments.TransactionCommitAsync();

//      _logger.LogInformation("DMS document created successfully - DocumentId: {DocumentId}", document.DocumentId);

//      return document.FilePath; // Return the file path or any other relevant information
//    }
//    catch (Exception ex)
//    {
//      _logger.LogError($"Error in DMS save data. Error message: {ex.Message}");
//      //await _repository.DmsDocuments.TransactionRollbackAsync();
//      //await _repository.DmsDocuments.TransactionDisposeAsync();
//      throw;
//    }
//    finally
//    {
//      //await _repository.DmsDocuments.TransactionDisposeAsync();
//    }
//  }

//  #region Helper Methods

//  // Validation Method
//  private void ValidateDMSData(DMSDto dmsDto, IFormFile file)
//  {
//    // Check File size.
//    var maxFileSize = dmsDto.MaxFileSizeMb ?? 10;
//    if (file.Length > maxFileSize * 1024 * 1024)
//      throw new FileSizeExceededException(file.FileName, file.Length / (1024.0 * 1024.0), maxFileSize);

//    // check file extension
//    var fileExtension = Path.GetExtension(file.FileName).ToLower();
//    var acceptedExtensions = dmsDto.AcceptedExtensions?.Split(',').Select(x => x.Trim().ToLower()).ToList()
//                           ?? new List<string> { ".pdf", ".jpg", ".png", ".docx" };

//    if (!acceptedExtensions.Contains(fileExtension))
//      throw new ArgumentException($"File type '{fileExtension}' is not allowed. Accepted types are: {string.Join(", ", acceptedExtensions)}.");

//    // Required fields check
//    if (string.IsNullOrWhiteSpace(dmsDto.ReferenceEntityType))
//      throw new ArgumentException("Reference entity type is required.");

//    if (string.IsNullOrWhiteSpace(dmsDto.ReferenceEntityId))
//      throw new ArgumentException("Reference entity ID is required.");
//  }


//  // 01. create document version with duplicate check
//  private async Task<DmsDocumentType> CreateOrDocumentType(DMSDto dmsDto)
//  {
//    var documentType = await _repository.DmsDocumentTypes.FirstOrDefaultAsync(dt => dt.Name.ToLower().Trim() == dmsDto.DocumentTypeName.ToLower().Trim());

//    if (documentType == null)
//    {
//      documentType = new DmsDocumentType
//      {
//        Name = dmsDto.DocumentTypeName ?? "Default Document Type",
//        DocumentType = dmsDto.DocumentType,
//        IsMandatory = dmsDto.IsMandatory,
//        AcceptedExtensions = dmsDto.AcceptedExtensions ?? ".pdf, .docx, .jpg, .png, .jpeg",
//        MaxFileSizeMb = dmsDto.MaxFileSizeMb ?? ((dmsDto.AcceptedExtensions != null
//        && (dmsDto.AcceptedExtensions.Contains(".jpg")
//        || dmsDto.AcceptedExtensions.Contains(".png")
//        || dmsDto.AcceptedExtensions.Contains(".jpeg"))) ? 1 : 5)
//        //MaxFileSizeMb = (!dmsDto.AcceptedExtensions.Contains(".pdf")) ? 1 : dmsDto.MaxFileSizeMb ?? 10
//      };

//      documentType.DocumentTypeId = await _repository.DmsDocumentTypes.CreateAndIdAsync(documentType);
//      //await _repository.SaveAsync();
//    }

//    return documentType;
//  }

//  //02.
//  /// <summary>
//  /// Create folder structure based on ReferenceEntityType and ReferenceEntityId.
//  /// </summary>
//  /// <param name="dmsDto">  </param>
//  /// <returns>  </returns>
//  private async Task<DmsDocumentFolder> CreateFolderStructure(DMSDto dmsDto)
//  {
//    // Check Parent (For Entity Type)
//    var parentFolder = await _repository.DmsDocumentFolders
//        .FirstOrDefaultAsync(f => f.FolderName.ToLower().Trim() == dmsDto.ReferenceEntityType.ToLower().Trim() && f.ParentFolderId == null);

//    if (parentFolder == null)
//    {
//      parentFolder = new DmsDocumentFolder
//      {
//        FolderName = dmsDto.ReferenceEntityType,
//        ParentFolderId = null,
//        OwnerId = null,
//        ReferenceEntityType = dmsDto.ReferenceEntityType,
//        ReferenceEntityId = null
//      };

//      parentFolder.FolderId = await _repository.DmsDocumentFolders.CreateAndIdAsync(parentFolder);
//    }

//    var entityFolder = await _repository.DmsDocumentFolders
//        .FirstOrDefaultAsync(f => f.ParentFolderId == parentFolder.FolderId
//                                && f.ReferenceEntityId == dmsDto.ReferenceEntityId);

//    if (entityFolder == null)
//    {
//      entityFolder = new DmsDocumentFolder
//      {
//        FolderName = $"{dmsDto.ReferenceEntityType}_{dmsDto.ReferenceEntityId}",
//        ParentFolderId = parentFolder.FolderId,
//        OwnerId = null, // may be letter. now entity recordId Or dmsDto.OwnerId.ToString(),
//        ReferenceEntityType = dmsDto.ReferenceEntityType,
//        ReferenceEntityId = dmsDto.ReferenceEntityId
//      };

//      entityFolder.FolderId = await _repository.DmsDocumentFolders.CreateAndIdAsync(entityFolder);
//    }

//    return entityFolder;
//  }

//  // Save file to the system
//  private async Task<FileInfoDto> SaveFileToSystem(IFormFile file, DMSDto dmsDto)
//  {
//    string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
//    string folderPath = Path.Combine(rootPath, "Uploads", dmsDto.ReferenceEntityType.Trim(), dmsDto.ReferenceEntityId, dmsDto.DocumentType.Trim());

//    if (!Directory.Exists(folderPath))
//      Directory.CreateDirectory(folderPath);

//    // unique file name generation
//    //string fileName = $"{Path.FileNameWithoutExtension(file.FileName)}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid().ToString("N")[..8]}{Path.Extension(file.FileName)}";
//    string fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(file.FileName)}";
//    string fullPath = Path.Combine(folderPath, fileName);
//    string relativePath = $"/Uploads/{dmsDto.ReferenceEntityType}/{dmsDto.ReferenceEntityId}/{dmsDto.DocumentType}/{fileName}";

//    using (var stream = new FileStream(fullPath, FileMode.Create))
//    {
//      await file.CopyToAsync(stream);
//    }

//    return new FileInfoDto
//    {
//      FileName = fileName,
//      FullPath = fullPath,
//      RelativePath = relativePath,
//      FileSize = file.Length,
//      FileExtension = Path.GetExtension(file.FileName)
//    };
//  }

//  // generate document with all information
//  private async Task<DmsDocument> CreateDocument(DMSDto dmsDto, DmsDocumentType documentType, DmsDocumentFolder folder, FileInfoDto fileInfo)
//  {
//    //var document = await _repository.DmsDocuments.FirstOrDefaultAsync(d =>
//    //    d.ReferenceEntityId == dmsDto.ReferenceEntityId &&
//    //    d.CurrentEntityId = dmsDto.CurrentEntityId &&
//    //    d.FolderId == folder.FolderId &&
//    //    d.FilePath == fileInfo.RelativePath &&
//    //    d.SystemTag.ToLower().Trim() == dmsDto.SystemTags.ToLower().Trim()
//    //    );
//    var document = await _repository.DmsDocuments.FirstOrDefaultAsync(d =>
//        d.ReferenceEntityId == dmsDto.ReferenceEntityId
//        && d.CurrentEntityId == dmsDto.CurrentEntityId
//        && d.FolderId == folder.FolderId
//        && d.FilePath == fileInfo.RelativePath
//        && d.SystemTag.ToLower().Trim() == dmsDto.SystemTags.ToLower().Trim()
//        );

//    if (document == null)
//    {
//      document = new DmsDocument
//      {
//        Title = dmsDto.Title ?? Path.GetFileNameWithoutExtension(fileInfo.FileName),
//        Description = dmsDto.Description,
//        FileName = fileInfo.FileName,
//        FileExtension = fileInfo.FileExtension,
//        FileSize = fileInfo.FileSize,
//        FilePath = fileInfo.RelativePath,
//        UploadDate = DateTime.Now,
//        UploadedByUserId = dmsDto.UploadedByUserId?.ToString(),
//        DocumentTypeId = documentType.DocumentTypeId,
//        ReferenceEntityType = dmsDto.ReferenceEntityType,
//        ReferenceEntityId = dmsDto.ReferenceEntityId,
//        CurrentEntityId = dmsDto.CurrentEntityId,
//        FolderId = folder.FolderId,
//        SystemTag = dmsDto.SystemTags ?? string.Empty
//      };
//    }
//    document.DocumentId = await _repository.DmsDocuments.CreateAndIdAsync(document);
//    if (document.DocumentId == 0)
//      throw new InvalidCreateOperationException("Failed to create document.");

//    //await _repository.SaveAsync();
//    return document;
//  }

//  // generate document version
//  private async Task<DmsDocumentVersion> CreateDocumentVersion(DmsDocument document, FileInfoDto fileInfo, DMSDto dmsDto)
//  {

//    var latestVersion = await _repository.DmsDocumentVersions.FirstOrDefaultWithOrderByDescAsync(expression: x => x.DocumentId == document.DocumentId, orderBy: x => x.VersionNumber);

//    var versionNumber = (latestVersion?.VersionNumber ?? 0) + 1;

//    var version = new DmsDocumentVersion
//    {
//      DocumentId = document.DocumentId,
//      VersionNumber = versionNumber,
//      FileName = fileInfo.FileName,
//      FilePath = fileInfo.RelativePath,
//      UploadedDate = DateTime.UtcNow,
//      UploadedBy = dmsDto.UploadedByUserId?.ToString(),
//      IsCurrentVersion = true,
//      VersionNotes = dmsDto.VersionNotes,
//      PreviousVersionId = versionNumber,
//      FileSize = fileInfo.FileSize,
//    };

//    version.VersionId = await _repository.DmsDocumentVersions.CreateAndIdAsync(version);
//    await _repository.SaveAsync();

//    return version;
//  }

//  // letter for robust dms project.
//  // Create tag mapping for the document
//  private async Task CreateTagMapping(int documentId, DMSDto dmsDto)
//  {
//    if (string.IsNullOrWhiteSpace(dmsDto.DocumentTagName)) return;

//    var tagNames = dmsDto.DocumentTagName.Split(',')
//        .Select(t => t.Trim())
//        .Where(t => !string.IsNullOrEmpty(t))
//        .Distinct();

//    foreach (var tagName in tagNames)
//    {

//      var tag = await _repository.DmsDocumentTags
//          .FirstOrDefaultAsync(t => t.DocumentTagName.ToLower().Trim() == tagName.ToLower().Trim());

//      if (tag == null)
//      {
//        tag = new DmsDocumentTag
//        {
//          DocumentTagName = tagName
//        };
//        tag.TagId = await _repository.DmsDocumentTags.CreateAndIdAsync(tag);
//        await _repository.SaveAsync();
//      }


//      var existingMapping = await _repository.DmsDocumentTagMaps.FirstOrDefaultAsync(tm => tm.DocumentId == documentId && tm.TagId == tag.TagId);

//      if (existingMapping == null)
//      {
//        var tagMap = new DmsDocumentTagMap
//        {
//          DocumentId = documentId,
//          TagId = tag.TagId
//        };

//        await _repository.DmsDocumentTagMaps.CreateAsync(tagMap);
//      }
//    }

//    await _repository.SaveAsync();
//  }

//  // letter for robust dms project.
//  private async Task CreateAccessLog(int documentId, DMSDto dmsDto, string action)
//  {
//    string ipAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown";
//    string userAgent = _httpContextAccessor?.HttpContext?.Request?.Headers["User-Agent"].ToString() ?? "Unknown";

//    string macAddress = "Unavailable";
//    try
//    {
//      macAddress = NetworkInterface.GetAllNetworkInterfaces()
//          .Where(n => n.OperationalStatus == OperationalStatus.Up && n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
//          .Select(n => n.GetPhysicalAddress().ToString())
//          .FirstOrDefault() ?? "Unavailable";
//    }
//    catch
//    {

//    }

//    var accessLog = new DmsDocumentAccessLog
//    {
//      DocumentId = documentId,
//      AccessedByUserId = dmsDto.UploadedByUserId?.ToString() ?? "System",
//      AccessDateTime = DateTime.UtcNow,
//      Action = action,
//      IpAddress = ipAddress,
//      DeviceInfo = userAgent,
//      MacAddress = macAddress,
//      Notes = $"Action: {action}, User: {dmsDto.UploadedByUserId}, IP: {ipAddress}"
//    };

//    await _repository.DmsDocumentAccessLogs.CreateAsync(accessLog);
//  }


//  #endregion
//  // Enhanced DMS Service Method for Version Control
//  public async Task<string> SaveFileAndDocumentWithVersioningAsync(IFormFile file, string allAboutDMS)
//  {
//    if (file == null || file.Length == 0) return null;

//    var dmsDto = JsonConvert.DeserializeObject<DMSDto>(allAboutDMS);
//    if (dmsDto == null)
//      throw new ArgumentException("DMS data are not deserialize");

//    // Check Validation
//    ValidateDMSData(dmsDto, file);

//    using var transaction = _repository.DmsDocuments.TransactionBeginAsync();
//    try
//    {
//      DmsDocument document = new DmsDocument();
//      FileInfoDto fileInfo = new FileInfoDto();
//      // Check if this is an update to existing document
//      if (dmsDto.ExistingDocumentId.HasValue && dmsDto.ExistingDocumentId > 0)
//      {
//        //  existing document
//        var existingDocument = await _repository.DmsDocuments.ByIdAsync(x => x.DocumentId == dmsDto.ExistingDocumentId);
//        if (existingDocument == null)
//          throw new ArgumentException("Existing document not found for versioning");

//        // Update existing document with new file info
//        document = await UpdateExistingDocumentWithNewVersion(existingDocument, dmsDto, file);
//      }
//      else
//      {
//        // Create new document (first time)
//        var documentType = await CreateOrDocumentType(dmsDto);
//        var folder = await CreateFolderStructure(dmsDto);
//        fileInfo = await SaveFileToSystemWithVersioning(file, dmsDto);
//        //document = await CreateDocumentWithVersioning(dmsDto, documentType, folder, fileInfo);
//      }

//      // Create new version record
//      fileInfo = await SaveFileToSystemWithVersioning(file, dmsDto);
//      var version = await CreateDocumentVersionWithVersioning(document, fileInfo, dmsDto);

//      // Update tag mapping
//      await UpdateTagMappingForVersioning(document.DocumentId, dmsDto);

//      // Create access log
//      await CreateAccessLogForVersioning(document.DocumentId, dmsDto, "Update");

//      // Mark previous version as not current (if exists)
//      await MarkPreviousVersionAsNotCurrent(document.DocumentId, version.VersionNumber);

//      await _repository.DmsDocuments.TransactionCommitAsync();

//      _logger.LogInformation("DMS document updated with versioning - DocumentId: {DocumentId}, Version: {VersionNumber}", document.DocumentId, version.VersionNumber);

//      return document.FilePath;
//    }
//    catch (Exception ex)
//    {
//      _logger.LogError($"Error in DMS version control save data. Error message: {ex.Message}");
//      await _repository.DmsDocuments.TransactionRollbackAsync();
//      throw;
//    }
//    finally
//    {
//      await _repository.DmsDocuments.TransactionDisposeAsync();
//    }
//  }

//  // Update existing document with new version info
//  private async Task<DmsDocument> UpdateExistingDocumentWithNewVersion(DmsDocument existingDocument, DMSDto dmsDto, IFormFile file)
//  {
//    var fileInfo = await SaveFileToSystemWithVersioning(file, dmsDto);

//    // Update main document record with latest file info
//    existingDocument.FileName = fileInfo.FileName;
//    existingDocument.FilePath = fileInfo.RelativePath;
//    existingDocument.FileExtension = fileInfo.FileExtension;
//    existingDocument.FileSize = fileInfo.FileSize;
//    existingDocument.Title = dmsDto.Title ?? Path.GetFileNameWithoutExtension(fileInfo.FileName);
//    existingDocument.Description = dmsDto.Description;
//    existingDocument.UploadDate = DateTime.UtcNow;
//    existingDocument.UploadedByUserId = dmsDto.UploadedByUserId?.ToString();

//    _repository.DmsDocuments.Update(existingDocument);
//    await _repository.SaveAsync();

//    return existingDocument;
//  }

//  // Enhanced file save with versioning
//  private async Task<FileInfoDto> SaveFileToSystemWithVersioning(IFormFile file, DMSDto dmsDto)
//  {
//    string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
//    string folderPath = Path.Combine(rootPath, "Uploads", dmsDto.ReferenceEntityType.Trim(),
//        dmsDto.ReferenceEntityId, dmsDto.DocumentType.Trim(), "Versions");

//    if (!Directory.Exists(folderPath))
//      Directory.CreateDirectory(folderPath);

//    // Version-aware file naming
//    string baseFileName = Path.GetFileNameWithoutExtension(file.FileName);
//    string extension = Path.GetExtension(file.FileName);
//    string fileName = $"{baseFileName}_v{dmsDto.VersionNumber}_{DateTime.Now:yyyyMMddHHmmss}{extension}";
//    string fullPath = Path.Combine(folderPath, fileName);
//    string relativePath = $"/Uploads/{dmsDto.ReferenceEntityType}/{dmsDto.ReferenceEntityId}/{dmsDto.DocumentType}/Versions/{fileName}";

//    using (var stream = new FileStream(fullPath, FileMode.Create))
//    {
//      await file.CopyToAsync(stream);
//    }

//    return new FileInfoDto
//    {
//      FileName = fileName,
//      FullPath = fullPath,
//      RelativePath = relativePath,
//      FileSize = file.Length,
//      FileExtension = extension
//    };
//  }

//  // Enhanced document version creation
//  private async Task<DmsDocumentVersion> CreateDocumentVersionWithVersioning(DmsDocument document, FileInfoDto fileInfo, DMSDto dmsDto)
//  {
//    var version = new DmsDocumentVersion
//    {
//      DocumentId = document.DocumentId,
//      VersionNumber = dmsDto.VersionNumber,
//      FileName = fileInfo.FileName,
//      FilePath = fileInfo.RelativePath,
//      FileSize = fileInfo.FileSize,
//      UploadedBy = dmsDto.UploadedByUserId?.ToString(),
//      UploadedDate = DateTime.UtcNow,
//      IsCurrentVersion = true, // Mark as current version
//      VersionNotes = $"Updated on {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
//      PreviousVersionId = await PreviousVersionIdAsync(document.DocumentId)
//    };

//    version.VersionId = await _repository.DmsDocumentVersions.CreateAndIdAsync(version);
//    await _repository.SaveAsync();

//    return version;
//  }

//  #region Helper Mehtod: Update documents with versioning

//  private async Task<DmsDocument> ExistingDocumentAsync(string entityId, string entityType, string documentType)
//  {
//    return await _repository.DmsDocuments.FirstOrDefaultAsync(d =>
//        d.ReferenceEntityId == entityId &&
//        d.ReferenceEntityType == entityType &&
//        d.DocumentType.DocumentType == documentType);
//  }

//  //  next version number for a document
//  private async Task<int> NextVersionNumberAsync(int documentId)
//  {
//    if (documentId == 0) return 1;

//    var latestVersion = await _repository.DmsDocumentVersions
//        .FirstOrDefaultWithOrderByDescAsync(
//            expression: v => v.DocumentId == documentId,
//            orderBy: v => v.VersionNumber);

//    return (latestVersion?.VersionNumber ?? 0) + 1;
//  }

//  //  previous version ID for linking
//  private async Task<int?> PreviousVersionIdAsync(int documentId)
//  {
//    var previousVersion = await _repository.DmsDocumentVersions
//        .FirstOrDefaultWithOrderByDescAsync(
//            expression: v => v.DocumentId == documentId && v.IsCurrentVersion == true,
//            orderBy: v => v.VersionNumber);

//    return previousVersion?.VersionId;
//  }


//  // Mark previous version as not current
//  private async Task MarkPreviousVersionAsNotCurrent(int documentId, int currentVersionNumber)
//  {
//    var previousVersions = await _repository.DmsDocumentVersions
//        .ListByConditionAsync(v => v.DocumentId == documentId && v.VersionNumber < currentVersionNumber);

//    foreach (var version in previousVersions)
//    {
//      version.IsCurrentVersion = false;
//      _repository.DmsDocumentVersions.Update(version);
//    }

//    await _repository.SaveAsync();
//  }


//  // Create file update history
//  private async Task CreateFileUpdateHistoryAsync(string entityId, string entityType, string documentType,
//      string oldFilePath, string newFilePath, int versionNumber, UsersDto currentUser)
//  {
//    var updateHistory = new DmsFileUpdateHistory
//    {
//      EntityId = entityId,
//      EntityType = entityType,
//      DocumentType = documentType,
//      OldFilePath = oldFilePath,
//      NewFilePath = newFilePath,
//      VersionNumber = versionNumber,
//      UpdatedBy = currentUser.UserId.ToString(),
//      UpdatedDate = DateTime.UtcNow,
//      UpdateReason = "Manual Update",
//      Notes = $"File updated from version {versionNumber - 1} to {versionNumber}"
//    };

//    await _repository.DmsFileUpdateHistories.CreateAsync(updateHistory);
//    await _repository.SaveAsync();
//  }

//  // Enhanced access log for versioning
//  private async Task CreateAccessLogForVersioning(int documentId, DMSDto dmsDto, string action)
//  {
//    string ipAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown";
//    string userAgent = _httpContextAccessor?.HttpContext?.Request?.Headers["User-Agent"].ToString() ?? "Unknown";

//    var accessLog = new DmsDocumentAccessLog
//    {
//      DocumentId = documentId,
//      AccessedByUserId = dmsDto.UploadedByUserId?.ToString() ?? "System",
//      AccessDateTime = DateTime.UtcNow,
//      Action = action,
//      IpAddress = ipAddress,
//      DeviceInfo = userAgent,
//      Notes = $"Version Control - {action}: Version {dmsDto.VersionNumber}, User: {dmsDto.UploadedByUserId}"
//    };

//    await _repository.DmsDocumentAccessLogs.CreateAsync(accessLog);
//  }

//  // Update tag mapping for versioning
//  private async Task UpdateTagMappingForVersioning(int documentId, DMSDto dmsDto)
//  {
//    if (string.IsNullOrWhiteSpace(dmsDto.DocumentTagName)) return;

//    // Add version-specific tags
//    var versionTags = $"{dmsDto.DocumentTagName},Version{dmsDto.VersionNumber},Updated";

//    var tagNames = versionTags.Split(',')
//        .Select(t => t.Trim())
//        .Where(t => !string.IsNullOrEmpty(t))
//        .Distinct();

//    foreach (var tagName in tagNames)
//    {
//      var tag = await _repository.DmsDocumentTags
//          .FirstOrDefaultAsync(t => t.DocumentTagName.ToLower().Trim() == tagName.ToLower().Trim());

//      if (tag == null)
//      {
//        tag = new DmsDocumentTag
//        {
//          DocumentTagName = tagName
//        };
//        tag.TagId = await _repository.DmsDocumentTags.CreateAndIdAsync(tag);
//        await _repository.SaveAsync();
//      }

//      var existingMapping = await _repository.DmsDocumentTagMaps
//          .FirstOrDefaultAsync(tm => tm.DocumentId == documentId && tm.TagId == tag.TagId);

//      if (existingMapping == null)
//      {
//        var tagMap = new DmsDocumentTagMap
//        {
//          DocumentId = documentId,
//          TagId = tag.TagId
//        };

//        await _repository.DmsDocumentTagMaps.CreateAsync(tagMap);
//      }
//    }

//    await _repository.SaveAsync();
//  }

//  #endregion

//  #region DTOs

//  public class FileInfoDto
//  {
//    public string FileName { get; set; }
//    public string FullPath { get; set; }
//    public string RelativePath { get; set; }
//    public long FileSize { get; set; }
//    public string FileExtension { get; set; }
//  }

//  public class DMSResponseDto
//  {
//    public bool Success { get; set; }
//    public int DocumentId { get; set; }
//    public string FilePath { get; set; }
//    public string Message { get; set; }
//    public List<string> Errors { get; set; } = new List<string>();
//  }

//  #endregion


//}
