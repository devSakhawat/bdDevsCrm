using Newtonsoft.Json;

namespace bdDevs.Shared;

public class ErrorDetails
{
  public int StatusCode { get; set; }
  public string Message { get; set; }
  public string ErrorType { get; set; }

  public override string ToString() => JsonConvert.SerializeObject(this);

  //public override string ToString()
  //{
  //  return JsonConvert.SerializeObject(this);
  //}
}