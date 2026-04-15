using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateEmployeeRecord.
/// </summary>
public class CreateEmployeeRecordValidator : BaseRecordValidator<CreateEmployeeRecord>
{
    public CreateEmployeeRecordValidator()
    {
        RuleFor(x => x.FullName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.FullName))
            .WithMessage($"FullName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.FatherName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.FatherName))
            .WithMessage($"FatherName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.MotherName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.MotherName))
            .WithMessage($"MotherName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.SpouseName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.SpouseName))
            .WithMessage($"SpouseName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.NationalId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.NationalId))
            .WithMessage($"NationalId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.PassportNo)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.PassportNo))
            .WithMessage($"PassportNo cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.PresentAddress)
            .MaximumLength(MaxAddressLength)
            .When(x => !string.IsNullOrEmpty(x.PresentAddress))
            .WithMessage($"PresentAddress cannot exceed {MaxAddressLength} characters");

        RuleFor(x => x.PermanentAddress)
            .MaximumLength(MaxAddressLength)
            .When(x => !string.IsNullOrEmpty(x.PermanentAddress))
            .WithMessage($"PermanentAddress cannot exceed {MaxAddressLength} characters");

        RuleFor(x => x.HomePhone)
            .MaximumLength(MaxPhoneLength)
            .When(x => !string.IsNullOrEmpty(x.HomePhone))
            .WithMessage($"HomePhone cannot exceed {MaxPhoneLength} characters");

        RuleFor(x => x.MobileNo)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.MobileNo))
            .WithMessage($"MobileNo cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.PersonalEmail)
            .MaximumLength(MaxEmailLength)
            .When(x => !string.IsNullOrEmpty(x.PersonalEmail))
            .WithMessage($"PersonalEmail cannot exceed {MaxEmailLength} characters");

        RuleFor(x => x.InternetMessenger)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.InternetMessenger))
            .WithMessage($"InternetMessenger cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.InternetProfileLink)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.InternetProfileLink))
            .WithMessage($"InternetProfileLink cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AdditionalInfo)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AdditionalInfo))
            .WithMessage($"AdditionalInfo cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.BloodGroup)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.BloodGroup))
            .WithMessage($"BloodGroup cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.OriginalBirthDay)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.OriginalBirthDay))
            .WithMessage($"OriginalBirthDay cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Profilepicture)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Profilepicture))
            .WithMessage($"Profilepicture cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Birthidentification)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Birthidentification))
            .WithMessage($"Birthidentification cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Height)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Height))
            .WithMessage($"Height cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Weight)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Weight))
            .WithMessage($"Weight cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Hobby)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Hobby))
            .WithMessage($"Hobby cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Signature)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Signature))
            .WithMessage($"Signature cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Identificationmark)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Identificationmark))
            .WithMessage($"Identificationmark cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ShortName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.ShortName))
            .WithMessage($"ShortName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.PresentPostCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.PresentPostCode))
            .WithMessage($"PresentPostCode cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.PermanentPostCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.PermanentPostCode))
            .WithMessage($"PermanentPostCode cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.Refempid)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Refempid))
            .WithMessage($"Refempid cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateEmployeeRecord.
/// </summary>
public class UpdateEmployeeRecordValidator : BaseRecordValidator<UpdateEmployeeRecord>
{
    public UpdateEmployeeRecordValidator()
    {
        RuleFor(x => x.HrrecordId)
            .GreaterThan(0)
            .WithMessage("HrrecordId must be greater than 0");

        RuleFor(x => x.FullName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.FullName))
            .WithMessage($"FullName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.FatherName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.FatherName))
            .WithMessage($"FatherName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.MotherName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.MotherName))
            .WithMessage($"MotherName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.SpouseName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.SpouseName))
            .WithMessage($"SpouseName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.NationalId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.NationalId))
            .WithMessage($"NationalId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.PassportNo)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.PassportNo))
            .WithMessage($"PassportNo cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.PresentAddress)
            .MaximumLength(MaxAddressLength)
            .When(x => !string.IsNullOrEmpty(x.PresentAddress))
            .WithMessage($"PresentAddress cannot exceed {MaxAddressLength} characters");

        RuleFor(x => x.PermanentAddress)
            .MaximumLength(MaxAddressLength)
            .When(x => !string.IsNullOrEmpty(x.PermanentAddress))
            .WithMessage($"PermanentAddress cannot exceed {MaxAddressLength} characters");

        RuleFor(x => x.HomePhone)
            .MaximumLength(MaxPhoneLength)
            .When(x => !string.IsNullOrEmpty(x.HomePhone))
            .WithMessage($"HomePhone cannot exceed {MaxPhoneLength} characters");

        RuleFor(x => x.MobileNo)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.MobileNo))
            .WithMessage($"MobileNo cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.PersonalEmail)
            .MaximumLength(MaxEmailLength)
            .When(x => !string.IsNullOrEmpty(x.PersonalEmail))
            .WithMessage($"PersonalEmail cannot exceed {MaxEmailLength} characters");

        RuleFor(x => x.InternetMessenger)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.InternetMessenger))
            .WithMessage($"InternetMessenger cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.InternetProfileLink)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.InternetProfileLink))
            .WithMessage($"InternetProfileLink cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AdditionalInfo)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AdditionalInfo))
            .WithMessage($"AdditionalInfo cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.BloodGroup)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.BloodGroup))
            .WithMessage($"BloodGroup cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.OriginalBirthDay)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.OriginalBirthDay))
            .WithMessage($"OriginalBirthDay cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Profilepicture)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Profilepicture))
            .WithMessage($"Profilepicture cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Birthidentification)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Birthidentification))
            .WithMessage($"Birthidentification cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Height)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Height))
            .WithMessage($"Height cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Weight)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Weight))
            .WithMessage($"Weight cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Hobby)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Hobby))
            .WithMessage($"Hobby cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Signature)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Signature))
            .WithMessage($"Signature cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Identificationmark)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Identificationmark))
            .WithMessage($"Identificationmark cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ShortName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.ShortName))
            .WithMessage($"ShortName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.PresentPostCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.PresentPostCode))
            .WithMessage($"PresentPostCode cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.PermanentPostCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.PermanentPostCode))
            .WithMessage($"PermanentPostCode cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.Refempid)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Refempid))
            .WithMessage($"Refempid cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteEmployeeRecord.
/// </summary>
public class DeleteEmployeeRecordValidator : BaseRecordValidator<DeleteEmployeeRecord>
{
    public DeleteEmployeeRecordValidator()
    {
        RuleFor(x => x.HrrecordId)
            .GreaterThan(0)
            .WithMessage("HrrecordId must be greater than 0");

    }
}