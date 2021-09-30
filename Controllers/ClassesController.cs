using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using CompManager.Entities;
using CompManager.Helpers;
using CompManager.Models.Classes;
using CompManager.Services;

namespace CompManager.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class ClassesController : BaseController
  {
    private readonly IClassService _classService;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;

    public ClassesController(
        IClassService classService,
        IMapper mapper,
        IOptions<AppSettings> appSettings)
    {
      _classService = classService;
      _appSettings = appSettings.Value;
      _mapper = mapper;
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpPost]
    public ActionResult<ClassResponse> Create(CreateRequest model)
    {
      var classObj = _classService.Create(model);
      return Ok(classObj);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpGet]
    public ActionResult<IEnumerable<ClassResponse>> GetAll()
    {
      var classes = _classService.GetAll();
      return Ok(classes);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpGet("{id:int}")]
    public ActionResult<IEnumerable<ClassResponse>> GetById(int id)
    {
      var classObj = _classService.GetById(id);
      return Ok(classObj);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpPut("{id:int}")]
    public ActionResult<ClassResponse> Update(int id, UpdateRequest model)
    {
      var classObj = _classService.Update(id, model);
      return Ok(classObj);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
      _classService.Delete(id);
      return Ok(new { message = "Standort gelöscht" });
    }
  }
}