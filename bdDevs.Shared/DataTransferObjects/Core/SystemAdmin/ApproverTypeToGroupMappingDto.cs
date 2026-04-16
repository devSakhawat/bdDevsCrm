namespace bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

public class ApproverTypeToGroupMappingDto
{
    public int ApproverTypeMapId { get; set; }
    public int? ApproverTypeId { get; set; }
    public int? ModuleId { get; set; }
    public int? GroupId { get; set; }
}

public class ApproverTypeToGroupMappingDDLDto
{
    public int ApproverTypeMapId { get; set; }
    public int? ApproverTypeId { get; set; }
    public int? GroupId { get; set; }
}
