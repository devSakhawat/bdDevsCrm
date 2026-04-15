using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateDocumentQueryMappingRecord.
/// </summary>
public class CreateDocumentQueryMappingRecordValidator : BaseRecordValidator<CreateDocumentQueryMappingRecord>
{
    public CreateDocumentQueryMappingRecordValidator()
    {
        RuleFor(x => x.ReportHeaderId)
            .GreaterThan(0)
            .WithMessage("ReportHeaderId must be greater than 0");

        RuleFor(x => x.DocumentTypeId)
            .GreaterThan(0)
            .WithMessage("DocumentTypeId must be greater than 0");

        RuleFor(x => x.ParameterDefination)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ParameterDefination))
            .WithMessage($"ParameterDefination cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateDocumentQueryMappingRecord.
/// </summary>
public class UpdateDocumentQueryMappingRecordValidator : BaseRecordValidator<UpdateDocumentQueryMappingRecord>
{
    public UpdateDocumentQueryMappingRecordValidator()
    {
        RuleFor(x => x.DocumentQueryId)
            .GreaterThan(0)
            .WithMessage("DocumentQueryId must be greater than 0");

        RuleFor(x => x.ReportHeaderId)
            .GreaterThan(0)
            .WithMessage("ReportHeaderId must be greater than 0");

        RuleFor(x => x.DocumentTypeId)
            .GreaterThan(0)
            .WithMessage("DocumentTypeId must be greater than 0");

        RuleFor(x => x.ParameterDefination)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ParameterDefination))
            .WithMessage($"ParameterDefination cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteDocumentQueryMappingRecord.
/// </summary>
public class DeleteDocumentQueryMappingRecordValidator : BaseRecordValidator<DeleteDocumentQueryMappingRecord>
{
    public DeleteDocumentQueryMappingRecordValidator()
    {
        RuleFor(x => x.DocumentQueryId)
            .GreaterThan(0)
            .WithMessage("DocumentQueryId must be greater than 0");

    }
}