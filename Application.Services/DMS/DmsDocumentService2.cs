using bdDevs.Shared.Constants;
﻿


//using Application.Shared.Grid;
//using Domain.Entities.Entities.System;
//using Domain.Entities.Entities.DMS;
//
//using Domain.Contracts.Services.Core.SystemAdmin;
//using Domain.Contracts.Services.DMS;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.DMS;
//using Domain.Contracts.Repositories;
//using Application.Services.Mappings;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.NetworkInformation;
//using System.Text;
//using System.Threading.Tasks;

//namespace Domain.Contracts.Services.DMS;

//internal sealed class DmsDocumentService2 : IDmsDocumentService2
//{
//  private readonly IRepositoryManager _repository;
//  private readonly ILoggerManager _logger;
//  private readonly IConfiguration _configuration;
//  private readonly IHttpContextAccessor _httpContextAccessor;

//  public DmsDocumentService2(IRepositoryManager repository, ILoggerManager logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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

//    var gridEntity = await _repository.DmsDocuments.GridData<DmsDocumentDto>(query, options, orderBy, "");

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
//    _logger.LogWarn($"New document created with Id: {createdId}");

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

//    _logger.LogWarn($"Document with Id: {key} updated.");

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

//    _logger.LogWarn($"Document with Id: {key} deleted.");

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
//      _logger.LogWarn($"New document created with Id: {createdId}");
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

//        _logger.LogWarn($"Document with Id: {key} updated.");
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

//    var dmsDto = Newtonsoft.Json.JsonConvert.DeserializeObject<DMSDto>(allAboutDMS);
//    var crmInstituteDto = Newtonsoft.Json.JsonConvert.DeserializeObject<CrmInstituteDto>(allAboutDMS);
//    var usersDto = Newtonsoft.Json.JsonConvert.DeserializeObject<UsersDto>(allAboutDMS);

//    if (dmsDto == null || crmInstituteDto == null || usersDto == null) throw new BadRequestException(nameof(DMSDto));

//    01.Check DMSDocumentType by dmsDto.EntityType
//   var dmsDocumentType = await _repository.DmsDocumentTypes.FirstOrDefaultAsync(f => f.DocumentType.ToLower().Trim() == dmsDto.DocumentType.ToLower().Trim());
//    if (dmsDocumentType == null)
//    {
//      dmsDocumentType = new DmsDocumentType
//      {
//        Name = dmsDto.DocumentTypeName ?? "Default Document Type",
//        DocumentType = dmsDto.DocumentType,
//        IsMandatory = dmsDto.IsMandatory,
//        AcceptedExtensions = dmsDto.AcceptedExtensions ?? ".pdf, .docx, .jpg, .png, .jpeg",
//        MaxFileSizeMb = dmsDto.MaxFileSizeMb ?? ((dmsDto.AcceptedExtensions != null
//        && (dmsDto.AcceptedExtensions.Contains(".jpg")
//        || dmsDto.AcceptedExtensions.Contains(".png")
//        || dmsDto.AcceptedExtensions.Contains(".jpeg"))) ? 1 : 5)
//        MaxFileSizeMb = (!dmsDto.AcceptedExtensions.Contains(".pdf")) ? 1 : dmsDto.MaxFileSizeMb ?? 10
//      };
//      dmsDocumentType.DocumentTypeId = await _repository.DmsDocumentTypes.CreateAndIdAsync(dmsDocumentType);
//    }

//    02.Check parent forlder and child folder DmsDocumentFolders by dmsDto.FolderName and ParentFolderId
//   var parentFolder = await _repository.DmsDocumentFolders.FirstOrDefaultAsync(f => f.FolderName.ToLower().Trim() == dmsDto.ReferenceEntityType.ToLower().Trim());
//    if (parentFolder == null)
//    {
//      parentFolder = new DmsDocumentFolder
//      {
//        FolderName = dmsDto.ReferenceEntityType,
//        OwnerId = null, // OwnerId: Folder Owner.
//        ReferenceEntityType = dmsDto.ReferenceEntityType, // e.g., "Crminstitute"
//        ReferenceEntityId = "0"
//      };
//      Parent folder has no ReferenceEntityId ,OwnerId, and ParentFolderId
//       because it is the top - level folder.
//       OwnerId,ReferenceEntityId,ParentFolderId is not set here, it will be set later when creating child folders.
//      parentFolder.ParentFolderId = await _repository.DmsDocumentFolders.CreateAndIdAsync(parentFolder);
//    }

