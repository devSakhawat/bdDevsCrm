using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Extensions;
using bdDevs.Shared.Records.CRM;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services.CRM;
using Domain.Entities.Entities.CRM;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.CRM;

internal sealed class CrmScholarshipApplicationService : ICrmScholarshipApplicationService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmScholarshipApplicationService> _logger;
    private readonly IConfiguration _configuration;

    public CrmScholarshipApplicationService(IRepositoryManager repository, ILogger<CrmScholarshipApplicationService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<CrmScholarshipApplicationDto> CreateAsync(CreateCrmScholarshipApplicationRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmScholarshipApplicationRecord));
        _ = await _repository.CrmApplications.CrmApplicationAsync(record.ApplicationId, false, cancellationToken)
            ?? throw new NotFoundException("CrmApplication", "ApplicationId", record.ApplicationId.ToString());
        var entity = record.MapTo<CrmScholarshipApplication>();
        int newId = await _repository.CrmScholarshipApplications.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        entity.ScholarshipApplicationId = newId;
        return entity.MapTo<CrmScholarshipApplicationDto>();
    }

    public async Task<CrmScholarshipApplicationDto> UpdateAsync(UpdateCrmScholarshipApplicationRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmScholarshipApplicationRecord));
        _ = await _repository.CrmScholarshipApplications.ScholarshipApplicationAsync(record.ScholarshipApplicationId, false, cancellationToken)
            ?? throw new NotFoundException("CrmScholarshipApplication", "ScholarshipApplicationId", record.ScholarshipApplicationId.ToString());
        var entity = record.MapTo<CrmScholarshipApplication>();
        _repository.CrmScholarshipApplications.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmScholarshipApplicationDto>();
    }

    public async Task DeleteAsync(DeleteCrmScholarshipApplicationRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.ScholarshipApplicationId <= 0) throw new BadRequestException("Invalid delete request!");
        await _repository.CrmScholarshipApplications.DeleteAsync(x => x.ScholarshipApplicationId == record.ScholarshipApplicationId, false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmScholarshipApplicationDto> ScholarshipApplicationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => (await _repository.CrmScholarshipApplications.ScholarshipApplicationAsync(id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CrmScholarshipApplication", "ScholarshipApplicationId", id.ToString())).MapTo<CrmScholarshipApplicationDto>();

    public async Task<IEnumerable<CrmScholarshipApplicationDto>> ScholarshipApplicationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmScholarshipApplications.ScholarshipApplicationsAsync(trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmScholarshipApplicationDto>() : Enumerable.Empty<CrmScholarshipApplicationDto>();
    }

    public async Task<IEnumerable<CrmScholarshipApplicationDto>> ScholarshipApplicationsByApplicationIdAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmScholarshipApplications.ScholarshipApplicationsByApplicationIdAsync(applicationId, trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmScholarshipApplicationDto>() : Enumerable.Empty<CrmScholarshipApplicationDto>();
    }

    public async Task<GridEntity<CrmScholarshipApplicationDto>> ScholarshipApplicationsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT ScholarshipApplicationId, ApplicationId, ScholarshipName, ScholarshipType, GrantedAmount, Currency, ScholarshipPercentage, ConfirmedDate, ExpiryDate, Status, Notes, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmScholarshipApplication";
        return await _repository.CrmScholarshipApplications.AdoGridDataAsync<CrmScholarshipApplicationDto>(sql, options, "ScholarshipApplicationId DESC", string.Empty, cancellationToken);
    }

    public async Task<CrmScholarshipCommissionImpactDto> CommissionImpactAsync(int applicationId, CancellationToken cancellationToken = default)
    {
        var application = await _repository.CrmApplications.CrmApplicationAsync(applicationId, false, cancellationToken)
            ?? throw new NotFoundException("CrmApplication", "ApplicationId", applicationId.ToString());

        var tuitionAmount = (await _repository.CrmCourseFees.CrmCourseFeesByCourseIdAsync(application.ProgramId, false, cancellationToken))
            .Where(x => x.IntakeId == application.IntakeId && x.FeeType == 1)
            .Sum(x => x.Amount);
        var scholarshipAmount = (await _repository.CrmScholarshipApplications.ScholarshipApplicationsByApplicationIdAsync(applicationId, false, cancellationToken))
            .Where(x => x.Status != 4)
            .Sum(x => x.GrantedAmount);
        var commissionableAmount = Math.Max(0m, tuitionAmount - scholarshipAmount);
        var institute = await _repository.CrmInstitutes.CrmInstituteAsync(application.UniversityId, false, cancellationToken);
        var rate = institute?.CommissionRate ?? 0m;
        var type = institute?.CommissionType;
        var estimatedCommissionAmount = type == 2 ? rate : Math.Round(commissionableAmount * rate / 100m, 2);

        return new CrmScholarshipCommissionImpactDto
        {
            ApplicationId = applicationId,
            TuitionAmount = tuitionAmount,
            ScholarshipAmount = scholarshipAmount,
            CommissionableAmount = commissionableAmount,
            InstituteCommissionRate = rate,
            InstituteCommissionType = type,
            EstimatedCommissionAmount = estimatedCommissionAmount
        };
    }
}
