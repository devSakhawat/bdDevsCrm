namespace Domain.Exceptions.Authentication;

public class UsernamePasswordMismatchException : BadRequestException
{
  public UsernamePasswordMismatchException() : base("The username or password is incorrect. Please try again.")
  {
  }

  public UsernamePasswordMismatchException(string message) : base(message)
  {
  }
}