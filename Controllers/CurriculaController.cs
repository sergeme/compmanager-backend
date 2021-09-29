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
  public class CurriculaController : BaseController
  {
    private readonly ICurriculumService _curriculumService;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;

    public CurriculaController(
        ICurriculumService curriculumService,
        IMapper mapper,
        IOptions<AppSettings> appSettings)
    {
      _curriculumService = curriculumService;
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
