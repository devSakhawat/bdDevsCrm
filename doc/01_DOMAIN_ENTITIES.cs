// ============================================================================
// DOMAIN LAYER - ENTITIES
// Place these files in: Domain.Entities/Entities/[Module]/
// ============================================================================

// ============================================================================
// FILE 1: Domain.Entities/Analytics/AnalyticsSnapshot.cs
// ============================================================================
namespace Domain.Entities.Entities.Analytics;

/// <summary>
/// Daily snapshot of analytics metrics. Created nightly (2 AM) for performance.
/// Instead of running expensive aggregations on-demand, we cache daily snapshots.
/// This reduces database load and provides instant dashboard response.
/// </summary>
public class AnalyticsSnapshot
{
    /// <summary>Primary key.</summary>
    public int AnalyticsSnapshotId { get; set; }

    /// <summary>The date this snapshot represents (e.g., 2025-04-28).</summary>
    public DateTime SnapshotDate { get; set; }

    #region Funnel Metrics - Lead to Enrollment Pipeline
    /// <summary>Total number of leads created on this date.</summary>
    public int TotalLeads { get; set; }

    /// <summary>Leads that moved to counseling stage (showed interest).</summary>
    public int LeadsInCounseling { get; set; }

    /// <summary>Applications submitted on this date.</summary>
    public int ApplicationsSubmitted { get; set; }

    /// <summary>Students enrolled on this date.</summary>
    public int EnrollmentCompleted { get; set; }
    #endregion

    #region Revenue Metrics
    /// <summary>Total commission earned on this date (in base currency).</summary>
    public decimal TotalRevenue { get; set; }

    /// <summary>Commission earned but not yet paid out to agents.</summary>
    public decimal OutstandingPayments { get; set; }

    /// <summary>Commission already paid to agents/sub-agents.</summary>
    public decimal PaidCommission { get; set; }
    #endregion

    #region Calculated Properties (Read-Only)
    /// <summary>
    /// Percentage of leads that became applications.
    /// Example: 100 leads, 30 apps = 30% conversion rate.
    /// </summary>
    public decimal ConversionRate
    {
        get => TotalLeads > 0
            ? ((decimal)ApplicationsSubmitted / TotalLeads) * 100
            : 0;
    }

    /// <summary>
    /// Percentage of applications that became enrollments.
    /// Example: 30 apps, 20 enrolled = 66.67% enrollment rate.
    /// </summary>
    public decimal EnrollmentRate
    {
        get => ApplicationsSubmitted > 0
            ? ((decimal)EnrollmentCompleted / ApplicationsSubmitted) * 100
            : 0;
    }

    /// <summary>
    /// End-to-end conversion: leads → enrollments.
    /// Example: 100 leads, 20 enrolled = 20% total conversion.
    /// </summary>
    public decimal TotalConversionRate
    {
        get => TotalLeads > 0
            ? ((decimal)EnrollmentCompleted / TotalLeads) * 100
            : 0;
    }

    /// <summary>
    /// Collection rate: how much of total revenue was collected vs outstanding.
    /// Example: Paid $8000, Outstanding $2000 = 80% collection rate.
    /// </summary>
    public decimal CollectionRate
    {
        get
        {
            var total = PaidCommission + OutstandingPayments;
            return total > 0 ? (PaidCommission / total) * 100 : 0;
        }
    }
    #endregion

    #region Audit Fields
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    #endregion
}

// ============================================================================
// FILE 2: Domain.Entities/Analytics/ConversionFunnel.cs
// ============================================================================

namespace Domain.Entities.Entities.Analytics;

/// <summary>
/// Tracks individual lead progression through the funnel.
/// When a lead moves from Inquiry → Counseling → Application → Enrollment,
/// we log each transition here for detailed attribution analysis.
/// </summary>
public class ConversionFunnel
{
    /// <summary>Primary key.</summary>
    public int FunnelId { get; set; }

    /// <summary>Foreign key to the lead being tracked.</summary>
    public int LeadId { get; set; }

    /// <summary>
    /// Current stage: Inquiry, Counseling, Application, Enrollment, or Rejected.
    /// This tracks where the lead is in the pipeline.
    /// </summary>
    public string Stage { get; set; } = "Inquiry"; // Inquiry, Counseling, Application, Enrollment, Rejected

    /// <summary>When the lead entered this stage.</summary>
    public DateTime EnteredAt { get; set; } = DateTime.UtcNow;

    /// <summary>When the lead exited this stage (moved to next stage or rejected).</summary>
    public DateTime? ExitedAt { get; set; }

    #region Attribution Tracking (For Analysis)
    /// <summary>Which country did the lead apply to? Helps answer: "Which countries convert best?"</summary>
    public int? CountryId { get; set; }

    /// <summary>Which course/program? Helps answer: "Which courses convert best?"</summary>
    public int? CourseId { get; set; }

    /// <summary>Which intake month? Helps answer: "Which months are busiest?"</summary>
    public int? IntakeMonthId { get; set; }

