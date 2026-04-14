using Application.Shared.DataTransferObjects.Common;

namespace Application.Shared.DataTransferObjects.Core.SystemAdmin;


public class MenuDto : CommonDto
{
  public int? MenuId { get; set; }
  public string? MenuName { get; set; }
  public int? ModuleId { get; set; }
  public string? ModuleName { get; set; }
  public string? MenuPath { get; set; }
  public int? ParentMenu { get; set; }
  public string? ParentMenuName { get; set; }
  public int? TotalCount { get; set; }
  public int? ToDo { get; set; }
  public int? SortOrder { get; set; }
  public int IsQuickLink { get; set; }
  public int? MenuType { get; set; }
  public string? MenuCode { get; set; }
  public int? IsActive { get; set; }
}

//IsActive:1
//IsQuickLink: false
//MenuCode: ""
//MenuId: 0
//MenuName: "Crm Admin"
//MenuPath: ""
//MenuType: 0
//ModuleId: 31
//ModuleId_input: "CRM"
//ParentMenu: 611
//ParentMenu_input: "CRM"
//SortOrder: 0