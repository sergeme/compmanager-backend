using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
    public ActionResult<ClassResponse> Create(CreateRequest[] model)
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

    [Authorize(Role.ROLE_ADMIN, Role.ROLE_TEACHER)]
    [HttpPost("by-location-and-course")]
    public ActionResult<IEnumerable<ClassResponse>> GetByLocationAndCourse(ClassesByLocationAndCourseRequest model)
    {
      var classes = _classService.GetByLocationAndCourse(model);
      return Ok(classes);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpPut]
    public ActionResult<ClassResponse> Update(UpdateRequest model)
    {
      var classObj = _classService.Update(model);
      return Ok(classObj);
    }

    [Authorize(Role.ROLE_TEACHER, Role.ROLE_ADMIN)]
    [HttpPut("add-student")]
    public ActionResult<ClassResponse> AddStudent(ChangeClassStudentRequest model)
    {
      var classObj = _classService.AddStudent(model);
      return Ok(classObj);
    }

    [Authorize(Role.ROLE_TEACHER, Role.ROLE_ADMIN)]
    [HttpPut("remove-student")]
    public ActionResult<ClassResponse> RemoveStudent(ChangeClassStudentRequest model)
    {
      var classObj = _classService.RemoveStudent(model);
      return Ok(classObj);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
      _classService.Delete(id);
      return Ok(new { message = "Klasse gelöscht" });
    }
  }
}