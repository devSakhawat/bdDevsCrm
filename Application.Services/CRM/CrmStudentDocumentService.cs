using System.Globalization;
using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Extensions;
using bdDevs.Shared.Records.CRM;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services.CRM;
using Domain.Entities.Entities.CRM;
using Domain.Entities.Entities.DMS;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.CRM;

internal sealed class CrmStudentDocumentService : ICrmStudentDocumentService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmStudentDocumentService> _logger;
    private readonly IConfiguration _config;

    public CrmStudentDocumentService(IRepositoryManager repository, ILogger<CrmStudentDocumentService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    public async Task<CrmStudentDocumentDto> CreateAsync(CreateCrmStudentDocumentRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmStudentDocumentRecord));
        var entity = record.MapTo<CrmStudentDocument>();
        int newId = await _repository.CrmStudentDocuments.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        var dto = entity.MapTo<CrmStudentDocumentDto>();
        dto.StudentDocumentId = newId;
        return dto;
    }

    public async Task<CrmStudentDocumentDto> UpdateAsync(UpdateCrmStudentDocumentRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmStudentDocumentRecord));
        _ = await _repository.CrmStudentDocuments.StudentDocumentAsync(record.StudentDocumentId, false, cancellationToken)
            ?? throw new NotFoundException("CrmStudentDocument", "StudentDocumentId", record.StudentDocumentId.ToString());
        var entity = record.MapTo<CrmStudentDocument>();
        _repository.CrmStudentDocuments.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
        await UpdateChecklistAndStudentReadinessAsync(entity.StudentId, entity.DocumentTypeId, entity.Status == 3, cancellationToken, entity.UpdatedBy ?? entity.CreatedBy);
        return entity.MapTo<CrmStudentDocumentDto>();
    }

    public async Task DeleteAsync(DeleteCrmStudentDocumentRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.StudentDocumentId <= 0) throw new BadRequestException("Invalid delete request!");
        var entity = await _repository.CrmStudentDocuments.StudentDocumentAsync(record.StudentDocumentId, true, cancellationToken)
            ?? throw new NotFoundException("CrmStudentDocument", "StudentDocumentId", record.StudentDocumentId.ToString());
        entity.IsDeleted = true;
        entity.UpdatedDate = DateTime.UtcNow;
        _repository.CrmStudentDocuments.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmStudentDocumentDto> StudentDocumentAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => (await _repository.CrmStudentDocuments.StudentDocumentAsync(id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CrmStudentDocument", "StudentDocumentId", id.ToString())).MapTo<CrmStudentDocumentDto>();

    public async Task<IEnumerable<CrmStudentDocumentDto>> StudentDocumentsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmStudentDocuments.StudentDocumentsAsync(trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmStudentDocumentDto>() : Enumerable.Empty<CrmStudentDocumentDto>();
    }

    public async Task<IEnumerable<CrmStudentDocumentDto>> StudentDocumentsByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmStudentDocuments.StudentDocumentsByStudentIdAsync(studentId, trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmStudentDocumentDto>() : Enumerable.Empty<CrmStudentDocumentDto>();
    }

    public async Task<GridEntity<CrmStudentDocumentDto>> StudentDocumentsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT StudentDocumentId, StudentId, DocumentTypeId, BranchId, OriginalFileName, StoredFileName, FileSizeKb, MimeType, Status, RejectionReason, VerifiedBy, VerifiedDate, ExpiryDate, IsDeleted, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmStudentDocument";
        return await _repository.CrmStudentDocuments.AdoGridDataAsync<CrmStudentDocumentDto>(sql, options, "StudentDocumentId DESC", string.Empty, cancellationToken);
    }

    public async Task<CrmStudentDocumentDto> UploadAsync(StudentDocumentUploadRequestDto request, IFormFile file, CancellationToken cancellationToken = default)
    {
        if (request == null) throw new BadRequestException(nameof(StudentDocumentUploadRequestDto));
        if (file == null || file.Length == 0) throw new BadRequestException("A file must be provided.");

        var documentType = await _repository.DmsDocumentTypes.DocumentTypeAsync(request.DocumentTypeId, false, cancellationToken)
            ?? throw new NotFoundException("DmsDocumentType", "DocumentTypeId", request.DocumentTypeId.ToString());
        ValidateFile(file, documentType);

        var configuredStoragePath = _config["CrmDocumentStorage:RootPath"];
        var fallbackStorageRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "Uploads", "StudentDocuments"));
        var configuredStorageRoot = string.IsNullOrWhiteSpace(configuredStoragePath)
            ? fallbackStorageRoot
            : Path.GetFullPath(configuredStoragePath);
        if (!configuredStorageRoot.StartsWith(fallbackStorageRoot, StringComparison.OrdinalIgnoreCase) &&
            !fallbackStorageRoot.StartsWith(configuredStorageRoot, StringComparison.OrdinalIgnoreCase))
        {
            throw new BadRequestException("Configured student document storage path is invalid.");
        }

        var uploadsRoot = Path.GetFullPath(Path.Combine(configuredStorageRoot, request.StudentId.ToString(CultureInfo.InvariantCulture)));
        if (!uploadsRoot.StartsWith(configuredStorageRoot, StringComparison.OrdinalIgnoreCase))
        {
            throw new BadRequestException("Resolved student document storage path is invalid.");
        }
        Directory.CreateDirectory(uploadsRoot);
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var allowedExtensions = (documentType.AcceptedExtensions ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(x => x.StartsWith('.') ? x.ToLowerInvariant() : $".{x.ToLowerInvariant()}")
            .ToList();
        if (allowedExtensions.Any() && !allowedExtensions.Contains(extension))
            throw new BadRequestException($"File type {extension} is not allowed for {documentType.Name}.");
        var storedFileName = $"{Guid.NewGuid():N}{extension}";
        var fullPath = Path.Combine(uploadsRoot, storedFileName);
        await using (var stream = File.Create(fullPath))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        var entity = new CrmStudentDocument
        {
            StudentId = request.StudentId,
            DocumentTypeId = request.DocumentTypeId,
            BranchId = request.BranchId,
            OriginalFileName = file.FileName,
            StoredFileName = storedFileName,
            FileSizeKb = Math.Round(file.Length / 1024m, 2),
            MimeType = file.ContentType ?? "application/octet-stream",
            Status = 2,
            ExpiryDate = request.ExpiryDate,
            IsDeleted = false,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = request.RequestedBy,
        };

        int newId = await _repository.CrmStudentDocuments.CreateAndIdAsync(entity, cancellationToken);
        await _repository.CrmDocumentVerificationHistories.CreateAsync(new CrmDocumentVerificationHistory
        {
            DocumentId = newId,
            OldStatus = 0,
            NewStatus = 2,
            ChangedBy = request.RequestedBy,
            ChangedDate = DateTime.UtcNow,
            Notes = "Uploaded and moved to pending verification"
        }, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        await UpdateChecklistAndStudentReadinessAsync(entity.StudentId, entity.DocumentTypeId, false, cancellationToken, request.RequestedBy, submitted: true);
        entity.StudentDocumentId = newId;
        return entity.MapTo<CrmStudentDocumentDto>();
    }

    public async Task<CrmStudentDocumentDto> ChangeStatusAsync(ChangeCrmStudentDocumentStatusRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(ChangeCrmStudentDocumentStatusRecord));
        var entity = await _repository.CrmStudentDocuments.StudentDocumentAsync(record.StudentDocumentId, true, cancellationToken)
            ?? throw new NotFoundException("CrmStudentDocument", "StudentDocumentId", record.StudentDocumentId.ToString());

        if (record.NewStatus is < 2 or > 4) throw new BadRequestException("Invalid document status transition.");
        if (record.NewStatus == 4 && string.IsNullOrWhiteSpace(record.RejectionReason)) throw new BadRequestException("Rejection reason is required.");

        byte oldStatus = entity.Status;
        entity.Status = record.NewStatus;
        entity.RejectionReason = record.NewStatus == 4 ? record.RejectionReason : null;
        entity.VerifiedBy = record.NewStatus is 3 or 4 ? record.ChangedBy : entity.VerifiedBy;
        entity.VerifiedDate = record.NewStatus is 3 or 4 ? DateTime.UtcNow : entity.VerifiedDate;
        entity.UpdatedBy = record.ChangedBy;
        entity.UpdatedDate = DateTime.UtcNow;
        _repository.CrmStudentDocuments.UpdateByState(entity);
        await _repository.CrmDocumentVerificationHistories.CreateAsync(new CrmDocumentVerificationHistory
        {
            DocumentId = entity.StudentDocumentId,
            OldStatus = oldStatus,
            NewStatus = entity.Status,
            ChangedBy = record.ChangedBy,
            ChangedDate = DateTime.UtcNow,
            Notes = record.NewStatus == 4 ? record.RejectionReason ?? record.Notes : record.Notes
        }, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        await UpdateChecklistAndStudentReadinessAsync(entity.StudentId, entity.DocumentTypeId, entity.Status == 3, cancellationToken, record.ChangedBy, submitted: true);
        return entity.MapTo<CrmStudentDocumentDto>();
    }

    public async Task<int> EscalateRejectedDocumentsAsync(CancellationToken cancellationToken = default)
    {
        var thresholdDays = _config.GetValue<int?>("CrmDocumentJobs:RejectionEscalationDays") ?? 2;
        var documents = (await _repository.CrmStudentDocuments.ListByConditionAsync(
                x => x.Status == 4 && !x.IsDeleted && x.VerifiedDate.HasValue && x.VerifiedDate.Value <= DateTime.UtcNow.AddDays(-thresholdDays),
                x => x.StudentDocumentId,
                false,
                false,
                cancellationToken))
            .ToList();
        foreach (var document in documents)
        {
            bool alreadyEscalated = (await _repository.CrmDocumentVerificationHistories.DocumentVerificationHistoriesByDocumentIdAsync(document.StudentDocumentId, false, cancellationToken))
                .Any(x => (x.Notes ?? string.Empty).Contains("escalated", StringComparison.OrdinalIgnoreCase));
            if (alreadyEscalated) continue;
            await _repository.CrmDocumentVerificationHistories.CreateAsync(new CrmDocumentVerificationHistory
            {
                DocumentId = document.StudentDocumentId,
                OldStatus = document.Status,
                NewStatus = document.Status,
                ChangedBy = document.UpdatedBy ?? document.CreatedBy,
                ChangedDate = DateTime.UtcNow,
                Notes = "Rejected document escalated for follow-up"
            }, cancellationToken);
        }
        await _repository.SaveAsync(cancellationToken);
        return documents.Count;
    }

    private void ValidateFile(IFormFile file, DmsDocumentType documentType)
    {
        var maxFileSizeMb = documentType.MaxFileSizeMb ?? 10;
        if (file.Length > maxFileSizeMb * 1024L * 1024L)
            throw new BadRequestException($"File exceeds {maxFileSizeMb} MB limit.");
    }

    private async Task UpdateChecklistAndStudentReadinessAsync(int studentId, int documentTypeId, bool isVerified, CancellationToken cancellationToken, int changedBy, bool submitted = false)
    {
        var checklist = (await _repository.CrmStudentDocumentChecklists.StudentDocumentChecklistsByStudentIdAsync(studentId, true, cancellationToken))
            .FirstOrDefault(x => x.DocumentTypeId == documentTypeId);
        if (checklist != null)
        {
            checklist.IsSubmitted = checklist.IsSubmitted || submitted;
            checklist.IsVerified = isVerified;
            checklist.UpdatedBy = changedBy;
            checklist.UpdatedDate = DateTime.UtcNow;
            _repository.CrmStudentDocumentChecklists.UpdateByState(checklist);
        }

        var allChecklists = (await _repository.CrmStudentDocumentChecklists.StudentDocumentChecklistsByStudentIdAsync(studentId, false, cancellationToken)).ToList();
        if (!allChecklists.Any()) return;

        var student = await _repository.CrmStudents.CrmStudentAsync(studentId, true, cancellationToken);
        if (student == null) return;

        bool allMandatoryVerified = allChecklists.Where(x => x.IsMandatory).All(x => x.IsVerified);
        student.IsApplicationReady = allMandatoryVerified;
        student.ApplicationReadyDate = allMandatoryVerified ? DateTime.UtcNow : null;
        student.ApplicationReadySetBy = allMandatoryVerified ? changedBy : null;
        student.UpdatedBy = changedBy;
        student.UpdatedDate = DateTime.UtcNow;
        _repository.CrmStudents.UpdateByState(student);
        await _repository.SaveAsync(cancellationToken);
    }
}
