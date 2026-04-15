using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateEmploymentRecord.
/// </summary>
public class CreateEmploymentRecordValidator : BaseRecordValidator<CreateEmploymentRecord>
{
    public CreateEmploymentRecordValidator()
    {
        RuleFor(x => x.EmployeeId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.EmployeeId))
            .WithMessage($"EmployeeId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.TelephoneExtension)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.TelephoneExtension))
            .WithMessage($"TelephoneExtension cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.OfficialEmail)
            .MaximumLength(MaxEmailLength)
            .When(x => !string.IsNullOrEmpty(x.OfficialEmail))
            .WithMessage($"OfficialEmail cannot exceed {MaxEmailLength} characters");

        RuleFor(x => x.EmergencyContactName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.EmergencyContactName))
            .WithMessage($"EmergencyContactName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.EmergencyContactNo)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.EmergencyContactNo))
            .WithMessage($"EmergencyContactNo cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Duties)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Duties))
            .WithMessage($"Duties cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AttendanceCardNo)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AttendanceCardNo))
            .WithMessage($"AttendanceCardNo cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.BankAccountNo)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.BankAccountNo))
            .WithMessage($"BankAccountNo cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Gpfno)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Gpfno))
            .WithMessage($"Gpfno cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Experience)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Experience))
            .WithMessage($"Experience cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.TinNumber)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.TinNumber))
            .WithMessage($"TinNumber cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ContactAddress)
            .MaximumLength(MaxAddressLength)
            .When(x => !string.IsNullOrEmpty(x.ContactAddress))
            .WithMessage($"ContactAddress cannot exceed {MaxAddressLength} characters");

        RuleFor(x => x.JobResponsibilities)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.JobResponsibilities))
            .WithMessage($"JobResponsibilities cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.FunctionalJob)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.FunctionalJob))
            .WithMessage($"FunctionalJob cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.SeparationRemarks)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.SeparationRemarks))
            .WithMessage($"SeparationRemarks cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateEmploymentRecord.
/// </summary>
public class UpdateEmploymentRecordValidator : BaseRecordValidator<UpdateEmploymentRecord>
{
    public UpdateEmploymentRecordValidator()
    {
        RuleFor(x => x.HrrecordId)
            .GreaterThan(0)
            .WithMessage("HrrecordId must be greater than 0");

        RuleFor(x => x.EmployeeId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.EmployeeId))
            .WithMessage($"EmployeeId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.TelephoneExtension)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.TelephoneExtension))
            .WithMessage($"TelephoneExtension cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.OfficialEmail)
            .MaximumLength(MaxEmailLength)
            .When(x => !string.IsNullOrEmpty(x.OfficialEmail))
            .WithMessage($"OfficialEmail cannot exceed {MaxEmailLength} characters");

        RuleFor(x => x.EmergencyContactName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.EmergencyContactName))
            .WithMessage($"EmergencyContactName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.EmergencyContactNo)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.EmergencyContactNo))
            .WithMessage($"EmergencyContactNo cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Duties)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Duties))
            .WithMessage($"Duties cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AttendanceCardNo)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AttendanceCardNo))
            .WithMessage($"AttendanceCardNo cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.BankAccountNo)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.BankAccountNo))
            .WithMessage($"BankAccountNo cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Gpfno)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Gpfno))
            .WithMessage($"Gpfno cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Experience)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Experience))
            .WithMessage($"Experience cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.TinNumber)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.TinNumber))
            .WithMessage($"TinNumber cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ContactAddress)
            .MaximumLength(MaxAddressLength)
            .When(x => !string.IsNullOrEmpty(x.ContactAddress))
            .WithMessage($"ContactAddress cannot exceed {MaxAddressLength} characters");

        RuleFor(x => x.JobResponsibilities)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.JobResponsibilities))
            .WithMessage($"JobResponsibilities cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.FunctionalJob)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.FunctionalJob))
            .WithMessage($"FunctionalJob cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.SeparationRemarks)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.SeparationRemarks))
            .WithMessage($"SeparationRemarks cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteEmploymentRecord.
/// </summary>
public class DeleteEmploymentRecordValidator : BaseRecordValidator<DeleteEmploymentRecord>
{
    public DeleteEmploymentRecordValidator()
    {
        RuleFor(x => x.HrrecordId)
            .GreaterThan(0)
            .WithMessage("HrrecordId must be greater than 0");

    }
}