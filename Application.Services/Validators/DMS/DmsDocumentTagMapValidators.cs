using bdDevs.Shared.Records.DMS;
using FluentValidation;

namespace Application.Services.Validators.DMS;

/// <summary>
/// Validator for CreateDmsDocumentTagMapRecord.
/// </summary>
public class CreateDmsDocumentTagMapRecordValidator : BaseRecordValidator<CreateDmsDocumentTagMapRecord>
{
    public CreateDmsDocumentTagMapRecordValidator()
    {
        RuleFor(x => x.DocumentId)
            .GreaterThan(0)
            .WithMessage("DocumentId must be greater than 0");

        RuleFor(x => x.TagId)
            .GreaterThan(0)
            .WithMessage("TagId must be greater than 0");

    }
}

/// <summary>
/// Validator for UpdateDmsDocumentTagMapRecord.
/// </summary>
public class UpdateDmsDocumentTagMapRecordValidator : BaseRecordValidator<UpdateDmsDocumentTagMapRecord>
{
    public UpdateDmsDocumentTagMapRecordValidator()
    {
        RuleFor(x => x.TagMapId)
            .GreaterThan(0)
            .WithMessage("TagMapId must be greater than 0");

        RuleFor(x => x.DocumentId)
            .GreaterThan(0)
            .WithMessage("DocumentId must be greater than 0");

        RuleFor(x => x.TagId)
            .GreaterThan(0)
            .WithMessage("TagId must be greater than 0");

    }
}

/// <summary>
/// Validator for DeleteDmsDocumentTagMapRecord.
/// </summary>
public class DeleteDmsDocumentTagMapRecordValidator : BaseRecordValidator<DeleteDmsDocumentTagMapRecord>
{
    public DeleteDmsDocumentTagMapRecordValidator()
    {
        RuleFor(x => x.TagMapId)
            .GreaterThan(0)
            .WithMessage("TagMapId must be greater than 0");

    }
}
