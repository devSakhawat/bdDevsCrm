using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.Layout;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services.Core.Infrastructure;
using Domain.Entities.Entities.System;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.Core.Infrastructure;

internal sealed class HeaderService : IHeaderService
{
  private const int MaxQuickLinks = 6;
  private const int MaxPanelItems = 5;

  private readonly IRepositoryManager _repository;
  private readonly ILogger<HeaderService> _logger;
  private readonly IConfiguration _configuration;

  public HeaderService(IRepositoryManager repository, ILogger<HeaderService> logger, IConfiguration configuration)
  {
    _repository = repository;
    _logger = logger;
    _configuration = configuration;
  }

  public async Task<HeaderSummaryDto> GetHeaderSummaryAsync(UsersDto currentUser, CancellationToken cancellationToken = default)
  {
    if (currentUser.UserId is null || currentUser.UserId <= 0)
      throw new GenericUnauthorizedException("User authentication required. Please log in again.");

    var userId = currentUser.UserId.Value;
    var hrRecordId = currentUser.HrRecordId ?? currentUser.EmployeeId ?? 0;
    var accessibleMenusTask = _repository.Menus.MenusByUserPermissionAsync(userId, trackChanges: false, cancellationToken);
    var branchNameTask = GetBranchNameAsync(currentUser.BranchId, cancellationToken);
    var birthdayItemsTask = GetBirthdayItemsAsync(cancellationToken);
    var pendingItemsTask = GetPendingApprovalItemsAsync(hrRecordId, cancellationToken);

    await Task.WhenAll(accessibleMenusTask, branchNameTask, birthdayItemsTask, pendingItemsTask);

    var accessibleMenus = accessibleMenusTask.Result
      .Where(menu => (menu.IsActive ?? 1) != 0)
      .ToList();

    var quickLinks = BuildQuickLinks(accessibleMenus);
    var pendingApprovals = BuildPendingApprovalsWidget(pendingItemsTask.Result, accessibleMenus);
    var birthdays = BuildBirthdayWidget(birthdayItemsTask.Result, accessibleMenus);
    var messages = BuildMessagesWidget();
    var notifications = BuildNotificationsWidget(pendingApprovals, messages, birthdays);

    _logger.LogInformation("Header summary prepared for userId {UserId}", userId);

    return new HeaderSummaryDto
    {
      User = BuildUserProfile(currentUser, branchNameTask.Result),
      QuickLinks = quickLinks,
      PendingApprovals = pendingApprovals,
      Birthdays = birthdays,
      Messages = messages,
      Notifications = notifications,
      ServerTimeUtc = DateTime.UtcNow
    };
  }

  private async Task<string?> GetBranchNameAsync(int? branchId, CancellationToken cancellationToken)
  {
    if (branchId is null || branchId <= 0)
      return null;

    var branch = await _repository.Branches.ByIdAsync(x => x.Branchid == branchId.Value, trackChanges: false, cancellationToken);
    return string.IsNullOrWhiteSpace(branch?.Branchname) ? null : branch.Branchname.Trim();
  }

  private async Task<List<Employee>> GetBirthdayItemsAsync(CancellationToken cancellationToken)
  {
    var today = DateTime.Today;

    var employees = await _repository.Employees.ListByConditionAsync(
      x => x.DateofBirth.HasValue
        && x.DateofBirth.Value.Month == today.Month
        && x.DateofBirth.Value.Day == today.Day,
      orderBy: x => x.FullName!,
      trackChanges: false,
      cancellationToken: cancellationToken);

    return employees
      .Where(x => !string.IsNullOrWhiteSpace(x.FullName))
      .ToList();
  }

  private async Task<List<ApproverHistory>> GetPendingApprovalItemsAsync(int hrRecordId, CancellationToken cancellationToken)
  {
    if (hrRecordId <= 0)
      return new List<ApproverHistory>();

    var approvals = await _repository.ApproverHistories.ListByConditionAsync(
      x => x.ApproverId == hrRecordId && x.IsActive == true,
      orderBy: x => x.AssignApproverId,
      trackChanges: false,
      descending: true,
      cancellationToken: cancellationToken);

    return approvals.ToList();
  }

