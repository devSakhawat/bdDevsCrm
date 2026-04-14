using System.ComponentModel.DataAnnotations;

namespace Application.Shared.DataTransferObjects.Authentication;

public class UserForAuthenticationDto
{
	[Required(ErrorMessage = "User name is required")]
	public string LoginId { get; set; }

	[StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20 characters")]
	[Required(ErrorMessage = "Password is required")]
	public string Password { get; set; }

	public bool IsRememberMe { get; set; }

	public bool? IsEncryptedPass { get; set; } = false;
}

public class TokenVerificationRequest
{
	[Required(ErrorMessage = "Token is required")]
	public string Token { get; set; }
}

