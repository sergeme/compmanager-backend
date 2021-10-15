using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CompManager.Entities;
using CompManager.Helpers;
using CompManager.Models.Processes;
using CompManager.Services;

namespace CompManager.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class ProcessesController : BaseController
  {
    private readonly IProcessService _processService;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;

    public ProcessesController(
        IProcessService processService,
        IMapper mapper,
        IOptions<AppSettings> appSettings)
    {
      _processService = processService;
      _appSettings = appSettings.Value;
      _mapper = mapper;
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpPost]
    public ActionResult<CreateRequest> Create(CreateRequest model)
    {
      var process = _processService.Create(model);
      return Ok(process);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpGet]
    public ActionResult<CreateRequest> GetAll()
    {
      var process = _processService.GetAll();
      return Ok(process);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpPut]
    public ActionResult<ProcessResponse> Update(UpdateRequest model)
    {
      var process = _processService.Update(model);
      return Ok(process);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
      var process = _processService.Delete(id);
      return Ok(process);
    }
  }
}
