using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Entities.CRM;

public partial class CrmAdditionalDocument
{
  [Key]
  public int AdditionalDocumentId { get; set; }

  public int? ApplicantId { get; set; }

  [StringLength(350)]
  [Unicode(false)]
  public string DocumentTitle { get; set; } = null!;

  [StringLength(350)]
  public string DocumentPath { get; set; } = null!;

  [StringLength(150)]
  [Unicode(false)]
  public string DocumentName { get; set; } = null!;

  [StringLength(150)]
  [Unicode(false)]
  public string RecordType { get; set; } = null!;

  [Column(TypeName = "datetime")]
  public DateTime CreatedDate { get; set; }

  public int CreatedBy { get; set; }

  [Column(TypeName = "datetime")]
  public DateTime? UpdatedDate { get; set; }

  public int? UpdatedBy { get; set; }
}
