using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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

    [Authorize(Role.ROLE_STUDENT, Role.ROLE_TEACHER, Role.ROLE_ADMIN)]
    [HttpGet("{id:int}")]
    public ActionResult<CurriculumResponse> GetById(int id)
    {
      var curriculum = _curriculumService.GetById(id);
      return Ok(curriculum);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpPut("{id:int}")]
    public ActionResult<CurriculumResponse> Update(int id, UpdateRequest model)
    {
      var curriculum = _curriculumService.Update(id, model);
      return Ok(curriculum);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpPut("add-processtype")]
    public ActionResult<CurriculumResponse> AddProcessType(ChangeCurriculumProcessTypeRequest model)
    {
      var curriculum = _curriculumService.AddProcessType(model);
      return Ok(curriculum);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpPut("remove-processtype")]
    public ActionResult<CurriculumResponse> RemoveProcessType(ChangeCurriculumProcessTypeRequest model)
    {
      var curriculum = _curriculumService.RemoveProcessType(model);
      return Ok(curriculum);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
      _curriculumService.Delete(id);
      return Ok(new { message = "Lehrplan erfolgreich gelöscht." });
    }
  }
}