//    var dmsDocumentFolder = await _repository.DmsDocumentFolders.FirstOrDefaultAsync(f => f.FolderName.ToLower().Trim() == dmsDto.FolderName.ToLower().Trim()
//    && f.ParentFolderId == dmsDto.ParentFolderId);
//    if (dmsDocumentFolder == null)
//    {
//      dmsDocumentFolder = new DmsDocumentFolder
//      {
//        ParentFolderId = parentFolder.ParentFolderId,
//        FolderName = dmsDto.ReferenceEntityType,
//        OwnerId = dmsDto.OwnerUserId.ToString(),
//        ReferenceEntityType = dmsDto.ReferenceEntityType, // e.g., "Crminstitute"
//        ReferenceEntityId = dmsDto.ReferenceEntityId // e.g., "1" (for Crminstitute)
//      };
//      dmsDocumentFolder.FolderId = await _repository.DmsDocumentFolders.CreateAndIdAsync(dmsDocumentFolder);
//    }

//    2.1.Find or create parent folder(e.g., "Institute")
//    string rootPath = Path.Combine(Directory.CurrentDirectory(), "wwwroot");
//    folderPath = "wwwroot/Uploads/students/CRMInstitute/1/passport/"
//     wwwroot / Uploads / EntityName(CRMInstitute, CRMCourse, Student, Agent, like controller / entity) / 1(record or data: like studentIdentity, crnIntituteIdency, CrmCourseIdentity) / DocumentType(like: student(passport, nid, accademic certificate || institute: logo, photo, procpectus etc))
//    string folderPath = Path.Combine(rootPath, "Uploads", dmsDto.ReferenceEntityType, dmsDto.ReferenceEntityId, dmsDto.DocumentType);
//    if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

//    string fileName = $"{Path.FileNameWithoutExtension(file.FileName)}{DateTime.Now.Ticks}{Path.Extension(file.FileName)}";
//    string fullPath = Path.Combine(folderPath, fileName);

//    Save file to disk
//    using (var stream = new FileStream(fullPath, FileMode.Create)) { await file.CopyToAsync(stream); }
//    string relativeFilePath = $"/Uploads/{dmsDto.ReferenceEntityType}/{dmsDto.ReferenceEntityId}/{dmsDto.DocumentType}/{fileName}";

//    3.Create DmsDocument
//   var document = new DmsDocument
//   {
//     Title = dmsDto.Title ?? fileName,
//     Description = dmsDto.Description,
//     FileName = fileName,
//     FilePath = relativeFilePath,
//     FileExtension = Path.Extension(file.FileName),
//     FileSize = file.Length,
//     FolderId = dmsDocumentFolder.FolderId,
//     DocumentTypeId = dmsDocumentType.DocumentTypeId,
//     ReferenceEntityType = dmsDto.ReferenceEntityType,
//     ReferenceEntityId = dmsDto.ReferenceEntityId,
//     UploadDate = DateTime.UtcNow,
//     UploadedByUserId = usersDto.UserId.ToString(),
//   };
//    document.DocumentId = await _repository.DmsDocuments.CreateAndIdAsync(document);
//    _logger.LogInfo($"DMS Document created with Id: {document.DocumentId}, Title: {document.Title}, FilePath: {relativeFilePath}");

