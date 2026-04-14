using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.DataTransferObjects.DMS;

public class DmsDocumentVersionDto
{
  public int VersionId { get; set; }

  public int DocumentId { get; set; }

  public int VersionNumber { get; set; }

  public string FileName { get; set; } = null!;

  public string FilePath { get; set; } = null!;

  public DateTime? UploadedDate { get; set; }

  public string? UploadedBy { get; set; }

  public bool? IsCurrentVersion { get; set; }

  public string? VersionNotes { get; set; }

  public int? PreviousVersionId { get; set; }

  public long? FileSize { get; set; }
}
