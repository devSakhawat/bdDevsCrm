namespace Domain.Entities.Entities.CRM;

public partial class CrmStudentAcademicProfile
{
    public int StudentAcademicProfileId { get; set; }
    public int StudentId { get; set; }
    public string? SscResult { get; set; }
    public int? SscYear { get; set; }
    public string? SscInstitute { get; set; }
    public string? HscResult { get; set; }
    public int? HscYear { get; set; }
    public string? HscInstitute { get; set; }
    public string? BachelorResult { get; set; }
    public int? BachelorYear { get; set; }
    public string? BachelorInstitute { get; set; }
    public string? MasterResult { get; set; }
    public int? MasterYear { get; set; }
    public string? MasterInstitute { get; set; }
    public string? PhdResult { get; set; }
    public int? PhdYear { get; set; }
    public string? PhdInstitute { get; set; }
    public string? CurrentEnglishProficiency { get; set; }
    public decimal? CurrentEnglishScore { get; set; }
    public DateTime CreatedDate { get; set; }
    public int CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int? UpdatedBy { get; set; }
}
