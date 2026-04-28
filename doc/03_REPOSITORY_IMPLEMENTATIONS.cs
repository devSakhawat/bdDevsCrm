// ============================================================================
// INFRASTRUCTURE LAYER - REPOSITORIES
// Place these files in: Infrastructure.Repositories/[Module]/
// ============================================================================

using Domain.Entities.Entities;
using Infrastructure.Sql.Context;

// ============================================================================
// FILE 1: Infrastructure.Repositories/Analytics/AnalyticsSnapshotRepository.cs
// ============================================================================

namespace Infrastructure.Repositories.Analytics;

/// <summary>
/// Repository for AnalyticsSnapshot entity.
/// Note: Most queries use AdoExecuteListQueryAsync for complex SQL aggregations.
/// EF Core is used only for create/update of snapshots.
/// </summary>
public class AnalyticsSnapshotRepository : RepositoryBase<AnalyticsSnapshot>, IAnalyticsSnapshotRepository
{
    public AnalyticsSnapshotRepository(CrmContext context) : base(context) { }

    /// <summary>Gets today's snapshot (or null if not yet created).</summary>
    public async Task<AnalyticsSnapshot?> GetTodaySnapshotAsync(
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        var today = DateTime.Today;
        return await FirstOrDefaultAsync(
            x => x.SnapshotDate == today,
            trackChanges,
            cancellationToken);
    }

    /// <summary>Gets snapshot for a specific date.</summary>
    public async Task<AnalyticsSnapshot?> GetSnapshotByDateAsync(
        DateTime date,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        var snapshotDate = date.Date; // Ensure we're comparing dates, not datetimes
        return await FirstOrDefaultAsync(
            x => x.SnapshotDate == snapshotDate,
            trackChanges,
            cancellationToken);
    }

    /// <summary>Gets snapshots for a date range (for trend analysis).</summary>
    public async Task<IEnumerable<AnalyticsSnapshot>> GetSnapshotsByDateRangeAsync(
        DateTime dateFrom,
        DateTime dateTo,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(
            x => x.SnapshotDate >= dateFrom && x.SnapshotDate <= dateTo,
            trackChanges,
            cancellationToken);
    }
}

// ============================================================================
// FILE 2: Infrastructure.Repositories/Analytics/ConversionFunnelRepository.cs
// ============================================================================

namespace Infrastructure.Repositories.Analytics;

/// <summary>
/// Repository for ConversionFunnel entity.
/// Used for detailed lead tracking and attribution analysis.
/// </summary>
public class ConversionFunnelRepository : RepositoryBase<ConversionFunnel>, IConversionFunnelRepository
{
    public ConversionFunnelRepository(CrmContext context) : base(context) { }

    /// <summary>Gets funnel records for a specific lead.</summary>
    public async Task<IEnumerable<ConversionFunnel>> GetFunnelsByLeadAsync(
        int leadId,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(
            x => x.LeadId == leadId,
            orderBy: x => x.EnteredAt,
            trackChanges,
            cancellationToken);
    }

    /// <summary>
    /// Gets conversion funnel breakdown for a date range (with aggregation).
    /// Returns: count at each stage, conversion percentages, etc.
    /// This should use raw SQL for performance.
    /// </summary>
    public async Task<ConversionFunnelSummaryDto> GetFunnelSummaryAsync(
        DateTime dateFrom,
        DateTime dateTo,
        CancellationToken cancellationToken = default)
    {
        const string query = @"
            SELECT 
                CAST(COUNT(DISTINCT CASE WHEN Stage = 'Inquiry' THEN LeadId END) AS INT) AS InquiryCount,
                CAST(COUNT(DISTINCT CASE WHEN Stage = 'Counseling' THEN LeadId END) AS INT) AS CounselingCount,
                CAST(COUNT(DISTINCT CASE WHEN Stage = 'Application' THEN LeadId END) AS INT) AS ApplicationCount,
                CAST(COUNT(DISTINCT CASE WHEN Stage = 'Enrollment' THEN LeadId END) AS INT) AS EnrollmentCount
            FROM ConversionFunnel
            WHERE EnteredAt >= @DateFrom AND EnteredAt <= @DateTo";

        var result = await AdoExecuteSingleDataAsync<ConversionFunnelSummaryDto>(
            query,
            new[] { ("@DateFrom", (object)dateFrom), ("@DateTo", (object)dateTo) },
            cancellationToken);

        return result ?? new ConversionFunnelSummaryDto(0, 0, 0, 0);
    }

