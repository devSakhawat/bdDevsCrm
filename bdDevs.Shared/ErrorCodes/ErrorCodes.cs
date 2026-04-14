namespace bdDevs.Shared.ErrorCodes;

public static class ErrorCodes
{
  public const string MissingAcceptHeader = "MISSING_ACCEPT_HEADER";
  public const string InvalidMediaType = "INVALID_MEDIA_TYPE";
  public const string UnsupportedMediaType = "UNSUPPORTED_MEDIA_TYPE";

  public static class Auth
  {
    public const string InvalidToken = "AUTH_INVALID_TOKEN";
    public const string TokenExpired = "AUTH_TOKEN_EXPIRED";
  }

  public static class Validation
  {
    public const string RequiredField = "VALID_REQUIRED";
    public const string InvalidInput = "VALID_INVALID";
  }
}