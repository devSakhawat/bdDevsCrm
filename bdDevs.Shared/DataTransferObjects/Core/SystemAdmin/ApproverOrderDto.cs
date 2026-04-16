namespace bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

public class ApproverOrderDto
{
    public int ApproverOrderId { get; set; }
    public string? OrderTitle { get; set; }
    public int? ModuleId { get; set; }
    public int? ApproverTypeId { get; set; }
    public bool? IsEditable { get; set; }
}

public class ApproverOrderDDLDto
{
    public int ApproverOrderId { get; set; }
    public string? OrderTitle { get; set; }
}