  private List<HeaderQuickLinkDto> BuildQuickLinks(IEnumerable<Menu> accessibleMenus)
  {
    return accessibleMenus
      .Where(menu => IsSafePath(menu.MenuPath))
      .GroupBy(menu => menu.MenuId)
      .Select(group => group.First())
      .OrderBy(menu => menu.Sororder ?? int.MaxValue)
      .ThenBy(menu => menu.MenuName)
      .Take(MaxQuickLinks)
      .Select(menu => new HeaderQuickLinkDto
      {
        Title = string.IsNullOrWhiteSpace(menu.MenuName) ? "Shortcut" : menu.MenuName.Trim(),
        Url = NormalizePath(menu.MenuPath)!,
        Description = (menu.Todo ?? 0) > 0 ? $"{menu.Todo} pending" : "Quick access"
      })
      .ToList();
  }

  private HeaderUserProfileDto BuildUserProfile(UsersDto currentUser, string? branchName)
  {
    var displayName = FirstNonEmpty(currentUser.EmployeeName, currentUser.UserName, currentUser.LoginId, "User");
    var departmentAndBranch = new[] { currentUser.DepartmentName, branchName }
      .Where(static value => !string.IsNullOrWhiteSpace(value))
      .Select(static value => value!.Trim())
      .ToArray();

    return new HeaderUserProfileDto
    {
      DisplayName = displayName,
      Designation = NullIfWhiteSpace(currentUser.DESIGNATIONNAME),
      CompanyName = NullIfWhiteSpace(currentUser.CompanyName),
      DepartmentName = NullIfWhiteSpace(currentUser.DepartmentName),
      BranchName = branchName,
      AvatarUrl = NormalizeAssetPath(currentUser.ProfilePicture),
      Initials = BuildInitials(displayName)
    };
  }

  private HeaderWidgetDto BuildPendingApprovalsWidget(List<ApproverHistory> approvals, IReadOnlyCollection<Menu> accessibleMenus)
  {
    var defaultUrl = ResolveFirstMenuPath(accessibleMenus, "/Home/Index", "approval", "workflow", "approver");

    return new HeaderWidgetDto
    {
      Title = "Pending approvals",
      Count = approvals.Count,
      ActionUrl = defaultUrl,
      EmptyMessage = "No pending approvals right now.",
      Items = approvals
        .Take(MaxPanelItems)
        .Select(approval => new HeaderListItemDto
        {
          Title = $"Approval #{approval.AssignApproverId}",
          Subtitle = $"Module {approval.ModuleId} • HR {approval.HrRecordId}",
          Url = ResolveModuleUrl(approval.ModuleId, accessibleMenus, defaultUrl),
          BadgeText = "Pending"
        })
        .ToList()
    };
  }

  private HeaderWidgetDto BuildBirthdayWidget(List<Employee> employees, IReadOnlyCollection<Menu> accessibleMenus)
  {
    var defaultUrl = ResolveFirstMenuPath(accessibleMenus, "/Home/Index", "employee", "hr");

    return new HeaderWidgetDto
    {
      Title = "Today's birthdays",
      Count = employees.Count,
      ActionUrl = defaultUrl,
      EmptyMessage = "No employee birthdays today.",
      Items = employees
        .Take(MaxPanelItems)
        .Select(employee => new HeaderListItemDto
        {
          Title = employee.FullName?.Trim() ?? "Employee",
          Subtitle = "Birthday today",
          Url = defaultUrl,
          AvatarUrl = NormalizeAssetPath(employee.Profilepicture),
          BadgeText = "🎉"
        })
        .ToList()
    };
  }

  private static HeaderWidgetDto BuildMessagesWidget()
  {
    return new HeaderWidgetDto
    {
      Title = "Unread messages",
      Count = 0,
      EmptyMessage = "Unread messaging is not configured yet."
    };
  }

