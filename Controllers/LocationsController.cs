using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using CompManager.Entities;
using CompManager.Helpers;
using CompManager.Models.Locations;
using CompManager.Services;

namespace CompManager.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class LocationsController : BaseController
  {
    private readonly ILocationService _locationService;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;

    public LocationsController(
        ILocationService locationService,
        IMapper mapper,
        IOptions<AppSettings> appSettings)
    {
      _locationService = locationService;
      _appSettings = appSettings.Value;
      _mapper = mapper;
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpPost]
    public ActionResult<LocationResponse> Create(CreateRequest model)
    {
      var location = _locationService.Create(model);
      return Ok(location);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpGet]
    public ActionResult<IEnumerable<LocationResponse>> GetAll()
    {
      var locations = _locationService.GetAll();
      return Ok(locations);
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public ActionResult<LocationResponse> Update(int id, UpdateRequest model)
    {
      var location = _locationService.Update(id, model);
      return Ok(location);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
      _locationService.Delete(id);
      return Ok(new { message = "Standort gelöscht" });
    }
  }
}