    /// <summary>Which intake year? Helps answer: "How are current cohorts progressing?"</summary>
    public int? IntakeYearId { get; set; }

    /// <summary>Which agent/counselor? Helps answer: "Who converts best?"</summary>
    public int? AgentId { get; set; }
    #endregion

    #region Audit
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    #endregion

    #region Navigation Properties (For EF Core)
    // These are optional but helpful for eager loading
    // public virtual CrmLead? Lead { get; set; }
    // public virtual CrmCountry? Country { get; set; }
    // public virtual CrmCourse? Course { get; set; }
    // public virtual Users? Agent { get; set; }
    #endregion
}

// ============================================================================
// FILE 3: Domain.Entities/Email/EmailTemplate.cs
// ============================================================================

namespace Domain.Entities.Entities.Email;

/// <summary>
/// Pre-built email templates for consistent, branded communication.
/// Supports mail-merge with placeholders like {StudentName}, {UniversityName}, etc.
/// </summary>
public class EmailTemplate
{
    /// <summary>Primary key.</summary>
    public int EmailTemplateId { get; set; }

    /// <summary>Human-readable name: "Admission Offer", "Document Request", etc.</summary>
    public string TemplateName { get; set; } = string.Empty;

    /// <summary>System identifier: "ADMISSION_OFFER", "DOCUMENT_REQUEST" (no spaces, uppercase).</summary>
    public string TemplateCode { get; set; } = string.Empty;

    /// <summary>Email subject line with optional placeholders: "Congratulations {StudentName}!".</summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>Email body in HTML format. Supports placeholders and basic HTML formatting.</summary>
    public string BodyHtml { get; set; } = string.Empty;

    /// <summary>
    /// JSON array of available placeholders for UI hints.
    /// Example: '["StudentName","UniversityName","OfferExpiry","CounselorName"]'
    /// </summary>
    public string PlaceholdersMeta { get; set; } = "[]";

    /// <summary>Category/Group: "Admission", "Payment", "Visa", etc.</summary>
    public string Category { get; set; } = "General";

    /// <summary>Is this template available for use?</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Optional: Which organization/company owns this template?</summary>
    public int? CompanyId { get; set; }

    #region Audit
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int CreatedByUserId { get; set; }
    #endregion
}

// ============================================================================
// FILE 4: Domain.Entities/Email/EmailLog.cs
// ============================================================================

namespace Domain.Entities.Entities.Email;

/// <summary>
/// Log of every email sent. Used for:
/// - Tracking delivery status
/// - Proving communication occurred
/// - Retrying failed sends
/// - Compliance/auditing
/// </summary>
public class EmailLog
{
    /// <summary>Primary key.</summary>
    public int EmailLogId { get; set; }

    /// <summary>Foreign key to the template used.</summary>
    public int EmailTemplateId { get; set; }

    /// <summary>Recipient's email address.</summary>
    public string RecipientEmail { get; set; } = string.Empty;

    /// <summary>Recipient's name for personalization.</summary>
    public string RecipientName { get; set; } = string.Empty;

    /// <summary>Actual subject line sent (after placeholder replacement).</summary>
    public string SentSubject { get; set; } = string.Empty;

    /// <summary>Email delivery status: Queued, Sent, Delivered, Bounced, Failed.</summary>
    public string Status { get; set; } = "Queued"; // Queued, Sent, Delivered, Bounced, Failed

    /// <summary>When was the email sent?</summary>
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    /// <summary>When did the email get delivered? (if available from provider).</summary>
    public DateTime? DeliveredAt { get; set; }

    /// <summary>If send failed, what was the reason?</summary>
    public string? FailureReason { get; set; }

    /// <summary>External provider's message ID (e.g., SendGrid message ID).</summary>
    public string? ExternalMessageId { get; set; }

    /// <summary>Number of retry attempts (if failed).</summary>
    public int RetryCount { get; set; } = 0;

    #region Audit
    public int SentByUserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    #endregion
}

// ============================================================================
// FILE 5: Domain.Entities/Visa/VisaApplication.cs
// ============================================================================

namespace Domain.Entities.Entities.Visa;

/// <summary>
/// Tracks visa application for each applicant.
/// Different countries have different requirements and processing times.
/// </summary>
public class VisaApplication
{
    /// <summary>Primary key.</summary>
    public int VisaApplicationId { get; set; }

    /// <summary>Foreign key to the applicant.</summary>
    public int ApplicantId { get; set; }

    /// <summary>Foreign key to destination country.</summary>
    public int CountryId { get; set; }

    /// <summary>
    /// Type of visa: Student, Work, PR (Permanent Resident), Tourist.
    /// Different countries support different types.
    /// </summary>
    public string VisaType { get; set; } = "Student"; // Student, Work, PR, Tourist

    /// <summary>
    /// Current status: Draft, Ready, Applied, Processing, Approved, Rejected, Expired.
    /// Defines what documents are needed and next steps.
    /// </summary>
    public string Status { get; set; } = "Draft"; // Draft, Ready, Applied, Processing, Approved, Rejected, Expired

