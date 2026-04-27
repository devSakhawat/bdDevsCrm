namespace bdDevs.Shared.DataTransferObjects.CRM;

/// <summary>
/// Lightweight gender DTO for dropdown binding.
/// </summary>
public class GenderDDLDto
{
  public int GenderId { get; set; }
  public string GenderName { get; set; } = string.Empty;
}
