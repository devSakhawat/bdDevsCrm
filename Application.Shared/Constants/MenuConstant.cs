using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Constants;

public static class MenuConstant
{
  // Key = MenuName, Value = MenuPath (UI route)
  public static readonly IReadOnlyDictionary<string, string> MenuNameToPath =
    new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
      ["Dashboard"] = "/dashboard",
      ["CRM"] = "/crm",
      ["CrmApplication"] = "/CRM/Application",
      ["Lead"] = "/crm/lead",
      ["Contact"] = "/crm/contact",
      ["Account"] = "/crm/account",
      ["Opportunity"] = "/crm/opportunity",
      ["Quote"] = "/sales/quote",
      ["Order"] = "/sales/order",
      ["Invoice"] = "/sales/invoice",
      ["Product"] = "/product",
      ["Campaign"] = "/marketing/campaign",
      ["Activity"] = "/activity",
      ["Report"] = "/report",
      ["Document"] = "/document",
      ["Ticket"] = "/ticket",
      ["User Management"] = "/system/user-management",
      ["System Administration"] = "/system/administration",
    };

  // Safe accessor by menu name
  public static bool TryPath(string menuName, out string? path)
  {
    if (string.IsNullOrWhiteSpace(menuName))
    {
      path = null;
      return false;
    }

    if (MenuNameToPath.TryGetValue(menuName, out var value))
    {
      path = value;
      return true;
    }

    path = null;
    return false;
  }


}

