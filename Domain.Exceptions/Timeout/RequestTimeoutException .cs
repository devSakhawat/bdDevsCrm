namespace Domain.Exceptions.Timeout;

public class RequestTimeoutException : Exception
{
	public RequestTimeoutException(string message) : base(message) { }
}
