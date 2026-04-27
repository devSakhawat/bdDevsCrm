using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;
using Domain.Contracts.Services.CRM;
using bdDevs.Shared.DataTransferObjects.CRM;
using Domain.Exceptions;
using Application.Shared.Grid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using bdDevs.Shared.Records.CRM;
using bdDevs.Shared.Extensions;

namespace Application.Services.CRM;

/// <summary>CrmEnquiry service implementing business logic for enquiry management.</summary>
internal sealed class CrmEnquiryService : ICrmEnquiryService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmEnquiryService> _logger;
    private readonly IConfiguration _config;

    public CrmEnquiryService(IRepositoryManager repository, ILogger<CrmEnquiryService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    /// <summary>Creates a new enquiry record.</summary>
    public async Task<CrmEnquiryDto> CreateAsync(CreateCrmEnquiryRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateCrmEnquiryRecord));

        _logger.LogInformation("Creating new Enquiry. Time: {Time}", DateTime.UtcNow);

        CrmEnquiry entity = record.MapTo<CrmEnquiry>();
        int newId = await _repository.CrmEnquiries.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Enquiry created successfully. ID: {Id}, Time: {Time}", newId, DateTime.UtcNow);
        return entity.MapTo<CrmEnquiryDto>() with { EnquiryId = newId };
    }

    /// <summary>Updates an existing enquiry record.</summary>
    public async Task<CrmEnquiryDto> UpdateAsync(UpdateCrmEnquiryRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateCrmEnquiryRecord));

        _ = await _repository.CrmEnquiries
            .FirstOrDefaultAsync(x => x.EnquiryId == record.EnquiryId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("Enquiry", "EnquiryId", record.EnquiryId.ToString());

        _logger.LogInformation("Updating Enquiry. ID: {Id}, Time: {Time}", record.EnquiryId, DateTime.UtcNow);

        CrmEnquiry entity = record.MapTo<CrmEnquiry>();
        _repository.CrmEnquiries.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Enquiry updated successfully. ID: {Id}, Time: {Time}", record.EnquiryId, DateTime.UtcNow);
        return entity.MapTo<CrmEnquiryDto>();
    }

    /// <summary>Deletes an enquiry record.</summary>
    public async Task DeleteAsync(DeleteCrmEnquiryRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.EnquiryId <= 0)
            throw new BadRequestException("Invalid delete request!");

        _ = await _repository.CrmEnquiries
            .FirstOrDefaultAsync(x => x.EnquiryId == record.EnquiryId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Enquiry", "EnquiryId", record.EnquiryId.ToString());

        _logger.LogInformation("Deleting Enquiry. ID: {Id}, Time: {Time}", record.EnquiryId, DateTime.UtcNow);
        await _repository.CrmEnquiries.DeleteAsync(x => x.EnquiryId == record.EnquiryId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        _logger.LogWarning("Enquiry deleted successfully. ID: {Id}, Time: {Time}", record.EnquiryId, DateTime.UtcNow);
    }

    /// <summary>Retrieves a single enquiry record by ID.</summary>
    public async Task<CrmEnquiryDto> EnquiryAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Enquiry. ID: {Id}, Time: {Time}", id, DateTime.UtcNow);
        var entity = await _repository.CrmEnquiries
            .FirstOrDefaultAsync(x => x.EnquiryId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Enquiry", "EnquiryId", id.ToString());
        return entity.MapTo<CrmEnquiryDto>();
    }

    /// <summary>Retrieves all enquiry records.</summary>
    public async Task<IEnumerable<CrmEnquiryDto>> EnquiriesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all Enquiries. Time: {Time}", DateTime.UtcNow);
        var entities = await _repository.CrmEnquiries.CrmEnquiriesAsync(trackChanges, cancellationToken);
        if (!entities.Any())
        {
            _logger.LogWarning("No Enquiries found. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmEnquiryDto>();
        }
        _logger.LogInformation("Enquiries fetched. Count: {Count}, Time: {Time}", entities.Count(), DateTime.UtcNow);
        return entities.MapToList<CrmEnquiryDto>();
    }

    /// <summary>Retrieves a lightweight list of enquiries for dropdown binding.</summary>
    public async Task<IEnumerable<CrmEnquiryDto>> EnquiryForDDLAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Enquiries for DDL. Time: {Time}", DateTime.UtcNow);
        var entities = await _repository.CrmEnquiries.CrmEnquiriesAsync(false, cancellationToken);
        if (!entities.Any())
        {
            _logger.LogWarning("No Enquiries found for DDL. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmEnquiryDto>();
        }
        return entities.MapToList<CrmEnquiryDto>();
    }

    /// <summary>Retrieves a paginated summary grid of enquiries.</summary>
    public async Task<GridEntity<CrmEnquiryDto>> EnquiriesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Enquiries summary grid. Time: {Time}", DateTime.UtcNow);
        const string sql = @"SELECT EnquiryId, LeadId, StudentId, CourseId, InstituteId, CountryId, EnquiryDate, ExpectedIntakeMonth, ExpectedIntakeYear, Notes, IsActive, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmEnquiry";
        const string orderBy = "EnquiryDate DESC";
        return await _repository.CrmEnquiries.AdoGridDataAsync<CrmEnquiryDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }
}
