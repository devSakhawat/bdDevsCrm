using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateAssemblyInfoRecord.
/// </summary>
public class CreateAssemblyInfoRecordValidator : BaseRecordValidator<CreateAssemblyInfoRecord>
{
    public CreateAssemblyInfoRecordValidator()
    {
        RuleFor(x => x.AssemblyTitle)
            .NotEmpty()
            .WithMessage("AssemblyTitle is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"AssemblyTitle cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AssemblyDescription)
            .NotEmpty()
            .WithMessage("AssemblyDescription is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"AssemblyDescription cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AssemblyCompany)
            .NotEmpty()
            .WithMessage("AssemblyCompany is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"AssemblyCompany cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AssemblyProduct)
            .NotEmpty()
            .WithMessage("AssemblyProduct is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"AssemblyProduct cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AssemblyCopyright)
            .NotEmpty()
            .WithMessage("AssemblyCopyright is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"AssemblyCopyright cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AssemblyVersion)
            .NotEmpty()
            .WithMessage("AssemblyVersion is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"AssemblyVersion cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ProductBanner)
            .NotEmpty()
            .WithMessage("ProductBanner is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"ProductBanner cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.PoweredBy)
            .NotEmpty()
            .WithMessage("PoweredBy is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"PoweredBy cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.PoweredByUrl)
            .NotEmpty()
            .WithMessage("PoweredByUrl is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"PoweredByUrl cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ProductStyleSheet)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ProductStyleSheet))
            .WithMessage($"ProductStyleSheet cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ApiPath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ApiPath))
            .WithMessage($"ApiPath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CvBankPath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.CvBankPath))
            .WithMessage($"CvBankPath cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateAssemblyInfoRecord.
/// </summary>
public class UpdateAssemblyInfoRecordValidator : BaseRecordValidator<UpdateAssemblyInfoRecord>
{
    public UpdateAssemblyInfoRecordValidator()
    {
        RuleFor(x => x.AssemblyInfoId)
            .GreaterThan(0)
            .WithMessage("AssemblyInfoId must be greater than 0");

        RuleFor(x => x.AssemblyTitle)
            .NotEmpty()
            .WithMessage("AssemblyTitle is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"AssemblyTitle cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AssemblyDescription)
            .NotEmpty()
            .WithMessage("AssemblyDescription is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"AssemblyDescription cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AssemblyCompany)
            .NotEmpty()
            .WithMessage("AssemblyCompany is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"AssemblyCompany cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AssemblyProduct)
            .NotEmpty()
            .WithMessage("AssemblyProduct is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"AssemblyProduct cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AssemblyCopyright)
            .NotEmpty()
            .WithMessage("AssemblyCopyright is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"AssemblyCopyright cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AssemblyVersion)
            .NotEmpty()
            .WithMessage("AssemblyVersion is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"AssemblyVersion cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ProductBanner)
            .NotEmpty()
            .WithMessage("ProductBanner is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"ProductBanner cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.PoweredBy)
            .NotEmpty()
            .WithMessage("PoweredBy is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"PoweredBy cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.PoweredByUrl)
            .NotEmpty()
            .WithMessage("PoweredByUrl is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"PoweredByUrl cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ProductStyleSheet)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ProductStyleSheet))
            .WithMessage($"ProductStyleSheet cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ApiPath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ApiPath))
            .WithMessage($"ApiPath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CvBankPath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.CvBankPath))
            .WithMessage($"CvBankPath cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteAssemblyInfoRecord.
/// </summary>
public class DeleteAssemblyInfoRecordValidator : BaseRecordValidator<DeleteAssemblyInfoRecord>
{
    public DeleteAssemblyInfoRecordValidator()
    {
        RuleFor(x => x.AssemblyInfoId)
            .GreaterThan(0)
            .WithMessage("AssemblyInfoId must be greater than 0");

    }
}