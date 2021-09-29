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

    [HttpPost("")]
    public ActionResult<AuthenticateResponse> Authenticate(AuthenticateRequest model)
    {
      return Ok();
    }
  }
}
