using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateQueryAnalyzerRecord.
/// </summary>
public class CreateQueryAnalyzerRecordValidator : BaseRecordValidator<CreateQueryAnalyzerRecord>
{
    public CreateQueryAnalyzerRecordValidator()
    {
        RuleFor(x => x.ReportHeader)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ReportHeader))
            .WithMessage($"ReportHeader cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ReportTitle)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ReportTitle))
            .WithMessage($"ReportTitle cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateQueryAnalyzerRecord.
/// </summary>
public class UpdateQueryAnalyzerRecordValidator : BaseRecordValidator<UpdateQueryAnalyzerRecord>
{
    public UpdateQueryAnalyzerRecordValidator()
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

    }
}

/// <summary>
/// Validator for DeleteQueryAnalyzerRecord.
/// </summary>
public class DeleteQueryAnalyzerRecordValidator : BaseRecordValidator<DeleteQueryAnalyzerRecord>
{
    public DeleteQueryAnalyzerRecordValidator()
    {
        RuleFor(x => x.ReportHeaderId)
            .GreaterThan(0)
            .WithMessage("ReportHeaderId must be greater than 0");

    }
}