//    4.Create DmsDocumentVersion
//   var version = new DmsDocumentVersion();
//    if (dmsDto.VersionNumber > 0)
//    {
//      version.DocumentId = document.DocumentId;
//      version.VersionNumber = dmsDto.VersionNumber > 0 ? dmsDto.VersionNumber : 1;
//      version.FileName = fileName;
//      version.FilePath = relativeFilePath;
//      version.UploadedBy = usersDto.UserId.ToString();
//      version.UploadedDate = DateTime.UtcNow;
//      version.VersionId = await _repository.DmsDocumentVersions.CreateAndIdAsync(version);
//    }
//    ;
//    var documentVersionId = await _repository.DmsDocumentVersions.CreateAndIdAsync(version);
//    _logger.LogInfo($"DMS Document created with Id: {document.DocumentId}, VersionId: {version.VersionId}");


//    5.Create DmsDocumentAccessLog
//    string ipAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown";
//    string userAgent = _httpContextAccessor?.HttpContext?.Request?.Headers["User-Agent"].ToString() ?? "Unknown";
//    _logger.LogInfo($"File uploaded by UserId: {usersDto.UserId}, IP: {ipAddress}, User-Agent: {userAgent}");
//    string macAddress = NetworkInterface.AllNetworkInterfaces()
//        .Where(n => n.OperationalStatus == OperationalStatus.Up && n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
//        .Select(n => n.PhysicalAddress().ToString())
//        .FirstOrDefault() ?? "Unavailable";
//    var accessLog = new DmsDocumentAccessLog
//    {
//      DocumentId = document.DocumentId,
//      AccessedByUserId = usersDto.UserId.ToString(),
//      AccessDateTime = DateTime.UtcNow,
//      Action = dmsDto.Action ?? "Upload",
//      IpAddress = ipAddress,
//      DeviceInfo = userAgent, // User-Agent string
//      MacAddress = macAddress,
//      Notes = $"File uploaded by UserId: {usersDto.UserId}, IP: {ipAddress}, User-Agent: {userAgent}"
//    };
//    await _repository.DmsDocumentAccessLogs.CreateAsync(accessLog);
//    await _repository.SaveAsync();
//    _logger.LogInfo($"DMS Document access log created for DocumentId: {document.DocumentId}, Action: {accessLog.Action}, AccessedByUserId: {accessLog.AccessedByUserId}");
//    _configuration["DMS:FileUploadPath"] = relativeFilePath; // Save the file path to configuration if needed

//    7.Create DmsDocumentTag and Map
//    if (dmsDto.DocumentTagName != null && !string.IsNullOrWhiteSpace(dmsDto.DocumentTagName))
//    {
//      var tags = dmsDto.DocumentTagName.Split(',')
//          .Select(t => t.Trim())
//          .Where(t => !string.IsNullOrEmpty(t))
//          .Distinct();

//      foreach (var tagName in tags)
//      {
//        Check if the tag already exists
//       var tag = await _repository.DmsDocumentTags.FirstOrDefaultAsync(t => t.DocumentTagName == tagName);
//        if (tag == null)
//        {
//          tag = new DmsDocumentTag { DocumentTagName = tagName };
//          tag.TagId = await _repository.DmsDocumentTags.CreateAndIdAsync(tag);
//          await _repository.SaveAsync();
//        }

//        // Map the tag to the document
//        var tagMap = new DmsDocumentTagMap
//        {
//          DocumentId = document.DocumentId,
//          TagId = tag.TagId
//        };

//        await _repository.DmsDocumentTagMaps.CreateAsync(tagMap);
//      }
//    }





//    return relativeFilePath;
//  }


//}
























//using Application.Shared.Grid;
//using Domain.Entities.Entities.DMS;
//
//using Domain.Contracts.Services.Core.SystemAdmin;
//using Domain.Contracts.Services.DMS;
//using bdDevs.Shared.DataTransferObjects.DMS;
//using Domain.Contracts.Repositories;
//using Application.Services.Mappings;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Domain.Contracts.Services.DMS;

//internal sealed class DmsDocumentService2 : IDmsDocumentService2
//{
//  private readonly IRepositoryManager _repo;
//  private readonly ILoggerManager _log;
//  private readonly IConfiguration _cfg;

//  private static readonly string[] _imageExt = [".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp"];

