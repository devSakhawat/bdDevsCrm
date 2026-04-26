namespace bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

public class TokenBlacklistDto
{
  public Guid TokenId { get; set; }
  public string Token { get; set; } // The JWT token
  public DateTime ExpiryDate { get; set; } // Token's expiration date
  public DateTime CreatedAt { get; set; }
}
