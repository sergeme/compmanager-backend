using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using CompManager.Entities;
using CompManager.Helpers;
using CompManager.Models.Courses;
using CompManager.Services;

namespace CompManager.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class CoursesController : BaseController
  {
    private readonly ICourseService _courseService;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;

    public CoursesController(
        ICourseService courseService,
        IMapper mapper,
        IOptions<AppSettings> appSettings)
    {
      _courseService = courseService;
      _appSettings = appSettings.Value;
      _mapper = mapper;
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpPost]
    public ActionResult<CourseResponse> Create(CreateRequest model)
    {
      var course = _courseService.Create(model);
      return Ok(course);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpGet]
    public ActionResult<IEnumerable<CourseResponse>> GetAll()
    {
      var courses = _courseService.GetAll();
      return Ok(courses);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpGet("{id:int}")]
    public ActionResult<IEnumerable<CourseResponse>> GetById(int id)
    {
      var courses = _courseService.GetById(id);
      return Ok(courses);
    }

    [Authorize]
    [HttpPut]
    public ActionResult<CourseResponse> Update(UpdateRequest model)
    {
      var course = _courseService.Update(model);
      return Ok(course);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpPut("add-location")]
    public ActionResult<CourseResponse> AddLocation(ChangeCourseLocationRequest model)
    {
      var curriculum = _courseService.AddLocation(model);
      return Ok(curriculum);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpPut("remove-location")]
    public ActionResult<CourseResponse> RemoveLocation(ChangeCourseLocationRequest model)
    {
      var curriculum = _courseService.RemoveLocation(model);
      return Ok(curriculum);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
      _courseService.Delete(id);
      return Ok(new { message = "Lehrgang gelöscht" });
    }
  }
}
