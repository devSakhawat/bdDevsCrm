namespace bdDevs.Shared.DataTransferObjects.Layout;

public sealed class HeaderSummaryDto
{
  public HeaderUserProfileDto User { get; set; } = new();
  public List<HeaderQuickLinkDto> QuickLinks { get; set; } = new();
  public HeaderWidgetDto Notifications { get; set; } = new();
  public HeaderWidgetDto PendingApprovals { get; set; } = new();
  public HeaderWidgetDto Messages { get; set; } = new();
  public HeaderWidgetDto Birthdays { get; set; } = new();
  public DateTime ServerTimeUtc { get; set; } = DateTime.UtcNow;
}

public sealed class HeaderUserProfileDto
{
  public string DisplayName { get; set; } = string.Empty;
  public string? Designation { get; set; }
  public string? CompanyName { get; set; }
  public string? DepartmentName { get; set; }
  public string? BranchName { get; set; }
  public string? AvatarUrl { get; set; }
  public string Initials { get; set; } = "U";
}

public sealed class HeaderQuickLinkDto
{
  public string Title { get; set; } = string.Empty;
  public string Url { get; set; } = string.Empty;
  public string? Description { get; set; }
}

public sealed class HeaderWidgetDto
{
  public string Title { get; set; } = string.Empty;
  public int Count { get; set; }
  public string EmptyMessage { get; set; } = "No items available.";
  public string? ActionUrl { get; set; }
  public List<HeaderListItemDto> Items { get; set; } = new();
}

public sealed class HeaderListItemDto
{
  public string Title { get; set; } = string.Empty;
  public string? Subtitle { get; set; }
  public string? Url { get; set; }
  public string? AvatarUrl { get; set; }
  public string? BadgeText { get; set; }
}