    /// <summary>Gets leads attributed to a specific country (for attribution analysis).</summary>
    public async Task<IEnumerable<ConversionFunnel>> GetFunnelsByCountryAsync(
        int countryId,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(
            x => x.CountryId == countryId,
            trackChanges,
            cancellationToken);
    }

    /// <summary>Gets leads attributed to a specific agent (for performance tracking).</summary>
    public async Task<IEnumerable<ConversionFunnel>> GetFunnelsByAgentAsync(
        int agentId,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(
            x => x.AgentId == agentId,
            trackChanges,
            cancellationToken);
    }
}

// ============================================================================
// FILE 3: Infrastructure.Repositories/Email/EmailTemplateRepository.cs
// ============================================================================

namespace Infrastructure.Repositories.Email;

/// <summary>
/// Repository for EmailTemplate entity.
/// Simple CRUD for template management.
/// </summary>
public class EmailTemplateRepository : RepositoryBase<EmailTemplate>, IEmailTemplateRepository
{
    public EmailTemplateRepository(CrmContext context) : base(context) { }

    /// <summary>Gets template by its code (unique identifier).</summary>
    public async Task<EmailTemplate?> GetTemplateByCodeAsync(
        string templateCode,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(
            x => x.TemplateCode == templateCode && x.IsActive,
            trackChanges,
            cancellationToken);
    }

    /// <summary>Gets all active templates.</summary>
    public async Task<IEnumerable<EmailTemplate>> GetActiveTemplatesAsync(
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(
            x => x.IsActive,
            trackChanges,
            cancellationToken);
    }

    /// <summary>Gets templates by category.</summary>
    public async Task<IEnumerable<EmailTemplate>> GetTemplatesByCategoryAsync(
        string category,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(
            x => x.Category == category && x.IsActive,
            trackChanges,
            cancellationToken);
    }
}

// ============================================================================
// FILE 4: Infrastructure.Repositories/Email/EmailLogRepository.cs
// ============================================================================

namespace Infrastructure.Repositories.Email;

/// <summary>
/// Repository for EmailLog entity.
/// Tracks all sent emails for compliance, auditing, and debugging.
/// </summary>
public class EmailLogRepository : RepositoryBase<EmailLog>, IEmailLogRepository
{
    public EmailLogRepository(CrmContext context) : base(context) { }

    /// <summary>Gets email logs for a specific template.</summary>
    public async Task<IEnumerable<EmailLog>> GetLogsByTemplateAsync(
        int templateId,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(
            x => x.EmailTemplateId == templateId,
            orderBy: x => x.SentAt,
            isDescending: true,
            trackChanges,
            cancellationToken);
    }

    /// <summary>Gets failed emails that can be retried.</summary>
    public async Task<IEnumerable<EmailLog>> GetFailedEmailsAsync(
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(
            x => x.Status == "Failed" && x.RetryCount < 3,
            trackChanges,
            cancellationToken);
    }

    /// <summary>Gets email logs for a recipient (audit trail).</summary>
    public async Task<IEnumerable<EmailLog>> GetLogsByRecipientAsync(
        string recipientEmail,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(
            x => x.RecipientEmail == recipientEmail,
            orderBy: x => x.SentAt,
            isDescending: true,
            trackChanges,
            cancellationToken);
    }
}

// ============================================================================
// FILE 5: Infrastructure.Repositories/Visa/VisaApplicationRepository.cs
// ============================================================================

namespace Infrastructure.Repositories.Visa;

/// <summary>
/// Repository for VisaApplication entity.
/// Tracks visa applications per applicant and country.
/// </summary>
public class VisaApplicationRepository : RepositoryBase<VisaApplication>, IVisaApplicationRepository
{
    public VisaApplicationRepository(CrmContext context) : base(context) { }

    /// <summary>Gets visa applications for a specific applicant.</summary>
    public async Task<IEnumerable<VisaApplication>> GetApplicationsByApplicantAsync(
        int applicantId,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(
            x => x.ApplicantId == applicantId,
            trackChanges,
            cancellationToken);
    }

    /// <summary>Gets visa application for a specific applicant and country.</summary>
    public async Task<VisaApplication?> GetApplicationByApplicantAndCountryAsync(
        int applicantId,
        int countryId,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(
            x => x.ApplicantId == applicantId && x.CountryId == countryId,
            trackChanges,
            cancellationToken);
    }

    /// <summary>Gets all applications for a specific country (for country-wide reporting).</summary>
    public async Task<IEnumerable<VisaApplication>> GetApplicationsByCountryAsync(
        int countryId,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(
            x => x.CountryId == countryId,
            trackChanges,
            cancellationToken);
    }

