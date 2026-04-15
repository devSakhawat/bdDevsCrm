using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>
/// Validator for CreateCrmStatementOfPurposeRecord.
/// </summary>
public class CreateCrmStatementOfPurposeRecordValidator : BaseRecordValidator<CreateCrmStatementOfPurposeRecord>
{
    public CreateCrmStatementOfPurposeRecordValidator()
    {
        RuleFor(x => x.ApplicantId)
            .GreaterThan(0)
            .WithMessage("ApplicantId must be greater than 0");

        RuleFor(x => x.StatementOfPurposeRemarks)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.StatementOfPurposeRemarks))
            .WithMessage($"StatementOfPurposeRemarks cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.StatementOfPurposeFilePath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.StatementOfPurposeFilePath))
            .WithMessage($"StatementOfPurposeFilePath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for UpdateCrmStatementOfPurposeRecord.
/// </summary>
public class UpdateCrmStatementOfPurposeRecordValidator : BaseRecordValidator<UpdateCrmStatementOfPurposeRecord>
{
    public UpdateCrmStatementOfPurposeRecordValidator()
    {
        RuleFor(x => x.StatementOfPurposeId)
            .GreaterThan(0)
            .WithMessage("StatementOfPurposeId must be greater than 0");

        RuleFor(x => x.ApplicantId)
            .GreaterThan(0)
            .WithMessage("ApplicantId must be greater than 0");

        RuleFor(x => x.StatementOfPurposeRemarks)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.StatementOfPurposeRemarks))
            .WithMessage($"StatementOfPurposeRemarks cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.StatementOfPurposeFilePath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.StatementOfPurposeFilePath))
            .WithMessage($"StatementOfPurposeFilePath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for DeleteCrmStatementOfPurposeRecord.
/// </summary>
public class DeleteCrmStatementOfPurposeRecordValidator : BaseRecordValidator<DeleteCrmStatementOfPurposeRecord>
{
    public DeleteCrmStatementOfPurposeRecordValidator()
    {
        RuleFor(x => x.StatementOfPurposeId)
            .GreaterThan(0)
            .WithMessage("StatementOfPurposeId must be greater than 0");

    }
}
