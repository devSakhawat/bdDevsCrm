using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Constants;
using Microsoft.AspNetCore.Http;
using Presentation.LinkFactories;

namespace Presentation.LinkFactories.Core.SystemAdmin;

/// <summary>
/// HATEOAS link factory for Menu entity.
/// Uses RouteConstants — no magic strings.
/// </summary>
public class MenuLinkFactory : ILinkFactory<MenuDto>
{
  private readonly string _baseUrl;

  public MenuLinkFactory(IHttpContextAccessor httpContextAccessor)
  {
    var request = httpContextAccessor.HttpContext?.Request;
    _baseUrl = request != null
        ? $"{request.Scheme}://{request.Host}/bdDevs-crm"
        : string.Empty;
  }

  /// <summary>
  /// Row-level links: self, edit, delete
  /// </summary>
  public List<ResourceLink> GenerateRowLinks(int key) => new()
    {
        new ResourceLink
        {
            Rel         = "self",
            Method      = "GET",
            Href        = $"{_baseUrl}/{RouteConstants.ReadMenu.Replace("{key}", key.ToString())}",
            Description = " menu details"
        },
        new ResourceLink
        {
            Rel         = "edit",
            Method      = "PUT",
            Href        = $"{_baseUrl}/{RouteConstants.UpdateMenu.Replace("{key:int}", key.ToString())}",
            Description = "Update this menu"
        },
        new ResourceLink
        {
            Rel         = "delete",
            Method      = "DELETE",
            Href        = $"{_baseUrl}/{RouteConstants.DeleteMenu.Replace("{key:int}", key.ToString())}",
            Description = "Delete this menu"
        }
    };

  /// <summary>
  /// Resource-level navigation links
  /// </summary>
  public List<ResourceLink> GenerateResourceLinks() => new()
    {
        new ResourceLink
        {
            Rel         = "self",
            Method      = "POST",
            Href        = $"{_baseUrl}/{RouteConstants.MenuSummary}",
            Description = "Menu list with pagination"
        },
        new ResourceLink
        {
            Rel         = "create",
            Method      = "POST",
            Href        = $"{_baseUrl}/{RouteConstants.CreateMenu}",
            Description = "Create new menu"
        }
    };
}