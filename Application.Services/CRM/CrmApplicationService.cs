using Application.Shared.Grid;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Extensions;
using bdDevs.Shared.Records.CRM;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services.CRM;
using Domain.Entities.Entities.CRM;
using Domain.Entities.Entities.System;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.CRM;

internal sealed class CrmApplicationService : ICrmApplicationService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmApplicationService> _logger;
    private readonly IConfiguration _config;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CrmApplicationService(IRepositoryManager repository, ILogger<CrmApplicationService> logger, IConfiguration config, IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _logger = logger;
        _config = config;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CrmApplicationDto> CreateAsync(CreateCrmApplicationRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmApplicationRecord));
        await EnsureStudentReadyAsync(record.StudentId, cancellationToken);

        var entity = record.MapTo<CrmApplication>();
        entity.InternalRefNo = await GenerateInternalRefNoAsync(record.BranchId, record.AppliedDate ?? DateTime.UtcNow, cancellationToken);
        entity.PortalUsername = string.IsNullOrWhiteSpace(record.PortalUsername) ? null : EncryptDecryptHelper.Encrypt(record.PortalUsername);
        entity.PortalPassword = string.IsNullOrWhiteSpace(record.PortalPassword) ? null : EncryptDecryptHelper.Encrypt(record.PortalPassword);
        int newId = await _repository.CrmApplications.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        entity.ApplicationId = newId;
        return entity.MapTo<CrmApplicationDto>();
    }

    public async Task<CrmApplicationDto> UpdateAsync(UpdateCrmApplicationRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmApplicationRecord));
        await EnsureStudentReadyAsync(record.StudentId, cancellationToken);
        _ = await _repository.CrmApplications.CrmApplicationAsync(record.ApplicationId, false, cancellationToken)
            ?? throw new NotFoundException("CrmApplication", "ApplicationId", record.ApplicationId.ToString());
        var entity = record.MapTo<CrmApplication>();
        entity.InternalRefNo = string.IsNullOrWhiteSpace(record.InternalRefNo)
            ? await GenerateInternalRefNoAsync(record.BranchId, record.AppliedDate ?? DateTime.UtcNow, cancellationToken)
            : record.InternalRefNo;
        entity.PortalUsername = string.IsNullOrWhiteSpace(record.PortalUsername) ? null : EncryptDecryptHelper.Encrypt(record.PortalUsername);
        entity.PortalPassword = string.IsNullOrWhiteSpace(record.PortalPassword) ? null : EncryptDecryptHelper.Encrypt(record.PortalPassword);
        _repository.CrmApplications.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmApplicationDto>();
    }

    public async Task DeleteAsync(DeleteCrmApplicationRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.ApplicationId <= 0) throw new BadRequestException("Invalid delete request!");
        var entity = await _repository.CrmApplications.CrmApplicationAsync(record.ApplicationId, true, cancellationToken)
            ?? throw new NotFoundException("CrmApplication", "ApplicationId", record.ApplicationId.ToString());
        entity.IsDeleted = true;
        entity.UpdatedDate = DateTime.UtcNow;
        _repository.CrmApplications.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmApplicationDto> ApplicationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => (await _repository.CrmApplications.CrmApplicationAsync(id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CrmApplication", "ApplicationId", id.ToString())).MapTo<CrmApplicationDto>();

    public async Task<IEnumerable<CrmApplicationDto>> ApplicationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmApplications.CrmApplicationsAsync(trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmApplicationDto>() : Enumerable.Empty<CrmApplicationDto>();
    }

    public async Task<IEnumerable<CrmApplicationDto>> ApplicationsByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmApplications.CrmApplicationsByStudentIdAsync(studentId, trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmApplicationDto>() : Enumerable.Empty<CrmApplicationDto>();
    }

    public async Task<GridEntity<CrmApplicationGridDto>> ApplicationsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT ApplicationId, InternalRefNo, StudentId, BranchId, CountryId, UniversityId, ProgramId, IntakeId, Status, Priority, AppliedDate, OfferReceivedDate, EnrollmentDate, IsDeleted FROM CrmApplication";
        return await _repository.CrmApplications.AdoGridDataAsync<CrmApplicationGridDto>(sql, options, "ApplicationId DESC", string.Empty, cancellationToken);
    }

    public async Task<IEnumerable<CrmApplicationGridDto>> ApplicationsBoardAsync(CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT ApplicationId, InternalRefNo, StudentId, BranchId, CountryId, UniversityId, ProgramId, IntakeId, Status, Priority, AppliedDate, OfferReceivedDate, EnrollmentDate, IsDeleted FROM CrmApplication WHERE IsDeleted = 0";
        return await _repository.CrmApplications.AdoExecuteListQueryAsync<CrmApplicationGridDto>(sql, null, cancellationToken);
    }

    public async Task<CrmApplicationDto> ChangeStatusAsync(ChangeCrmApplicationStatusRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(ChangeCrmApplicationStatusRecord));
        var entity = await _repository.CrmApplications.CrmApplicationAsync(record.ApplicationId, true, cancellationToken)
            ?? throw new NotFoundException("CrmApplication", "ApplicationId", record.ApplicationId.ToString());
        if (!IsAllowedTransition(entity.Status, record.NewStatus)) throw new BadRequestException($"Invalid application status transition from {entity.Status} to {record.NewStatus}.");
        entity.Status = record.NewStatus;
        entity.UpdatedBy = record.ChangedBy;
        entity.UpdatedDate = DateTime.UtcNow;
        if (record.NewStatus == 4 && !entity.OfferReceivedDate.HasValue) entity.OfferReceivedDate = DateTime.UtcNow;
        if (record.NewStatus == 9 && !entity.EnrollmentDate.HasValue) entity.EnrollmentDate = DateTime.UtcNow;
        if (record.NewStatus is 10 or 11 && !entity.WithdrawnDate.HasValue) entity.WithdrawnDate = DateTime.UtcNow;
        _repository.CrmApplications.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmApplicationDto>();
    }

    private async Task EnsureStudentReadyAsync(int studentId, CancellationToken cancellationToken)
    {
        var student = await _repository.CrmStudents.CrmStudentAsync(studentId, false, cancellationToken)
            ?? throw new NotFoundException("Student", "StudentId", studentId.ToString());
        if (!student.IsApplicationReady) throw new BadRequestException("Student is not application ready.");
    }

    private async Task<string> GenerateInternalRefNoAsync(int branchId, DateTime appliedDate, CancellationToken cancellationToken)
    {
        var branch = await _repository.Branches.FirstOrDefaultAsync(x => x.Branchid == branchId, false, cancellationToken);
        var branchCode = string.IsNullOrWhiteSpace(branch?.Branchcode) ? $"BR{branchId}" : branch.Branchcode!.Trim().ToUpperInvariant();
        var year = appliedDate.Year;
        var count = (await _repository.CrmApplications.CrmApplicationsAsync(false, cancellationToken)).Count(x => x.BranchId == branchId && (x.AppliedDate ?? x.CreatedDate).Year == year);
        return $"APP-{branchCode}-{year}-{count + 1:0000}";
    }

    private static bool IsAllowedTransition(byte oldStatus, byte newStatus)
    {
        if (oldStatus == newStatus) return true;
        return oldStatus switch
        {
            1 => newStatus is 2 or 3 or 10 or 11,
            2 => newStatus is 3 or 10 or 11,
            3 => newStatus is 4 or 10 or 11,
            4 => newStatus is 5 or 10 or 11,
            5 => newStatus is 6 or 10 or 11,
            6 => newStatus is 7 or 10 or 11,
            7 => newStatus is 8 or 9 or 10 or 11,
            8 => newStatus is 9 or 10 or 11,
            _ => false
        };
    }
}
