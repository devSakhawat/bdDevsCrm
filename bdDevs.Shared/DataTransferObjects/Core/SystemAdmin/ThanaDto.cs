namespace bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

public class ThanaDto
{
    public int ThanaId { get; set; }
    public int DistrictId { get; set; }
    public string? ThanaName { get; set; }
    public string? ThanaCode { get; set; }
    public int? Status { get; set; }
    public string? ThanaNameBn { get; set; }
}

public class ThanaDDLDto
{
    public int ThanaId { get; set; }
    public string? ThanaName { get; set; }
    public string? ThanaNameBn { get; set; }
}
