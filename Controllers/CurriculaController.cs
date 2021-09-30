using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using CompManager.Entities;
using CompManager.Helpers;
using CompManager.Models.Curricula;
using CompManager.Services;

namespace CompManager.Controllers
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

    [Authorize(Role.ROLE_ADMIN)]
    [HttpPost]
    public ActionResult<CreateRequest> Create(CreateRequest model)
    {
      var curriculum = _curriculumService.Create(model);
      return Ok(curriculum);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpGet]
    public ActionResult<CreateRequest> GetAll(CreateRequest model)
    {
      var curriculum = _curriculumService.GetAll();
      return Ok(curriculum);
    }
  }
}