  private static HeaderWidgetDto BuildNotificationsWidget(
    HeaderWidgetDto pendingApprovals,
    HeaderWidgetDto messages,
    HeaderWidgetDto birthdays)
  {
    var items = new List<HeaderListItemDto>
    {
      new()
      {
        Title = pendingApprovals.Title,
        Subtitle = pendingApprovals.Count > 0 ? $"{pendingApprovals.Count} item(s) require action" : "Nothing is waiting for approval.",
        Url = pendingApprovals.ActionUrl,
        BadgeText = pendingApprovals.Count.ToString()
      },
      new()
      {
        Title = messages.Title,
        Subtitle = messages.Count > 0 ? $"{messages.Count} unread message(s)" : messages.EmptyMessage,
        Url = messages.ActionUrl,
        BadgeText = messages.Count.ToString()
      },
      new()
      {
        Title = birthdays.Title,
        Subtitle = birthdays.Count > 0 ? $"{birthdays.Count} birthday celebration(s) today" : birthdays.EmptyMessage,
        Url = birthdays.ActionUrl,
        BadgeText = birthdays.Count.ToString()
      }
    };

    return new HeaderWidgetDto
    {
      Title = "Notifications",
      Count = pendingApprovals.Count + messages.Count + birthdays.Count,
      EmptyMessage = "No notifications right now.",
      ActionUrl = pendingApprovals.ActionUrl,
      Items = items
    };
  }

  private static string ResolveModuleUrl(int moduleId, IReadOnlyCollection<Menu> accessibleMenus, string fallbackUrl)
  {
    var modulePath = accessibleMenus
      .Where(menu => menu.ModuleId == moduleId && IsSafePath(menu.MenuPath))
      .OrderBy(menu => menu.Sororder ?? int.MaxValue)
      .Select(menu => NormalizePath(menu.MenuPath))
      .FirstOrDefault();

    return modulePath ?? fallbackUrl;
  }

  private static string ResolveFirstMenuPath(
    IReadOnlyCollection<Menu> accessibleMenus,
    string fallbackUrl,
    params string[] nameHints)
  {
    var match = accessibleMenus
      .Where(menu => IsSafePath(menu.MenuPath))
      .OrderBy(menu => menu.Sororder ?? int.MaxValue)
      .FirstOrDefault(menu =>
        !string.IsNullOrWhiteSpace(menu.MenuName)
        && nameHints.Any(hint => menu.MenuName.Contains(hint, StringComparison.OrdinalIgnoreCase)));

    return NormalizePath(match?.MenuPath) ?? fallbackUrl;
  }

  private static bool IsSafePath(string? path)
  {
    if (string.IsNullOrWhiteSpace(path))
      return false;

    var trimmed = path.Trim();
    return !trimmed.StartsWith("javascript:", StringComparison.OrdinalIgnoreCase);
  }

  private static string? NormalizePath(string? path)
  {
    if (!IsSafePath(path))
      return null;

    var value = path!.Trim().Replace('\\', '/');

    if (Uri.TryCreate(value, UriKind.Absolute, out var absoluteUri))
      return absoluteUri.Scheme is "http" or "https" ? absoluteUri.ToString() : null;

    while (value.StartsWith("."))
      value = value[1..];

    if (!value.StartsWith('/'))
      value = $"/{value}";

    return value.Replace("//", "/");
  }

  private static string? NormalizeAssetPath(string? path)
  {
    if (string.IsNullOrWhiteSpace(path))
      return null;

    var value = path.Trim().Replace('\\', '/');

    if (Uri.TryCreate(value, UriKind.Absolute, out var absoluteUri))
      return absoluteUri.Scheme is "http" or "https" ? absoluteUri.ToString() : null;

    value = value.TrimStart('.', '~');

    if (!value.StartsWith('/'))
      value = $"/{value}";

    return value.Replace("//", "/");
  }

  private static string BuildInitials(string value)
  {
    var parts = value
      .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
      .Take(2)
      .Select(part => char.ToUpperInvariant(part[0]));

    var initials = string.Concat(parts);
    return string.IsNullOrWhiteSpace(initials) ? "U" : initials;
  }

  private static string FirstNonEmpty(params string?[] values)
  {
    return values.FirstOrDefault(value => !string.IsNullOrWhiteSpace(value))?.Trim() ?? string.Empty;
  }

  private static string? NullIfWhiteSpace(string? value)
  {
    return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
  }
}
