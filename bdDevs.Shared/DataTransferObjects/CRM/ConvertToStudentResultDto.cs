namespace bdDevs.Shared.DataTransferObjects.CRM;

public class ConvertToStudentResultDto
{
    public bool CanConvert { get; set; }
    public string ResultType { get; set; } = "SUCCESS";
    public int? LeadId { get; set; }
    public int? StudentId { get; set; }
    public int? ExistingStudentId { get; set; }
    public List<string> HardErrors { get; set; } = new();
    public List<string> SoftWarnings { get; set; } = new();
    public string Message { get; set; } = string.Empty;
}
