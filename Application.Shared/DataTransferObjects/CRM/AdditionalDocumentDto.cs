using Microsoft.AspNetCore.Http;

namespace bdDevs.Shared.DataTransferObjects.CRM;

public class AdditionalDocumentDto
{
  public int AdditionalDocumentId { get; set; }
  public int ApplicantId { get; set; } // Foreign Key
  public string? DocumentTitle { get; set; }
  public string? DocumentName { get; set; }
  public string? RecordType { get; set; } = "Document";
  public string DocumentPath { get; set; } = null!;

  //public string? AttachedDocument { get; set; }
  //public string? UploadFile { get; set; }

  public IFormFile? UploadFormFile { get; set; }

  public string? FileThumbnail { get; set; }

  // Common fields
  public DateTime CreatedDate { get; set; }
  public int CreatedBy { get; set; }
  public DateTime? UpdatedDate { get; set; }
  public int? UpdatedBy { get; set; }
}