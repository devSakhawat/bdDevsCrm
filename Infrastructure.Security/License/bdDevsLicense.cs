using System.Globalization;

namespace Infrastructure.Security.License;

public class bdDevsLicense
{
	private string _serialNo;

	private string _companyName;

	private string _licenseKey;

	private int _noOfUser;

	private int _noOfRepository;

	private DateTime _expiryDate;

	private string _module;

	public bdDevsLicense()
	{
		Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
		_serialNo = ".BN92QP1.CN70166133000F.BFEBFBFF00020655BN92QP12661";
		_companyName = " Karim & Karim";
		_licenseKey = "AZ12-LN12-WMG2-011E";
		_noOfUser = 100;
		_noOfRepository = 5;
		_expiryDate = DateTime.Parse("31/12/2027", new CultureInfo("en-GB"));
		//Module = "HR Record;Conveyance;Movement;Attendance;Leave;Payroll";
		_module = "CRM";
	}

	public string CompanyName()
	{
		return _companyName;
	}

	public string LicenseKey()
	{
		return _licenseKey;
	}

	public int NumberOfUser()
	{
		return _noOfUser;
	}

	public int NumberOfRepository()
	{
		return _noOfRepository;
	}

	public DateTime ExpiryDate()
	{
		return _expiryDate;
	}

	public bool ValidateActivation(string SerialNo, string LicenseKey)
	{
		if (SerialNo == _serialNo && LicenseKey == _licenseKey)
		{
			return true;
		}

		return false;
	}

	public bool ValidateNoOfUser(int NoOfUser)
	{
		if (NoOfUser >= _noOfUser)
		{
			return false;
		}

		return true;
	}

	public bool ValidateNoOfRepository(int NoOfRepository)
	{
		if (NoOfRepository >= _noOfRepository)
		{
			return false;
		}

		return true;
	}

	public bool ValidateExpiryDate()
	{
		if (DateTime.Today > _expiryDate)
		{
			return false;
		}

		return true;
	}

	public bool ValidateModulePermission(string ModuleName)
	{
		string[] array = _module.Split(';');
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].ToLower() == ModuleName.ToLower())
			{
				return true;
			}
		}

		return false;
	}
}
