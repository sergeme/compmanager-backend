using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using CompManager.Entities;
using CompManager.Helpers;
using CompManager.Models.Departments;
using CompManager.Services;

namespace CompManager.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class DepartmentsController : BaseController
  {
    private readonly IDepartmentService _departmentService;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;

    public DepartmentsController(
        IDepartmentService departmentService,
        IMapper mapper,
        IOptions<AppSettings> appSettings)
    {
      _departmentService = departmentService;
      _appSettings = appSettings.Value;
      _mapper = mapper;
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpPost]
    public ActionResult<DepartmentResponse> Create(CreateRequest model)
    {
      var department = _departmentService.Create(model);
      return Ok(department);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpGet]
    public ActionResult<IEnumerable<DepartmentResponse>> GetAll()
    {
      var departments = _departmentService.GetAll();
      return Ok(departments);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpGet("{id:int}")]
    public ActionResult<IEnumerable<DepartmentResponse>> GetById(int id)
    {
      var department = _departmentService.GetById(id);
      return Ok(department);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpPut("{id:int}")]
    public ActionResult<DepartmentResponse> Update(int id, UpdateRequest model)
    {
      var department = _departmentService.Update(id, model);
      return Ok(department);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
      _departmentService.Delete(id);
      return Ok(new { message = "Standort gelöscht" });
    }
  }
}
