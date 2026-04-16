namespace bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

public class MaritalStatusDto
{
    public int MaritalStatusId { get; set; }
    public string? MaritalStatusName { get; set; }
    public int? IsActive { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

public class MaritalStatusDDLDto
{
    public int MaritalStatusId { get; set; }
    public string? MaritalStatusName { get; set; }
}
