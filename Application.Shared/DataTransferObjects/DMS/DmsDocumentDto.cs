using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.DataTransferObjects.DMS;

// -----------------------
public class DmsDocumentDto 
{
  public int DocumentId { get; set; }
  public string Title { get; set; } = null!;
  public string? Description { get; set; }
  public string FileName { get; set; } = null!;
  public string FileExtension { get; set; } = null!;
  public long FileSize { get; set; }
  public string FilePath { get; set; } = null!;
  public DateTime UploadDate { get; set; } = DateTime.UtcNow;
  public string? UploadedByUserId { get; set; }
  public int DocumentTypeId { get; set; }
  public string ReferenceEntityType { get; set; } = null!;
  public string ReferenceEntityId { get; set; } = null!;

  // DmsDocumentAccessLogDto -----------------------
  //public long LogId { get; set; }

  //public int DocumentId { get; set; }

  public int AccessedByUserId { get; set; }

  public DateTime AccessDateTime { get; set; }

  public string Action { get; set; } = null!;

  // DmsDocumentFolderDto -----------------------
  //public int FolderId { get; set; }

  public int? ParentFolderId { get; set; }

  public string FolderName { get; set; } = null!;

  public int OwnerUserId { get; set; }

  //public string ReferenceEntityType { get; set; } = null!;

  //public string ReferenceEntityId { get; set; } = null!;


  // DmsDocumentTagDto-----------------------
  
  public int TagId { get; set; }

  public string DocumentTagName { get; set; } = null!;


  // DmsDocumentTypeDto -----------------------
  //public int DocumentTypeId { get; set; }
  
  public string DocumentTypeName { get; set; } = null!;
  public string EntityType { get; set; } = null!;
  public bool IsMandatory { get; set; }
  public string? AcceptedExtensions { get; set; }
  public int? MaxFileSizeMb { get; set; }

  // DmsDocumentVersionDto -----------------------
  public int VersionId { get; set; }
  //public int DocumentId { get; set; }
  public int VersionNumber { get; set; }
  //public string FileName { get; set; } = null!;
  //public string FilePath { get; set; } = null!;
  public DateTime UploadedDate { get; set; }
  public int UploadedBy { get; set; }
}