    /// <summary>When did the applicant start this application?</summary>
    public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;

    /// <summary>When should we expect a decision? (varies by country: 2-12 weeks).</summary>
    public DateTime? EstimatedDecisionDate { get; set; }

    /// <summary>When was the visa approved? (only filled if Status = Approved).</summary>
    public DateTime? ApprovalDate { get; set; }

    /// <summary>If rejected, reason for rejection.</summary>
    public string? RejectionReason { get; set; }

    /// <summary>Reference/application number from the immigration authority.</summary>
    public string? ReferenceNumber { get; set; }

    /// <summary>Notes/comments about the application.</summary>
    public string? Notes { get; set; }

    #region Audit
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int CreatedByUserId { get; set; }
    #endregion

    #region Navigation
    // public virtual Applicant? Applicant { get; set; }
    // public virtual CrmCountry? Country { get; set; }
    // public virtual ICollection<VisaDocument> Documents { get; set; } = new List<VisaDocument>();
    #endregion
}

// ============================================================================
// FILE 6: Domain.Entities/Visa/VisaDocument.cs
// ============================================================================

namespace Domain.Entities.Entities.Visa;

/// <summary>
/// Required documents for visa application.
/// Different countries need different documents (Australia needs 6, Canada needs 5, etc).
/// Tracks upload status, verification status, and when documents were submitted.
/// </summary>
public class VisaDocument
{
    /// <summary>Primary key.</summary>
    public int VisaDocumentId { get; set; }

    /// <summary>Foreign key to the visa application.</summary>
    public int VisaApplicationId { get; set; }

    /// <summary>What type of document: Passport, IELTS, Transcript, Bank Statement, etc.</summary>
    public string DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// Document status: NotRequired, Required, Pending, Submitted, Verified, Rejected.
    /// Shows where the document is in the process.
    /// </summary>
    public string Status { get; set; } = "Required"; // NotRequired, Required, Pending, Submitted, Verified, Rejected

    /// <summary>Is this document mandatory for this country/visa type?</summary>
    public bool IsRequired { get; set; } = true;

    /// <summary>When did the applicant upload this document?</summary>
    public DateTime? SubmittedDate { get; set; }

    /// <summary>When did the agent verify/approve this document?</summary>
    public DateTime? VerificationDate { get; set; }

    /// <summary>URL/path to the uploaded file (stored in cloud storage like S3/Azure Blob).</summary>
    public string? FileUrl { get; set; }

    /// <summary>File name as uploaded.</summary>
    public string? FileName { get; set; }

    /// <summary>If verification failed, reason.</summary>
    public string? RejectionReason { get; set; }

    /// <summary>Notes about this document.</summary>
    public string? Notes { get; set; }

    #region Audit
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int? VerifiedByUserId { get; set; }
    #endregion

    #region Navigation
    // public virtual VisaApplication? VisaApplication { get; set; }
    #endregion
}

// ============================================================================
// FILE 7: Domain.Entities/Messaging/MessageLog.cs
// ============================================================================

namespace Domain.Entities.Entities.Messaging;

/// <summary>
/// Log of SMS and WhatsApp messages sent.
/// Used for compliance, delivery tracking, and cost monitoring.
/// </summary>
public class MessageLog
{
    /// <summary>Primary key.</summary>
    public int MessageLogId { get; set; }

    /// <summary>Recipient's phone number (E.164 format: +8801712345678).</summary>
    public string RecipientPhone { get; set; } = string.Empty;

    /// <summary>
    /// Communication channel: SMS or WhatsApp.
    /// SMS: Reliable, cheaper, limited text.
    /// WhatsApp: More personal, can send media, higher engagement.
    /// </summary>
    public string Channel { get; set; } = "SMS"; // SMS, WhatsApp

    /// <summary>The message content (text only).</summary>
    public string MessageContent { get; set; } = string.Empty;

    /// <summary>
    /// Delivery status: Queued, Sent, Delivered, Read, Failed, Bounced.
    /// Updated as webhooks come from Twilio.
    /// </summary>
    public string Status { get; set; } = "Queued"; // Queued, Sent, Delivered, Read, Failed, Bounced

    /// <summary>When was the message sent?</summary>
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    /// <summary>When was the message delivered? (from provider).</summary>
    public DateTime? DeliveredAt { get; set; }

    /// <summary>External provider's message ID (Twilio SID).</summary>
    public string? ExternalMessageId { get; set; }

    /// <summary>Cost of the message (SMS: $0.0075, WhatsApp: $0.013).</summary>
    public decimal? Cost { get; set; }

    /// <summary>If send failed, the reason.</summary>
    public string? FailureReason { get; set; }

    /// <summary>How many times was this message retried?</summary>
    public int RetryCount { get; set; } = 0;

    /// <summary>Can this message be retried if it fails?</summary>
    public bool IsRetryable { get; set; } = true;

    #region Audit
    public int SentByUserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    #endregion
}
