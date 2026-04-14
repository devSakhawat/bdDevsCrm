using System.ComponentModel.DataAnnotations;
using System.Resources;
//using ValidationResult = Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult;

namespace Application.Shared.Common;


public class AppValidationResult
{
	public string Key { get; set; }
	public string localizedMessage { get; set; }
}

public class ValidationHelper
{
	private readonly ResourceManager resMan;

	public ValidationHelper(ResourceManager resourceManager)
	{
		resMan = resourceManager;
	}

	public ValidationHelper()
	{
	}

	public static List<AppValidationResult> Validate<T>(T entity)
	{
		var validationResults = new List<ValidationResult>();
		var context = new ValidationContext(entity, null, null);

		Validator.TryValidateObject(entity, context, validationResults, true);

		var appValidationResults = new List<AppValidationResult>();

		foreach (var result in validationResults)
		{
			foreach (var memberName in result.MemberNames)
			{
				appValidationResults.Add(new AppValidationResult
				{
					Key = memberName,
					localizedMessage = result.ErrorMessage
				});
			}
		}

		return appValidationResults;
	}

	public static List<AppValidationResult> ValidateList<T>(List<T> entities)
	{
		var allErrors = new List<AppValidationResult>();

		foreach (var entity in entities)
		{
			var errors = Validate(entity);
			allErrors.AddRange(errors);
		}

		return allErrors;
	}

	//public List<AppValidationResult> Validate<T>(T entity)
	//{
	//  ValidationResults results =
	//      Microsoft.Practices.EnterpriseLibrary.Validation.Validation.ValidateFromAttributes(entity);
	//  var validationResults = new List<AppValidationResult>();
	//  foreach (Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult result in results)
	//  {
	//    var pvr = new AppValidationResult();
	//    pvr.Key = result.Key;
	//    pvr.localizedMessage = buildMessage(result.Message, result.Tag);
	//    validationResults.Add(pvr);
	//  }
	//  return validationResults;
	//}

	//public List<AppValidationResult> Validate<T>(List<T> entities)
	//{
	//  var validationResults = new List<AppValidationResult>();
	//  foreach (T entity in entities)
	//  {
	//    ValidationResults results = Validation.ValidateFromAttributes(entity);
	//    foreach (ValidationResult result in results)
	//    {
	//      var pvr = new AppValidationResult();
	//      pvr.Key = result.Key;
	//      pvr.localizedMessage = buildMessage(result.Message, result.Tag);
	//      validationResults.Add(pvr);
	//    }
	//  }
	//  return validationResults;
	//}

	private string buildMessage(string MessageTemplateKey, string replacement)
	{
		string msg = string.Empty;
		try
		{
			msg = resMan.GetString(MessageTemplateKey);
		}
		catch
		{
		}

		if (string.IsNullOrEmpty(msg))
		{
			return replacement + " is invalid";
		}
		return msg;
	}


	// will be come from system settings later
	public string ValidateUser(string loginID, int minLoginLength, string password, int minPassLength, int passType, bool specialCharIsAllowed)
	{
		string specialChs = @"! ~ @ # $ % ^ & * ( ) _ - + = { } [ ] : ; , . < > ? / | \";
		string[] specialCharacters = specialChs.Split(' ');
		string message = "Valid";
		if (loginID != "")
		{
			if (minLoginLength > loginID.Trim().Length)
			{
				message = "Login Id must have to be minimum " + minLoginLength + "character long!";
				return message;
			}
		}

		if (minPassLength > password.Trim().Length)
		{
			message = "Password must have to be minimum " + minPassLength + "character long!";
			return message;
		}

		if (minLoginLength == 0 && minPassLength == 0 && specialCharIsAllowed == false) return message;

		int numCount = 0; //Numaric Charcter in password text
		int charCount = 0; //Charecter count
		int specialcharCount = 0;
		char[] pasChars = password.ToCharArray();
		for (int i = 0; i < pasChars.Length; i++)
		{
			if (pasChars[i] == '0' || pasChars[i] == '1' || pasChars[i] == '2' || pasChars[i] == '3' ||
				pasChars[i] == '4' || pasChars[i] == '5' || pasChars[i] == '6' || pasChars[i] == '7' ||
				pasChars[i] == '8' || pasChars[i] == '9')
				numCount++;
			else
			{
				IEnumerable<string> found = specialCharacters.Where(x => x == pasChars[i].ToString());
				if (found.Count() == 0)
					charCount++;
				else
					specialcharCount++;
			}
		}
		//passType 0 = Alpjabetic, 1=Numeric, 2=AlphaNumeric
		if (passType == 0)
		{
			//0 = Alpjabetic

			if (numCount > 0)
			{
				message = "Password must not have any number!";
				return message;
			}

			if (charCount == 0)
			{
				message = "Password must have to be alphabetic characters!";
				return message;
			}
		}
		else if (passType == 1)
		{
			//1=Numeric
			if (numCount == 0)
			{
				message = "Password must have atleast one numeric character!";
				return message;
			}

			if (charCount > 0)
			{
				message = "Password must not have any alphabetic character!";
				return message;
			}
		}
		else
		{
			//2=AlphaNumeric
			if (numCount == 0)
			{
				message = "Password must have atleast one numeric character!";
				return message;
			}

			if (charCount == 0)
			{
				message = "Password must have atleast one alphabetic character!";
				return message;
			}
		}
		if (specialCharIsAllowed)
		{
			if (specialcharCount == 0)
			{
				message = "Password must have atleast one special character!";
				return message;
			}
		}

		return message;
	}

	public static bool ValidateLoginPassword(string inputPassword, string dbPassword, bool encryption)
	{
		string encryptPass = "";
		encryptPass = (encryption) ? EncryptDecryptHelper.Encrypt(inputPassword) : inputPassword;
		return (encryptPass == dbPassword);
	}

	public string EncryptedPassword(string password)
	{
		return EncryptDecryptHelper.Encrypt(password);
	}

	public string DecryptedPassword(string password)
	{
		return EncryptDecryptHelper.Decrypt(password);
	}
}
