namespace Application.Shared.ApiResponse;

public class ApiValidationErrorResponse : ApiResponse
{
  public ApiValidationErrorResponse()
  {
    StatusCode = 400;
    Errors = new List<string>();
  }

  public IEnumerable<string> Errors { get; set; }
}



