namespace bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

public class TokenBlacklistDto
{
  public int TokenId { get; set; }
  public string Token { get; set; } // The JWT token
  public DateTime ExpiryDate { get; set; } // Token's expiration date
}
