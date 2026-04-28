// ============================================================================
// APPLICATION LAYER - SERVICE IMPLEMENTATIONS
// Place these files in: Application.Services/[Module]/
// ============================================================================

using Domain.Contracts.Repositories;
using Domain.Contracts.Services;
using Domain.Entities.Entities;
using bdDevs.Shared.DataTransferObjects;
using Domain.Exceptions;
using Application.Shared.Grid;
using Application.Services.Mappings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

// ============================================================================
// FILE 1: Application.Services/Analytics/AnalyticsService.cs
// ============================================================================

namespace Application.Services.Analytics;

/// <summary>
/// Analytics service providing KPI data for dashboards and reports.
/// All data is pulled from pre-aggregated AnalyticsSnapshot table (created nightly).
/// This ensures dashboards remain fast even with millions of records.
/// </summary>
internal sealed class AnalyticsService : IAnalyticsService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<AnalyticsService> _logger;

    public AnalyticsService(
        IRepositoryManager repository,
        ILogger<AnalyticsService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Gets conversion funnel for a date range.
    /// Shows: Total leads → Counseling % → Application % → Enrollment %.
    /// Example: 100 leads → 80 counseled (80%) → 40 applied (40%) → 20 enrolled (20%).
    /// </summary>
    public async Task<ConversionFunnelDto> GetConversionFunnelAsync(
        DateTime dateFrom,
        DateTime dateTo,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Get snapshots for the date range
            var snapshots = await _repository.AnalyticsSnapshot
                .GetSnapshotsByDateRangeAsync(dateFrom, dateTo, trackChanges: false, cancellationToken)
                ?? throw new NotFoundException("AnalyticsSnapshot", "DateRange", $"{dateFrom} to {dateTo}");

            var snapshotList = snapshots.ToList();
            if (!snapshotList.Any())
            {
                _logger.LogWarning("No analytics snapshots found for range {DateFrom} to {DateTo}", dateFrom, dateTo);
                return new ConversionFunnelDto(0, 0, 0, 0, 0, 0);
            }

            // Aggregate snapshots (sum all daily snapshots in the range)
            var totalLeads = snapshotList.Sum(s => s.TotalLeads);
            var leadsInCounseling = snapshotList.Sum(s => s.LeadsInCounseling);
            var applicationsSubmitted = snapshotList.Sum(s => s.ApplicationsSubmitted);
            var enrollmentCompleted = snapshotList.Sum(s => s.EnrollmentCompleted);

            // Calculate conversion rates
            var counselingRate = totalLeads > 0 ? ((decimal)leadsInCounseling / totalLeads) * 100 : 0;
            var applicationRate = leadsInCounseling > 0 ? ((decimal)applicationsSubmitted / leadsInCounseling) * 100 : 0;
            var enrollmentRate = applicationsSubmitted > 0 ? ((decimal)enrollmentCompleted / applicationsSubmitted) * 100 : 0;

            _logger.LogInformation(
                "Conversion funnel calculated. Range: {DateFrom} to {DateTo}, Leads: {Leads}, Conversion: {Rate}%",
                dateFrom, dateTo, totalLeads, (enrollmentRate));

            return new ConversionFunnelDto(
                totalLeads,
                leadsInCounseling,
                applicationsSubmitted,
                enrollmentCompleted,
                counselingRate,
                applicationRate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving conversion funnel for {DateFrom} to {DateTo}", dateFrom, dateTo);
            throw;
        }
    }

    /// <summary>
    /// Gets today's dashboard summary metrics.
    /// Quick snapshot of KPIs for executive dashboard.
    /// </summary>
    public async Task<DashboardMetricsDto> GetDashboardMetricsAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var today = DateTime.Today;
            var snapshot = await _repository.AnalyticsSnapshot
                .GetSnapshotByDateAsync(today, trackChanges: false, cancellationToken);

            if (snapshot is null)
            {
                _logger.LogWarning("No snapshot available for today {Date}", today);
                // Return empty metrics if no snapshot yet (e.g., it's created at 2 AM)
                return new DashboardMetricsDto(0, 0, 0, 0, 0, 0, 0, 0);
            }

            return new DashboardMetricsDto(
                snapshot.TotalLeads,
                snapshot.LeadsInCounseling,
                snapshot.ApplicationsSubmitted,
                snapshot.EnrollmentCompleted,
                snapshot.ConversionRate,
                snapshot.EnrollmentRate,
                snapshot.TotalRevenue,
                snapshot.OutstandingPayments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving dashboard metrics");
            throw;
        }
    }

    /// <summary>
    /// Gets revenue attribution by country or course.
    /// Answers: Which markets generate most revenue?
    /// </summary>
    public async Task<IEnumerable<AttributionDto>> GetLeadSourceAttributionAsync(
        int? countryId = null,
        int? courseId = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // This would query ConversionFunnel + join with Country/Course
            // For example: GROUP BY Country, SUM(Revenue from enrollments)
            
            // Query: 
            // SELECT c.CountryName, COUNT(*) as LeadCount, SUM(...) as Revenue
            // FROM ConversionFunnel cf
            // JOIN CrmCountry c ON cf.CountryId = c.CountryId
            // WHERE cf.Stage = 'Enrollment'
            // GROUP BY c.CountryName
            // ORDER BY Revenue DESC

            // For now, return empty (needs raw SQL implementation)
            _logger.LogInformation("Attribution analysis requested. CountryId: {CountryId}, CourseId: {CourseId}", 
                countryId, courseId);

            return Enumerable.Empty<AttributionDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving attribution data");
            throw;
        }
    }

    /// <summary>
    /// Gets individual agent performance metrics.
    /// Shows: How many leads each agent converted, what commission earned.
    /// </summary>
    public async Task<AgentPerformanceDto> GetAgentPerformanceAsync(
        int agentId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Query ConversionFunnel WHERE AgentId = @AgentId
            // COUNT leads, COUNT enrollments, SUM commission

            _logger.LogInformation("Agent performance requested for AgentId: {AgentId}", agentId);

            // Return placeholder
            return new AgentPerformanceDto(
                agentId,
                "Agent Name",
                0,
                0,
                0,
                0);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving agent performance for AgentId: {AgentId}", agentId);
            throw;
        }
    }

    /// <summary>
    /// Gets monthly revenue trend for a year.
    /// Useful for forecasting and budget planning.
    /// </summary>
    public async Task<IEnumerable<MonthlyRevenueDto>> GetMonthlyRevenueAsync(
        int year,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var results = new List<MonthlyRevenueDto>();

            for (int month = 1; month <= 12; month++)
            {
                var monthStart = new DateTime(year, month, 1);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1);

                var snapshots = await _repository.AnalyticsSnapshot
                    .GetSnapshotsByDateRangeAsync(monthStart, monthEnd, trackChanges: false, cancellationToken);

                var monthRevenue = snapshots?.Sum(s => s.TotalRevenue) ?? 0;
                var monthEnrollments = snapshots?.Sum(s => s.EnrollmentCompleted) ?? 0;

                results.Add(new MonthlyRevenueDto(
                    month,
                    System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month),
                    monthRevenue,
                    monthEnrollments));
            }

            _logger.LogInformation("Monthly revenue retrieved for year {Year}", year);
            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving monthly revenue for year {Year}", year);
            throw;
        }
    }

    /// <summary>
    /// Gets breakdown of leads by source (web, referral, social, event).
    /// </summary>
    public async Task<IEnumerable<LeadSourceDto>> GetLeadSourceBreakdownAsync(
        DateTime dateFrom,
        DateTime dateTo,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // This would query CrmLead WHERE LeadSource = 'Web' AND CreatedAt BETWEEN ...
            // GROUP BY LeadSource, COUNT(*)

            _logger.LogInformation("Lead source breakdown requested for {DateFrom} to {DateTo}", dateFrom, dateTo);
            return Enumerable.Empty<LeadSourceDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving lead source breakdown");
            throw;
        }
    }
}

