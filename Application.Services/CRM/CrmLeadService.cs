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

/// <summary>CrmLead service implementing business logic for lead management.</summary>
internal sealed class CrmLeadService : ICrmLeadService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmLeadService> _logger;
    private readonly IConfiguration _config;

    public CrmLeadService(IRepositoryManager repository, ILogger<CrmLeadService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    /// <summary>Creates a new lead record.</summary>
    public async Task<CrmLeadDto> CreateAsync(CreateCrmLeadRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateCrmLeadRecord));

        _logger.LogInformation("Creating new Lead. Time: {Time}", DateTime.UtcNow);

        CrmLead entity = record.MapTo<CrmLead>();
        int newId = await _repository.CrmLeads.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Lead created successfully. ID: {Id}, Time: {Time}", newId, DateTime.UtcNow);
        return entity.MapTo<CrmLeadDto>() with { LeadId = newId };
    }

    /// <summary>Updates an existing lead record.</summary>
    public async Task<CrmLeadDto> UpdateAsync(UpdateCrmLeadRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateCrmLeadRecord));

        _ = await _repository.CrmLeads
            .FirstOrDefaultAsync(x => x.LeadId == record.LeadId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("Lead", "LeadId", record.LeadId.ToString());

        _logger.LogInformation("Updating Lead. ID: {Id}, Time: {Time}", record.LeadId, DateTime.UtcNow);

        CrmLead entity = record.MapTo<CrmLead>();
        _repository.CrmLeads.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Lead updated successfully. ID: {Id}, Time: {Time}", record.LeadId, DateTime.UtcNow);
        return entity.MapTo<CrmLeadDto>();
    }

    /// <summary>Deletes a lead record.</summary>
    public async Task DeleteAsync(DeleteCrmLeadRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.LeadId <= 0)
            throw new BadRequestException("Invalid delete request!");

        _ = await _repository.CrmLeads
            .FirstOrDefaultAsync(x => x.LeadId == record.LeadId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Lead", "LeadId", record.LeadId.ToString());

        _logger.LogInformation("Deleting Lead. ID: {Id}, Time: {Time}", record.LeadId, DateTime.UtcNow);
        await _repository.CrmLeads.DeleteAsync(x => x.LeadId == record.LeadId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        _logger.LogWarning("Lead deleted successfully. ID: {Id}, Time: {Time}", record.LeadId, DateTime.UtcNow);
    }

    /// <summary>Retrieves a single lead record by ID.</summary>
    public async Task<CrmLeadDto> LeadAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Lead. ID: {Id}, Time: {Time}", id, DateTime.UtcNow);
        var entity = await _repository.CrmLeads
            .FirstOrDefaultAsync(x => x.LeadId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Lead", "LeadId", id.ToString());
        return entity.MapTo<CrmLeadDto>();
    }

    /// <summary>Retrieves all lead records.</summary>
    public async Task<IEnumerable<CrmLeadDto>> LeadsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all Leads. Time: {Time}", DateTime.UtcNow);
        var entities = await _repository.CrmLeads.CrmLeadsAsync(trackChanges, cancellationToken);
        if (!entities.Any())
        {
            _logger.LogWarning("No Leads found. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmLeadDto>();
        }
        _logger.LogInformation("Leads fetched. Count: {Count}, Time: {Time}", entities.Count(), DateTime.UtcNow);
        return entities.MapToList<CrmLeadDto>();
    }

    /// <summary>Retrieves a lightweight list of leads for dropdown binding.</summary>
    public async Task<IEnumerable<CrmLeadDto>> LeadForDDLAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Leads for DDL. Time: {Time}", DateTime.UtcNow);
        var entities = await _repository.CrmLeads.CrmLeadsAsync(false, cancellationToken);
        if (!entities.Any())
        {
            _logger.LogWarning("No Leads found for DDL. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmLeadDto>();
        }
        return entities.MapToList<CrmLeadDto>();
    }

    /// <summary>Retrieves a paginated summary grid of leads.</summary>
    public async Task<GridEntity<CrmLeadDto>> LeadsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Leads summary grid. Time: {Time}", DateTime.UtcNow);
        const string sql = @"SELECT LeadId, LeadName, Email, Phone, LeadSourceId, LeadStatusId, AssignedCounselorId, AgentId, CountryOfInterest, CourseOfInterest, Budget, Notes, IsActive, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmLead";
        const string orderBy = "LeadName ASC";
        return await _repository.CrmLeads.AdoGridDataAsync<CrmLeadDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }
}
