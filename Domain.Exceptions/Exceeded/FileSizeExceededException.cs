namespace Domain.Exceptions.Exceeded;

public class FileSizeExceededException : BadRequestException
{
  public FileSizeExceededException(string fileName, double fileSizeMB, double maxSizeMB)
      : base($"File '{fileName}' size ({fileSizeMB:F2} MB) exceeds the maximum allowed size of {maxSizeMB} MB.")
  {
  }

  public FileSizeExceededException(double maxSizeMB)
      : base($"File size cannot exceed {maxSizeMB} MB.")
  {
  }
}
