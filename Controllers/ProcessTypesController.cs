using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CompManager.Entities;
using CompManager.Helpers;
using CompManager.Models.ProcessTypes;
using CompManager.Services;

namespace CompManager.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class ProcessTypesController : BaseController
  {
    private readonly IProcessTypeService _processTypeService;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;

    public ProcessTypesController(
        IProcessTypeService processTypeService,
        IMapper mapper,
        IOptions<AppSettings> appSettings)
    {
      _processTypeService = processTypeService;
      _appSettings = appSettings.Value;
      _mapper = mapper;
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpPost]
    public ActionResult<CreateRequest> Create(CreateRequest model)
    {
      var processType = _processTypeService.Create(model);
      return Ok(processType);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpGet]
    public ActionResult<CreateRequest> GetAll()
    {
      var processType = _processTypeService.GetAll();
      return Ok(processType);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpPut("{id:int}")]
    public ActionResult<ProcessTypeResponse> Update(int id, UpdateRequest model)
    {
      var account = _processTypeService.Update(id, model);
      return Ok(account);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
      _processTypeService.Delete(id);
      return Ok(new { message = "Prozesstyp erfolgreich gelöscht." });
    }
  }
}
