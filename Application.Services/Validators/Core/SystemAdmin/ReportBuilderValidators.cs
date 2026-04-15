using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateReportBuilderRecord.
/// </summary>
public class CreateReportBuilderRecordValidator : BaseRecordValidator<CreateReportBuilderRecord>
{
    public CreateReportBuilderRecordValidator()
    {
        RuleFor(x => x.ReportHeader)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ReportHeader))
            .WithMessage($"ReportHeader cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ReportTitle)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ReportTitle))
            .WithMessage($"ReportTitle cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.QueryText)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.QueryText))
            .WithMessage($"QueryText cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.OrderByColumn)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.OrderByColumn))
            .WithMessage($"OrderByColumn cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateReportBuilderRecord.
/// </summary>
public class UpdateReportBuilderRecordValidator : BaseRecordValidator<UpdateReportBuilderRecord>
{
    public UpdateReportBuilderRecordValidator()
    {
        RuleFor(x => x.ReportHeaderId)
            .GreaterThan(0)
            .WithMessage("ReportHeaderId must be greater than 0");

        RuleFor(x => x.ReportHeader)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ReportHeader))
            .WithMessage($"ReportHeader cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ReportTitle)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ReportTitle))
            .WithMessage($"ReportTitle cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.QueryText)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.QueryText))
            .WithMessage($"QueryText cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.OrderByColumn)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.OrderByColumn))
            .WithMessage($"OrderByColumn cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteReportBuilderRecord.
/// </summary>
public class DeleteReportBuilderRecordValidator : BaseRecordValidator<DeleteReportBuilderRecord>
{
    public DeleteReportBuilderRecordValidator()
    {
        RuleFor(x => x.ReportHeaderId)
            .GreaterThan(0)
            .WithMessage("ReportHeaderId must be greater than 0");

    }
}