namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmStudentAcademicProfileDto
{
    public int StudentAcademicProfileId { get; init; }
    public int StudentId { get; init; }
    public string? SscResult { get; init; }
    public int? SscYear { get; init; }
    public string? SscInstitute { get; init; }
    public string? HscResult { get; init; }
    public int? HscYear { get; init; }
    public string? HscInstitute { get; init; }
    public string? BachelorResult { get; init; }
    public int? BachelorYear { get; init; }
    public string? BachelorInstitute { get; init; }
    public string? MasterResult { get; init; }
    public int? MasterYear { get; init; }
    public string? MasterInstitute { get; init; }
    public string? PhdResult { get; init; }
    public int? PhdYear { get; init; }
    public string? PhdInstitute { get; init; }
    public string? CurrentEnglishProficiency { get; init; }
    public decimal? CurrentEnglishScore { get; init; }
    public DateTime CreatedDate { get; init; }
    public int CreatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public int? UpdatedBy { get; init; }
}
