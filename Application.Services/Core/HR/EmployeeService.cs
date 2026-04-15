using Domain.Contracts.Repositories;
﻿using Domain.Entities.Entities.System;

using Domain.Contracts.Services.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.Core.HR;
using Domain.Contracts.Services.Core.HR;
using bdDevs.Shared.DataTransferObjects.Core.HR;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Domain.Exceptions;
using Application.Services.Mappings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.ComponentModel.Design;

namespace Application.Services.Core.HR;


internal sealed class EmployeeService : IEmployeeService
{
  private readonly IRepositoryManager _repository;
  private readonly ILogger<EmployeeService> _logger;
  private readonly IConfiguration _configuration;

  public EmployeeService(IRepositoryManager repository, ILogger<EmployeeService> logger, IConfiguration configuration)
  {
    _repository = repository;
    _logger = logger;
    _configuration = configuration;
  }

  public async Task<EmploymentDto> EmploymentByHrRecordId(int hrRecordId ,CancellationToken cancellationToken = default)
  {
    Employment employment = await _repository.Employees.EmploymentByHrRecordId(hrRecordId, cancellationToken);
    //Check if the result is null
    if (employment == null) throw new NotFoundException("EmploymentDto", "HrRecordId", hrRecordId.ToString());

    EmploymentDto employmentDto = MyMapper.JsonClone<Employment, EmploymentDto>(employment);
    return employmentDto;
  }

  public async Task<WfStateDto> EmployeeCurrentStatusByHrRecordId(int hrRecordId, CancellationToken cancellationToken = default)
  {
    WfState wfstate = await _repository.Employees.EmployeeCurrentStatusByHrRecordId(hrRecordId, cancellationToken);
    //Check if the result is null
    if (wfstate == null) throw new NotFoundException("WfState", "Employee.StateId", hrRecordId.ToString());

    WfStateDto wfstateDto = MyMapper.JsonClone<WfState, WfStateDto>(wfstate);
    return wfstateDto;
  }

  public async Task<EmployeeDto> EmployeeByHrRecordId(int hrRecordId, CancellationToken cancellationToken = default)
  {
    Employee employee = await _repository.Employees.EmployeeByHrRecordId(hrRecordId, cancellationToken);
    //Check if the result is null
    if (employee == null) throw new NotFoundException("Employee", "Employee.HrrecordId", hrRecordId.ToString());

    EmployeeDto employeeDto = MyMapper.JsonClone<Employee, EmployeeDto>(employee);
    return employeeDto;
  }

  // get employee types with id, name and code
  public async Task<IEnumerable<EmployeeTypeDto>> EmployeeTypes(int param, CancellationToken cancellationToken = default)
  {
    // Initialize the employeeTypes collection properly
    IEnumerable<Employeetype> employeeTypes = Enumerable.Empty<Employeetype>();

    if (param == 0)
    {
      employeeTypes = await _repository.EmployeeTypes.ListByWhereWithSelectAsync(
        selector: x => new Employeetype
        {
          Employeetypeid = x.Employeetypeid,
          EmployeeTypeCode = x.EmployeeTypeCode,
          Employeetypename = x.Employeetypename
        }
        , x => x.IsActive == 0 && (x.IsNotAccess == null || x.IsNotAccess == false)
        , orderBy: x => x.Employeetypename
        , trackChanges: false
      );
    }
    else
    {
      employeeTypes = await _repository.EmployeeTypes.ListByWhereWithSelectAsync(
        selector: x => new Employeetype
        {
          Employeetypeid = x.Employeetypeid,
          EmployeeTypeCode = x.EmployeeTypeCode,
          Employeetypename = x.Employeetypename
        }
        , x => x.IsActive == 1, orderBy: x => x.Employeetypename, trackChanges: false, cancellationToken: cancellationToken);
    }

    // Check if the result is null or empty
    if (employeeTypes == null || !employeeTypes.Any())
      throw new NotFoundException("EmployeeType", "EmployeeTypeId", "0");

    IEnumerable<EmployeeTypeDto> result = MyMapper.JsonCloneIEnumerableToList<Employeetype, EmployeeTypeDto>(employeeTypes);
    return result;
  }


  // get employees with id, name and code
  public async Task<IEnumerable<EmployeesByCompanyBranchDepartmentDto>> EmployeeByCompanyIdAndBranchIdAndDepartmentId(int companyId, int branchId, int departmentId, CancellationToken cancellationToken = default)
  {
    string condition = "";
    if (companyId == 0)
    {
      condition = "";
    }
    else
    {
      condition = " where CompanyId = " + companyId;
    }

    if (departmentId == 0)
    {
      condition = condition;
    }
    else
    {
      if (condition == "")
      {
        condition = "where DEPARTMENTID=" + departmentId;
      }
      else
      {
        condition += " and DEPARTMENTID = " + departmentId;
      }
    }

    if (branchId != 0)
    {
      if (condition != "")
      {
        condition += " and BranchId=" + branchId;
      }
      else
      {
        condition = " where BranchId=" + branchId;
      }
    }

    IEnumerable<EmployeesByCompanyBranchDepartmentDto> result = await _repository.Employees.EmployeeByCompanyIdAndBranchIdAndDepartmentId(condition, cancellationToken);

    return result;
  }


}
