namespace bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

public class DocumentParameterDto
{
    public int ParameterId { get; set; }
    public string ParameterName { get; set; } = null!;
    public string ParameterKey { get; set; } = null!;
    public string? ControlRole { get; set; }
    public string? DataSource { get; set; }
    public int? ControlSequence { get; set; }
    public string? DataTextField { get; set; }
    public string? DataValueField { get; set; }
    public string? CaseCading { get; set; }
}

public class DocumentParameterDDLDto
{
    public int ParameterId { get; set; }
    public string ParameterName { get; set; } = null!;
    public string ParameterKey { get; set; } = null!;
}
