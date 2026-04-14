namespace Application.Shared.DataTransferObjects.Core.SystemAdmin;

public class GroupMemberDto
{

  public int GroupId { get; set; }
  public int UserId { get; set; }
  public string? GroupOption { get; set; }
}

