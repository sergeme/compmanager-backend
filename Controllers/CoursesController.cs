using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Accounts;
using WebApi.Services;

namespace WebApi.Controllers
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

    [HttpPost("")]
    public ActionResult<AuthenticateResponse> Authenticate(AuthenticateRequest model)
    {
      return Ok();
    }
  }
}