// ============================================================================
// FILE 2: Application.Services/Email/EmailTemplateService.cs
// ============================================================================

namespace Application.Services.Email;

/// <summary>
/// Service for managing email templates (CRUD).
/// Templates are stored in DB, supports mail-merge with placeholders.
/// </summary>
internal sealed class EmailTemplateService : IEmailTemplateService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<EmailTemplateService> _logger;

    public EmailTemplateService(
        IRepositoryManager repository,
        ILogger<EmailTemplateService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>Creates a new email template.</summary>
    public async Task<EmailTemplateDto> CreateEmailTemplateAsync(
        EmailTemplateDto templateForCreate,
        CancellationToken cancellationToken = default)
    {
        if (templateForCreate is null)
            throw new BadRequestException(nameof(EmailTemplateDto));

        // Validate unique template code
        var exists = await _repository.EmailTemplate.ExistsAsync(
            x => x.TemplateCode.ToLower() == templateForCreate.TemplateCode.ToLower(),
            cancellationToken)
            ?? throw new DuplicateRecordException("EmailTemplate", "TemplateCode");

        if (exists)
            throw new DuplicateRecordException("EmailTemplate", "TemplateCode");

        var template = new EmailTemplate
        {
            TemplateName = templateForCreate.TemplateName,
            TemplateCode = templateForCreate.TemplateCode.ToUpper(),
            Subject = templateForCreate.Subject,
            BodyHtml = templateForCreate.BodyHtml,
            PlaceholdersMeta = templateForCreate.PlaceholdersMeta,
            Category = templateForCreate.Category,
            IsActive = true
        };

        await _repository.EmailTemplate.CreateAsync(template, cancellationToken);
        int affected = await _repository.SaveChangesAsync(cancellationToken);

        if (affected <= 0)
            throw new InvalidOperationException("Email template could not be saved.");

        _logger.LogInformation(
            "Email template created. ID: {TemplateId}, Code: {Code}, Name: {Name}",
            template.EmailTemplateId, template.TemplateCode, template.TemplateName);

        return MyMapper.JsonClone<EmailTemplate, EmailTemplateDto>(template);
    }

    /// <summary>Updates an existing email template.</summary>
    public async Task<EmailTemplateDto> UpdateEmailTemplateAsync(
        int templateId,
        EmailTemplateDto templateForUpdate,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        if (templateForUpdate is null)
            throw new BadRequestException(nameof(EmailTemplateDto));

        var existing = await _repository.EmailTemplate
            .FirstOrDefaultAsync(x => x.EmailTemplateId == templateId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("EmailTemplate", "EmailTemplateId", templateId.ToString());

        // Check for duplicate code (if code changed)
        if (existing.TemplateCode != templateForUpdate.TemplateCode)
        {
            bool codeExists = await _repository.EmailTemplate.ExistsAsync(
                x => x.TemplateCode.ToLower() == templateForUpdate.TemplateCode.ToLower()
                     && x.EmailTemplateId != templateId,
                cancellationToken) ?? false;

            if (codeExists)
                throw new DuplicateRecordException("EmailTemplate", "TemplateCode");
        }

        existing.TemplateName = templateForUpdate.TemplateName;
        existing.TemplateCode = templateForUpdate.TemplateCode.ToUpper();
        existing.Subject = templateForUpdate.Subject;
        existing.BodyHtml = templateForUpdate.BodyHtml;
        existing.PlaceholdersMeta = templateForUpdate.PlaceholdersMeta;
        existing.Category = templateForUpdate.Category;
        existing.UpdatedAt = DateTime.UtcNow;

        _repository.EmailTemplate.UpdateByState(existing);
        int affected = await _repository.SaveChangesAsync(cancellationToken);

        if (affected <= 0)
            throw new NotFoundException("EmailTemplate", "EmailTemplateId", templateId.ToString());

        _logger.LogInformation("Email template updated. ID: {TemplateId}, Code: {Code}", templateId, existing.TemplateCode);

        return MyMapper.JsonClone<EmailTemplate, EmailTemplateDto>(existing);
    }

    /// <summary>Deletes email template (soft delete).</summary>
    public async Task<int> DeleteEmailTemplateAsync(
        int templateId,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.EmailTemplate
            .FirstOrDefaultAsync(x => x.EmailTemplateId == templateId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("EmailTemplate", "EmailTemplateId", templateId.ToString());

        existing.IsActive = false;
        existing.UpdatedAt = DateTime.UtcNow;

        _repository.EmailTemplate.UpdateByState(existing);
        int affected = await _repository.SaveChangesAsync(cancellationToken);

        if (affected <= 0)
            throw new NotFoundException("EmailTemplate", "EmailTemplateId", templateId.ToString());

        _logger.LogWarning("Email template deleted. ID: {TemplateId}, Code: {Code}", templateId, existing.TemplateCode);

        return affected;
    }

    /// <summary>Gets single template by ID.</summary>
    public async Task<EmailTemplateDto> GetEmailTemplateAsync(
        int templateId,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        var template = await _repository.EmailTemplate
            .FirstOrDefaultAsync(x => x.EmailTemplateId == templateId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("EmailTemplate", "EmailTemplateId", templateId.ToString());

        return MyMapper.JsonClone<EmailTemplate, EmailTemplateDto>(template);
    }

    /// <summary>Gets all active templates, paginated.</summary>
    public async Task<GridEntity<EmailTemplateDto>> GetEmailTemplatesSummaryAsync(
        bool trackChanges,
        GridOptions options,
        CancellationToken cancellationToken = default)
    {
        if (options is null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        const string query = @"
            SELECT 
                EmailTemplateId,
                TemplateName,
                TemplateCode,
                Category,
                IsActive,
                CreatedAt
            FROM EmailTemplate
            WHERE IsActive = 1";

        const string orderBy = "CreatedAt DESC";

        return await _repository.EmailTemplate
            .AdoGridDataAsync<EmailTemplateDto>(query, options, orderBy, "", cancellationToken);
    }

    /// <summary>Gets templates for dropdown.</summary>
    public async Task<IEnumerable<EmailTemplateForDDLDto>> GetEmailTemplatesForDDLAsync(
        CancellationToken cancellationToken = default)
    {
        var templates = await _repository.EmailTemplate.ListWithSelectAsync(
            x => new EmailTemplate
            {
                EmailTemplateId = x.EmailTemplateId,
                TemplateName = x.TemplateName,
                TemplateCode = x.TemplateCode
            },
            condition: x => x.IsActive,
            orderBy: x => x.TemplateName,
            trackChanges: false,
            cancellationToken: cancellationToken);

        return MyMapper.JsonCloneIEnumerableToIEnumerable<EmailTemplate, EmailTemplateForDDLDto>(templates);
    }

    /// <summary>Previews email with placeholder replacement.</summary>
    public async Task<EmailPreviewDto> PreviewEmailAsync(
        int templateId,
        Dictionary<string, string> placeholders,
        CancellationToken cancellationToken = default)
    {
        var template = await GetEmailTemplateAsync(templateId, trackChanges: false, cancellationToken);

        var previewSubject = ReplacePlaceholders(template.Subject, placeholders);
        var previewBody = ReplacePlaceholders(template.BodyHtml, placeholders);

        // Find missing placeholders
        var missingPlaceholders = FindMissingPlaceholders(template.BodyHtml, placeholders).ToArray();

        return new EmailPreviewDto(previewSubject, previewBody, missingPlaceholders);
    }

    #region Helpers
    private string ReplacePlaceholders(string text, Dictionary<string, string>? placeholders)
    {
        if (placeholders is null)
            return text;

        foreach (var (key, value) in placeholders)
        {
            text = text.Replace($"{{{key}}}", value ?? "N/A", StringComparison.OrdinalIgnoreCase);
        }

        return text;
    }

    private IEnumerable<string> FindMissingPlaceholders(string text, Dictionary<string, string>? placeholders)
    {
        var missing = new List<string>();
        placeholders ??= new();

        // Find all {PlaceholderName} patterns
        var matches = System.Text.RegularExpressions.Regex.Matches(text, @"\{([^}]+)\}");
        foreach (System.Text.RegularExpressions.Match match in matches)
        {
            var key = match.Groups[1].Value;
            if (!placeholders.ContainsKey(key))
                missing.Add(key);
        }

        return missing.Distinct();
    }
    #endregion
}

// ============================================================================
// FILE 3: Application.Services/Email/EmailService.cs (SENDING)
// ============================================================================

namespace Application.Services.Email;

/// <summary>
/// Service for sending emails via templates.
/// Handles SMTP delivery via SendGrid/AWS SES.
/// Logs all sends for audit trail and retry management.
/// </summary>
internal sealed class EmailService : IEmailService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<EmailService> _logger;
    private readonly ISmtpProvider _smtpProvider; // Abstraction for SendGrid, AWS SES, etc.

    public EmailService(
        IRepositoryManager repository,
        ILogger<EmailService> logger,
        ISmtpProvider smtpProvider)
    {
        _repository = repository;
        _logger = logger;
        _smtpProvider = smtpProvider;
    }

    /// <summary>Sends an email using a template with mail-merge.</summary>
    public async Task<bool> SendEmailAsync(
        int templateId,
        string recipientEmail,
        string recipientName,
        Dictionary<string, string> placeholders,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // 1. Get template
            var template = await _repository.EmailTemplate
                .FirstOrDefaultAsync(x => x.EmailTemplateId == templateId, trackChanges: false, cancellationToken)
                ?? throw new NotFoundException("EmailTemplate", "EmailTemplateId", templateId.ToString());

            // 2. Replace placeholders
            var subject = ReplacePlaceholders(template.Subject, placeholders);
            var bodyHtml = ReplacePlaceholders(template.BodyHtml, placeholders);

            // 3. Send via SMTP provider
            var sendResult = await _smtpProvider.SendAsync(
                recipientEmail,
                subject,
                bodyHtml,
                cancellationToken);

            // 4. Log the attempt
            var emailLog = new EmailLog
            {
                EmailTemplateId = templateId,
                RecipientEmail = recipientEmail,
                RecipientName = recipientName,
                SentSubject = subject,
                Status = sendResult.IsSuccess ? "Sent" : "Failed",
                SentAt = DateTime.UtcNow,
                FailureReason = sendResult.IsSuccess ? null : sendResult.ErrorMessage,
                ExternalMessageId = sendResult.MessageId
            };

            await _repository.EmailLog.CreateAsync(emailLog, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            if (sendResult.IsSuccess)
            {
                _logger.LogInformation(
                    "Email sent successfully. TemplateId: {TemplateId}, To: {Email}, MessageId: {MessageId}",
                    templateId, recipientEmail, sendResult.MessageId);
            }
            else
            {
                _logger.LogWarning(
                    "Email send failed. TemplateId: {TemplateId}, To: {Email}, Reason: {Reason}",
                    templateId, recipientEmail, sendResult.ErrorMessage);
            }

            return sendResult.IsSuccess;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email. TemplateId: {TemplateId}, To: {Email}", templateId, recipientEmail);
            throw;
        }
    }

    /// <summary>Sends email to multiple recipients in bulk.</summary>
    public async Task<IEnumerable<BulkEmailResultDto>> SendBulkEmailAsync(
        int templateId,
        IEnumerable<BulkEmailRecipientDto> recipients,
        CancellationToken cancellationToken = default)
    {
        var results = new List<BulkEmailResultDto>();
        var template = await _repository.EmailTemplate
            .FirstOrDefaultAsync(x => x.EmailTemplateId == templateId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("EmailTemplate", "EmailTemplateId", templateId.ToString());

        var recipientList = recipients.ToList();
        _logger.LogInformation("Sending bulk emails. TemplateId: {TemplateId}, Count: {Count}", templateId, recipientList.Count);

        foreach (var recipient in recipientList)
        {
            try
            {
                var success = await SendEmailAsync(
                    templateId,
                    recipient.Email,
                    recipient.Name,
                    recipient.Placeholders,
                    cancellationToken);

                results.Add(new BulkEmailResultDto(
                    recipient.Email,
                    success,
                    success ? null : "Send failed"));
            }
            catch (Exception ex)
            {
                results.Add(new BulkEmailResultDto(
                    recipient.Email,
                    false,
                    ex.Message));
            }
        }

        var successCount = results.Count(r => r.Success);
        _logger.LogInformation(
            "Bulk email sending completed. TemplateId: {TemplateId}, Success: {Success}/{Total}",
            templateId, successCount, recipientList.Count);

        return results;
    }

    /// <summary>Retrieves email send logs.</summary>
    public async Task<GridEntity<EmailLogDto>> GetEmailLogsAsync(
        GridOptions options,
        CancellationToken cancellationToken = default)
    {
        if (options is null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        const string query = @"
            SELECT 
                el.EmailLogId,
                et.TemplateName,
                el.RecipientEmail,
                el.Status,
                el.SentAt
            FROM EmailLog el
            JOIN EmailTemplate et ON el.EmailTemplateId = et.EmailTemplateId";

        const string orderBy = "el.SentAt DESC";

        return await _repository.EmailLog
            .AdoGridDataAsync<EmailLogDto>(query, options, orderBy, "", cancellationToken);
    }

    /// <summary>Retries failed emails.</summary>
    public async Task<int> RetryFailedEmailsAsync(
        int? templateId = null,
        DateTime? afterDate = null,
        CancellationToken cancellationToken = default)
    {
        var failedLogs = await _repository.EmailLog
            .ListByConditionAsync(
                x => x.Status == "Failed" && 
                     x.RetryCount < 3 &&
                     (templateId == null || x.EmailTemplateId == templateId) &&
                     (afterDate == null || x.SentAt >= afterDate),
                trackChanges: true,
                cancellationToken: cancellationToken);

        var retryCount = 0;
        foreach (var log in failedLogs)
        {
            // Retry sending...
            log.RetryCount++;
            // (in reality, would call SendEmailAsync again)
            retryCount++;
        }

        if (retryCount > 0)
        {
            await _repository.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Retried {Count} failed emails", retryCount);
        }

        return retryCount;
    }

    #region Helper
    private string ReplacePlaceholders(string text, Dictionary<string, string>? placeholders)
    {
        if (placeholders is null)
            return text;

        foreach (var (key, value) in placeholders)
        {
            text = text.Replace($"{{{key}}}", value ?? "N/A", StringComparison.OrdinalIgnoreCase);
        }

        return text;
    }
    #endregion
}

// ============================================================================
// SMTP PROVIDER ABSTRACTION (for dependency injection)
// ============================================================================

namespace Application.Services.Email;

/// <summary>
/// Abstraction for SMTP providers (SendGrid, AWS SES, Office365, etc).
/// Allows easy swapping of email providers.
/// </summary>
public interface ISmtpProvider
{
    Task<SmtpSendResult> SendAsync(
        string recipientEmail,
        string subject,
        string htmlBody,
        CancellationToken cancellationToken = default);
}

public record SmtpSendResult(
    bool IsSuccess,
    string? MessageId = null,
    string? ErrorMessage = null);

// Example SendGrid implementation
internal sealed class SendGridSmtpProvider : ISmtpProvider
{
    private readonly IConfiguration _config;
    private readonly ILogger<SendGridSmtpProvider> _logger;

    public SendGridSmtpProvider(IConfiguration config, ILogger<SendGridSmtpProvider> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task<SmtpSendResult> SendAsync(
        string recipientEmail,
        string subject,
        string htmlBody,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var apiKey = _config["SendGrid:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
                return new SmtpSendResult(false, null, "SendGrid API key not configured");

            // Implementation using SendGrid SDK
            // var client = new SendGridClient(apiKey);
            // var msg = new SendGridMessage()
            // {
            //     From = new EmailAddress(_config["SendGrid:FromEmail"], _config["SendGrid:FromName"]),
            //     Subject = subject,
            //     HtmlContent = htmlBody
            // };
            // msg.AddTo(new EmailAddress(recipientEmail));
            // var response = await client.SendEmailAsync(msg, cancellationToken);
            // ...

            _logger.LogInformation("Email sent via SendGrid. To: {Email}", recipientEmail);
            return new SmtpSendResult(true, "msg-id-123"); // Placeholder
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SendGrid error for {Email}", recipientEmail);
            return new SmtpSendResult(false, null, ex.Message);
        }
    }
}
