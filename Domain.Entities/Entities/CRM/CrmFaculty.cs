namespace Domain.Entities.Entities.CRM;

public partial class CrmFaculty
{
    public int FacultyId { get; set; }
    public int InstituteId { get; set; }
    public string FacultyName { get; set; } = null!;
    public bool IsActive { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public virtual CrmInstitute? Institute { get; set; }
}