    /// <summary>Gets applications by status (for workflow/processing tracking).</summary>
    public async Task<IEnumerable<VisaApplication>> GetApplicationsByStatusAsync(
        string status,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(
            x => x.Status == status,
            trackChanges,
            cancellationToken);
    }
}

// ============================================================================
// FILE 6: Infrastructure.Repositories/Visa/VisaDocumentRepository.cs
// ============================================================================

namespace Infrastructure.Repositories.Visa;

/// <summary>
/// Repository for VisaDocument entity.
/// Tracks individual documents for each visa application.
/// </summary>
public class VisaDocumentRepository : RepositoryBase<VisaDocument>, IVisaDocumentRepository
{
    public VisaDocumentRepository(CrmContext context) : base(context) { }

    /// <summary>Gets all documents for a visa application.</summary>
    public async Task<IEnumerable<VisaDocument>> GetDocumentsByVisaAppAsync(
        int visaAppId,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(
            x => x.VisaApplicationId == visaAppId,
            trackChanges,
            cancellationToken);
    }

    /// <summary>Gets a specific document by type for a visa application.</summary>
    public async Task<VisaDocument?> GetDocumentByTypeAsync(
        int visaAppId,
        string documentType,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(
            x => x.VisaApplicationId == visaAppId && x.DocumentType == documentType,
            trackChanges,
            cancellationToken);
    }

    /// <summary>Gets pending documents (not yet submitted) for a visa application.</summary>
    public async Task<IEnumerable<VisaDocument>> GetPendingDocumentsAsync(
        int visaAppId,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(
            x => x.VisaApplicationId == visaAppId && 
                 (x.Status == "Required" || x.Status == "Pending"),
            trackChanges,
            cancellationToken);
    }

    /// <summary>Gets documents awaiting verification.</summary>
    public async Task<IEnumerable<VisaDocument>> GetDocumentsAwaitingVerificationAsync(
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(
            x => x.Status == "Submitted",
            orderBy: x => x.SubmittedDate,
            trackChanges,
            cancellationToken);
    }
}

// ============================================================================
// FILE 7: Infrastructure.Repositories/Messaging/MessageLogRepository.cs
// ============================================================================

namespace Infrastructure.Repositories.Messaging;

/// <summary>
/// Repository for MessageLog entity.
/// Tracks SMS and WhatsApp messages for delivery confirmation and cost tracking.
/// </summary>
public class MessageLogRepository : RepositoryBase<MessageLog>, IMessageLogRepository
{
    public MessageLogRepository(CrmContext context) : base(context) { }

    /// <summary>Gets message logs by status (for monitoring delivery).</summary>
    public async Task<IEnumerable<MessageLog>> GetLogsByStatusAsync(
        string status,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(
            x => x.Status == status,
            orderBy: x => x.SentAt,
            trackChanges,
            cancellationToken);
    }

    /// <summary>Gets messages sent to a specific phone number (audit trail).</summary>
    public async Task<IEnumerable<MessageLog>> GetLogsByPhoneAsync(
        string phoneNumber,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(
            x => x.RecipientPhone == phoneNumber,
            orderBy: x => x.SentAt,
            isDescending: true,
            trackChanges,
            cancellationToken);
    }

    /// <summary>Gets messages by channel (SMS vs WhatsApp).</summary>
    public async Task<IEnumerable<MessageLog>> GetLogsByChannelAsync(
        string channel, // "SMS" or "WhatsApp"
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(
            x => x.Channel == channel,
            trackChanges,
            cancellationToken);
    }

    /// <summary>Gets failed messages that should be retried.</summary>
    public async Task<IEnumerable<MessageLog>> GetFailedMessagesForRetryAsync(
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(
            x => x.Status == "Failed" && x.RetryCount < 3 && x.IsRetryable,
            trackChanges,
            cancellationToken);
    }

    /// <summary>Calculates total cost of messages sent in a period.</summary>
    public async Task<decimal> CalculateCostAsync(
        DateTime dateFrom,
        DateTime dateTo,
        string? channel = null,
        CancellationToken cancellationToken = default)
    {
        var messages = await ListByConditionAsync(
            x => x.SentAt >= dateFrom && x.SentAt <= dateTo &&
                 (channel == null || x.Channel == channel) &&
                 x.Status != "Failed",
            trackChanges: false,
            cancellationToken);

        return messages.Sum(m => m.Cost ?? 0);
    }
}

// ============================================================================
// DTO SUPPORT CLASSES (for Repository returns)
// ============================================================================

namespace Infrastructure.Repositories.Analytics;

public record ConversionFunnelSummaryDto(
    int InquiryCount,
    int CounselingCount,
    int ApplicationCount,
    int EnrollmentCount);
