using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateDocumentParameterMappingRecord.
/// </summary>
public class CreateDocumentParameterMappingRecordValidator : BaseRecordValidator<CreateDocumentParameterMappingRecord>
{
    public CreateDocumentParameterMappingRecordValidator()
    {
    }
}

/// <summary>
/// Validator for UpdateDocumentParameterMappingRecord.
/// </summary>
public class UpdateDocumentParameterMappingRecordValidator : BaseRecordValidator<UpdateDocumentParameterMappingRecord>
{
    public UpdateDocumentParameterMappingRecordValidator()
    {
        RuleFor(x => x.MappingId)
            .GreaterThan(0)
            .WithMessage("MappingId must be greater than 0");

    }
}

/// <summary>
/// Validator for DeleteDocumentParameterMappingRecord.
/// </summary>
public class DeleteDocumentParameterMappingRecordValidator : BaseRecordValidator<DeleteDocumentParameterMappingRecord>
{
    public DeleteDocumentParameterMappingRecordValidator()
    {
        RuleFor(x => x.MappingId)
            .GreaterThan(0)
            .WithMessage("MappingId must be greater than 0");

    }
}