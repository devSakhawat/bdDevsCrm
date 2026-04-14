using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.DataTransferObjects.DMS;

public class DmsDocumentFolderDto
{
  public int FolderId { get; set; }

  public int? ParentFolderId { get; set; }

  public string FolderName { get; set; } = null!;

  public int OwnerUserId { get; set; }

  public string ReferenceEntityType { get; set; } = null!;

  public string ReferenceEntityId { get; set; } = null!;

  //public virtual ICollection<DmsDocumentFolder> InverseParentFolder { get; set; } = new List<DmsDocumentFolder>();

  //public virtual DmsDocumentFolder? ParentFolder { get; set; }
}