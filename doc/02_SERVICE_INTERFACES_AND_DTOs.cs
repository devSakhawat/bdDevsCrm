// ============================================================================
// SERVICE CONTRACTS (INTERFACES)
// Place these files in: Domain.Contracts/Services/[Module]/
// ============================================================================

using bdDevs.Shared.DataTransferObjects;
using Application.Shared.Grid;

// ============================================================================
// FILE 1: Domain.Contracts/Services/IAnalyticsService.cs
// ============================================================================

namespace Domain.Contracts.Services.Analytics;

/// <summary>
/// Service for dashboard analytics and reporting.
/// Provides conversion funnels, revenue attribution, and agent performance metrics.
/// All queries use AnalyticsSnapshot table (pre-aggregated nightly) for performance.
/// </summary>
public interface IAnalyticsService
{
    /// <summary>
    /// Retrieves conversion funnel data for a date range.
    /// Shows: How many leads → counseled → applied → enrolled.
    /// Used for: Dashboard funnel chart, conversion rate calculation.
    /// </summary>
    /// <param name="dateFrom">Start date (inclusive).</param>
    /// <param name="dateTo">End date (inclusive).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Funnel breakdown by stage with counts and rates.</returns>
    Task<ConversionFunnelDto> GetConversionFunnelAsync(
        DateTime dateFrom,
        DateTime dateTo,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets dashboard summary metrics (KPIs).
    /// Today's snapshot: total leads, conversion %, revenue, outstanding payments.
    /// Used for: Dashboard summary cards, executive reporting.
    /// </summary>
    /// <returns>Today's key metrics.</returns>
    Task<DashboardMetricsDto> GetDashboardMetricsAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Revenue attribution by country or course.
    /// Shows which countries/courses generate most commission.
    /// Used for: Market analysis, resource allocation decisions.
    /// </summary>
    /// <param name="countryId">Optional: filter by specific country.</param>
    /// <param name="courseId">Optional: filter by specific course.</param>
    Task<IEnumerable<AttributionDto>> GetLeadSourceAttributionAsync(
        int? countryId = null,
        int? courseId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Individual agent/counselor performance metrics.
    /// Shows: leads handled, enrollments, commission earned, conversion %.
    /// Used for: Agent performance reviews, commission calculations, leaderboards.
    /// </summary>
    Task<AgentPerformanceDto> GetAgentPerformanceAsync(
        int agentId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Monthly revenue trend for a specific year.
    /// Shows: Month-by-month revenue progression.
    /// Used for: Revenue forecasting, budget planning, seasonal analysis.
    /// </summary>
    Task<IEnumerable<MonthlyRevenueDto>> GetMonthlyRevenueAsync(
        int year,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lead source breakdown: how many leads came from web, referral, social, events, etc.
    /// Used for: Marketing ROI analysis, channel effectiveness.
    /// </summary>
    Task<IEnumerable<LeadSourceDto>> GetLeadSourceBreakdownAsync(
        DateTime dateFrom,
        DateTime dateTo,
        CancellationToken cancellationToken = default);
}

// ============================================================================
// FILE 2: Domain.Contracts/Services/IEmailTemplateService.cs
// ============================================================================

namespace Domain.Contracts.Services.Email;

/// <summary>
/// Service for managing email templates.
/// CRUD operations for templates, retrieving available placeholders, previewing emails.
/// </summary>
public interface IEmailTemplateService
{
    /// <summary>Creates a new email template.</summary>
    /// <param name="templateForCreate">Template data with name, subject, body, etc.</param>
    /// <returns>Created template with ID.</returns>
    Task<EmailTemplateDto> CreateEmailTemplateAsync(
        EmailTemplateDto templateForCreate,
        CancellationToken cancellationToken = default);

    /// <summary>Updates an existing email template.</summary>
    Task<EmailTemplateDto> UpdateEmailTemplateAsync(
        int templateId,
        EmailTemplateDto templateForUpdate,
        bool trackChanges,
        CancellationToken cancellationToken = default);

    /// <summary>Deletes an email template (soft delete - sets IsActive = false).</summary>
    Task<int> DeleteEmailTemplateAsync(
        int templateId,
        bool trackChanges,
        CancellationToken cancellationToken = default);

    /// <summary>Gets single template by ID.</summary>
    Task<EmailTemplateDto> GetEmailTemplateAsync(
        int templateId,
        bool trackChanges,
        CancellationToken cancellationToken = default);

    /// <summary>Gets all active templates, paginated.</summary>
    Task<GridEntity<EmailTemplateDto>> GetEmailTemplatesSummaryAsync(
        bool trackChanges,
        GridOptions options,
        CancellationToken cancellationToken = default);

    /// <summary>Gets templates for dropdown (just ID and name).</summary>
    Task<IEnumerable<EmailTemplateForDDLDto>> GetEmailTemplatesForDDLAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Previews email with placeholder replacement.
    /// Used to show agent what the final email will look like.
    /// </summary>
    /// <param name="templateId">Which template to preview.</param>
    /// <param name="placeholders">Sample placeholder values.</param>
    Task<EmailPreviewDto> PreviewEmailAsync(
        int templateId,
        Dictionary<string, string> placeholders,
        CancellationToken cancellationToken = default);
}

// ============================================================================
// FILE 3: Domain.Contracts/Services/IEmailService.cs
// ============================================================================

namespace Domain.Contracts.Services.Email;

/// <summary>
/// Service for sending emails via templates.
/// Handles placeholder replacement, SMTP delivery, and logging.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends an email using a template.
    /// Replaces placeholders and sends via SMTP (SendGrid, AWS SES, etc).
    /// </summary>
    /// <param name="templateId">Which template to use.</param>
    /// <param name="recipientEmail">Recipient's email address.</param>
    /// <param name="recipientName">Recipient's name (for personalization).</param>
    /// <param name="placeholders">Values to replace in template.</param>
    /// <returns>True if queued successfully (not guaranteed delivery).</returns>
    Task<bool> SendEmailAsync(
        int templateId,
        string recipientEmail,
        string recipientName,
        Dictionary<string, string> placeholders,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends email to multiple recipients in bulk.
    /// More efficient than calling SendEmailAsync in a loop.
    /// </summary>
    /// <param name="templateId">Which template to use.</param>
    /// <param name="recipients">List of email + name + placeholders for each.</param>
    /// <returns>List of results (success/failure for each).</returns>
    Task<IEnumerable<BulkEmailResultDto>> SendBulkEmailAsync(
        int templateId,
        IEnumerable<BulkEmailRecipientDto> recipients,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves email send logs filtered by status/date.
    /// </summary>
    Task<GridEntity<EmailLogDto>> GetEmailLogsAsync(
        GridOptions options,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retries failed emails (status = Failed).
    /// </summary>
    Task<int> RetryFailedEmailsAsync(
        int? templateId = null,
        DateTime? afterDate = null,
        CancellationToken cancellationToken = default);
}

// ============================================================================
// FILE 4: Domain.Contracts/Services/IVisaService.cs
// ============================================================================

namespace Domain.Contracts.Services.Visa;

/// <summary>
/// Service for managing visa applications and required documents.
/// Tracks: Application status, document requirements by country, verification.
/// </summary>
public interface IVisaService
{
    /// <summary>
    /// Creates a new visa application for an applicant.
    /// Also auto-generates required documents checklist based on country.
    /// </summary>
    /// <param name="visaAppForCreate">Applicant, country, visa type.</param>
    /// <returns>Created visa application with auto-generated document list.</returns>
    Task<VisaApplicationDto> CreateVisaApplicationAsync(
        VisaApplicationDto visaAppForCreate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the required documents for a specific country/visa type.
    /// Example: Australia student visa requires 6 documents.
    /// </summary>
    /// <param name="countryId">Destination country.</param>
    /// <param name="visaType">Student, Work, PR, etc.</param>
    /// <returns>List of required documents with upload status.</returns>
    Task<IEnumerable<VisaDocumentRequirementDto>> GetRequiredDocumentsAsync(
        int countryId,
        string visaType,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Uploads a visa document for an application.
    /// Stores file in cloud storage (S3/Azure Blob), saves URL to database.
    /// </summary>
    /// <param name="visaAppId">Which visa application.</param>
    /// <param name="documentType">Passport, IELTS, Transcript, etc.</param>
    /// <param name="file">The document file (PDF, image, etc).</param>
    Task<VisaDocumentDto> UploadVisaDocumentAsync(
        int visaAppId,
        string documentType,
        IFormFile file,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifies a submitted document (marks as Verified or Rejected).
    /// </summary>
    /// <param name="visaDocId">Which document to verify.</param>
    /// <param name="isVerified">True = approved, False = rejected.</param>
    /// <param name="rejectionReason">If rejected, why.</param>
    Task<int> VerifyVisaDocumentAsync(
        int visaDocId,
        bool isVerified,
        string? rejectionReason = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets current status and progress of a visa application.
    /// Shows: current stage, completed documents, pending documents.
    /// </summary>
    Task<VisaApplicationStatusDto> GetVisaApplicationStatusAsync(
        int visaAppId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates visa application status (Draft → Ready → Applied → Processing → Approved/Rejected).
    /// </summary>
    Task<VisaApplicationDto> UpdateVisaApplicationStatusAsync(
        int visaAppId,
        string newStatus,
        string? notes = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all visa applications for an applicant.
    /// </summary>
    Task<IEnumerable<VisaApplicationDto>> GetVisaApplicationsByApplicantAsync(
        int applicantId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Grid view of all visa applications (for admin dashboard).
    /// </summary>
    Task<GridEntity<VisaApplicationDto>> GetVisaApplicationsSummaryAsync(
        GridOptions options,
        CancellationToken cancellationToken = default);
}

// ============================================================================
// FILE 5: Domain.Contracts/Services/ISmsService.cs & IWhatsAppService.cs
// ============================================================================

namespace Domain.Contracts.Services.Messaging;

/// <summary>
/// Service for sending SMS messages via Twilio.
/// </summary>
public interface ISmsService
{
    /// <summary>
    /// Sends an SMS to a phone number.
    /// Cost: ~$0.0075 per message.
    /// </summary>
    /// <param name="phoneNumber">Recipient in E.164 format: +8801712345678.</param>
    /// <param name="message">SMS content (max 160 chars, or 67 chars if special chars).</param>
    /// <returns>True if queued successfully.</returns>
    Task<bool> SendSmsAsync(
        string phoneNumber,
        string message,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends SMS to multiple recipients.
    /// </summary>
    Task<IEnumerable<SmsResultDto>> SendBulkSmsAsync(
        IEnumerable<string> phoneNumbers,
        string message,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Service for sending WhatsApp messages via Twilio.
/// </summary>
public interface IWhatsAppService
{
    /// <summary>
    /// Sends a WhatsApp message.
    /// Cost: ~$0.013 per message, but higher engagement than SMS.
    /// Recipient must have WhatsApp and opted in.
    /// </summary>
    /// <param name="phoneNumber">Recipient in E.164 format: +8801712345678.</param>
    /// <param name="message">Message content (supports longer text and media).</param>
    /// <returns>True if queued successfully.</returns>
    Task<bool> SendWhatsAppAsync(
        string phoneNumber,
        string message,
        CancellationToken cancellationToken = default);
}

// ============================================================================
// FILE 6: Domain.Contracts/Services/INotificationService.cs
// ============================================================================

namespace Domain.Contracts.Services.Messaging;

/// <summary>
/// High-level service that decides which channel to use for notifications.
/// Intelligent fallback: Email → SMS → WhatsApp based on preference and success.
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Sends notification via preferred channel with fallback.
    /// </summary>
    /// <param name="recipientId">User/Applicant ID.</param>
    /// <param name="messageType">Email template code or SMS template.</param>
    /// <param name="placeholders">Dynamic content for message.</param>
    /// <param name="preferredChannels">Preferred order: ["WhatsApp", "Email", "SMS"].</param>
    Task<bool> SendNotificationAsync(
        int recipientId,
        string messageType,
        Dictionary<string, string> placeholders,
        List<string>? preferredChannels = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends bulk notifications (e.g., "All applicants: Your visa was approved!").
    /// </summary>
    Task<NotificationBatchResultDto> SendBulkNotificationAsync(
        IEnumerable<int> recipientIds,
        string messageType,
        Dictionary<string, string> placeholders,
        CancellationToken cancellationToken = default);
}

// ============================================================================
// DTO CLASSES (Data Transfer Objects)
// Place these in: bdDevs.Shared/Records/[Module]/
// ============================================================================

namespace bdDevs.Shared.DataTransferObjects.Analytics;

public record ConversionFunnelDto(
    int TotalLeads,
    int LeadsInCounseling,
    int ApplicationsSubmitted,
    int EnrollmentCompleted,
    decimal ConversionRate,
    decimal EnrollmentRate);

public record DashboardMetricsDto(
    int TotalLeads,
    int LeadsInCounseling,
    int ApplicationsSubmitted,
    int EnrollmentCompleted,
    decimal ConversionRate,
    decimal EnrollmentRate,
    decimal TotalRevenue,
    decimal OutstandingPayments);

public record AttributionDto(
    string Country_Or_Course,
    int LeadCount,
    int EnrollmentCount,
    decimal Revenue,
    decimal ConversionRate);

public record AgentPerformanceDto(
    int AgentId,
    string AgentName,
    int LeadsHandled,
    int EnrollmentsCompleted,
    decimal CommissionEarned,
    decimal ConversionRate);

public record MonthlyRevenueDto(
    int Month,
    string MonthName,
    decimal Revenue,
    int EnrollmentCount);

public record LeadSourceDto(
    string Source, // Web, Referral, Social, Event
    int Count,
    decimal Percentage);

namespace bdDevs.Shared.DataTransferObjects.Email;

public record EmailTemplateDto(
    int? EmailTemplateId,
    string TemplateName,
    string TemplateCode,
    string Subject,
    string BodyHtml,
    string PlaceholdersMeta,
    string Category,
    bool IsActive);

public record EmailTemplateForDDLDto(
    int EmailTemplateId,
    string TemplateName,
    string TemplateCode);

public record EmailLogDto(
    int EmailLogId,
    string TemplateName,
    string RecipientEmail,
    string Status,
    DateTime SentAt);

public record EmailPreviewDto(
    string PreviewSubject,
    string PreviewBodyHtml,
    string[] MissingPlaceholders);

public record BulkEmailResultDto(
    string RecipientEmail,
    bool Success,
    string? ErrorMessage);

public record BulkEmailRecipientDto(
    string Email,
    string Name,
    Dictionary<string, string> Placeholders);

namespace bdDevs.Shared.DataTransferObjects.Visa;

public record VisaApplicationDto(
    int? VisaApplicationId,
    int ApplicantId,
    int CountryId,
    string VisaType,
    string Status,
    DateTime ApplicationDate,
    DateTime? EstimatedDecisionDate,
    DateTime? ApprovalDate,
    string? ReferenceNumber);

public record VisaDocumentDto(
    int? VisaDocumentId,
    int VisaApplicationId,
    string DocumentType,
    string Status,
    DateTime? SubmittedDate,
    DateTime? VerificationDate,
    string? FileUrl);

public record VisaDocumentRequirementDto(
    string DocumentType,
    bool IsRequired,
    string Description);

public record VisaApplicationStatusDto(
    int VisaApplicationId,
    string CurrentStatus,
    int DocumentsCompleted,
    int DocumentsRequired,
    decimal ProgressPercentage,
    IEnumerable<VisaDocumentDto> Documents);

namespace bdDevs.Shared.DataTransferObjects.Messaging;

public record SmsResultDto(
    string PhoneNumber,
    bool Success,
    string? ExternalMessageId,
    string? ErrorMessage);

public record NotificationBatchResultDto(
    int TotalRequests,
    int SuccessCount,
    int FailureCount,
    List<string> FailedRecipients);