//  public DmsDocumentService2(IRepositoryManager repo, ILoggerManager log, IConfiguration cfg)
//  {
//    _repo = repo;
//    _log = log;
//    _cfg = cfg;
//  }

//  // -------------- DDL ----------------------------------------------
//  public async Task<IEnumerable<KeyValuePair<int, string>>> DocumentDDLAsync()
//  {
//    var docs = await _repo.DmsDocuments.ActiveAsync(false);
//    if (!docs.Any()) throw new GenericListNotFoundException("DmsDocument");
//    return docs.Select(d => new KeyValuePair<int, string>(d.DocumentId, d.Title));
//  }

//  // -------------- Grid ---------------------------------------------
//  public async Task<GridEntity<DmsDocumentDto>> SummaryGrid(GridOptions opt)
//  {
//    string sql = "SELECT * FROM DmsDocument";   // 👉 এখানে আপনার নিজস্ব ভিউ/স্টোর্ড-প্রসিজিউর ব্যবহার করতে পারেন
//    string orderBy = " UploadDate DESC ";
//    return await _repo.DmsDocuments.GridData<DmsDocumentDto>(sql, opt, orderBy, "");
//  }

//  // -------------- Create -------------------------------------------
//  public async Task<string> CreateAsync(DmsDocumentDto dto)
//  {
//    if (dto.DocumentId != 0) throw new InvalidCreateOperationException("DocumentId must be 0.");

//    await ValidateFileRules(dto.FileExtension, dto.FileSize);

//    bool dup = await _repo.DmsDocuments.ExistsAsync(d =>
//        d.Title == dto.Title && d.ReferenceEntityId == dto.ReferenceEntityId);
//    if (dup) throw new DuplicateRecordException("DmsDocument", "Title");

//    var entity = MyMapper.JsonClone<DmsDocumentDto, DmsDocument>(dto);

//    // 👉 ফাইল সেভ করলে path সেট করুন (StorageService ইত্যাদি)
//    // entity.FilePath = _fileStore.Save(fileStream, dto.FileName);

//    int id = await _repo.DmsDocuments.CreateAndIdAsync(entity);
//    if (id <= 0) throw new InvalidCreateOperationException();

//    _log.LogInfo($"DMS Document created, id={id}");
//    return OperationMessage.Success;
//  }

//  // -------------- Update -------------------------------------------
//  public async Task<string> UpdateAsync(int key, DmsDocumentDto dto)
//  {
//    if (key != dto.DocumentId) throw new BadRequestException(key.ToString(), nameof(DmsDocumentDto));

//    await ValidateFileRules(dto.FileExtension, dto.FileSize);

//    bool exists = await _repo.DmsDocuments.ExistsAsync(x => x.DocumentId == key);
//    if (!exists) throw new NotFoundException("DmsDocument", "DocumentId", key.ToString());

//    var entity = MyMapper.JsonClone<DmsDocumentDto, DmsDocument>(dto);
//    _repo.DmsDocuments.Update(entity);
//    await _repo.SaveAsync();

//    _log.LogInfo($"DMS Document updated, id={key}");
//    return OperationMessage.Success;
//  }

//  // -------------- Delete -------------------------------------------
//  public async Task<string> DeleteAsync(int key)
//  {
//    await _repo.DmsDocuments.DeleteAsync(x => x.DocumentId == key, true);
//    await _repo.SaveAsync();

//    _log.LogInfo($"DMS Document deleted, id={key}");
//    return OperationMessage.Success;
//  }

//  // ---------------------------------------------------------
//  private static Task ValidateFileRules(string ext, long sizeInBytes)
//  {
//    bool isImage = _imageExt.Contains(ext.ToLower());
//    long max = isImage ? 1 * 1024 * 1024 : 5 * 1024 * 1024;

//    if (sizeInBytes > max)
//      throw new FileSizeExceededException($"File size exceeds limit ({max / 1024 / 1024} MB).");

//    return Task.CompletedTask;
//  }
//}

//// If the FileSizeExceededException class is not defined in the bdDevCRM.Entities.Exceptions namespace,
//// you need to define it. Below is the definition based on the provided type signature.



