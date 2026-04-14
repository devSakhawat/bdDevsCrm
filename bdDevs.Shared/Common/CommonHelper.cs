using System.Reflection;

namespace bdDevs.Shared;

public class CommonHelper
{
  public static string ReplaceMultipleSpecificSpecialCharacters(string input, Dictionary<char, char> replacements)
  {
    // Iterate through the dictionary and replace each character
    foreach (var replacement in replacements)
    {
      input = input.Replace(replacement.Key, replacement.Value);
    }

    return input;
  }

  public static bool IsEncrypted(string value)
  {
    return value.StartsWith("enc_");
  }

  public static string ToRelativeUrl(string fullPath)
  {
    var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
    return fullPath.Replace(rootPath, "").Replace("\\", "/");
  }

}
