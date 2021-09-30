using AutoMapper;
using BC = BCrypt.Net.BCrypt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CompManager.Entities;
using CompManager.Helpers;
using CompManager.Models.ProcessTypes;
using Microsoft.EntityFrameworkCore;

namespace CompManager.Services
{
  public interface IProcessTypeService
  {
    ProcessTypeResponse Create(CreateRequest model);
    IEnumerable<ProcessTypeResponse> GetByCurriculum(int id);
    ProcessTypeResponse Update(int id, UpdateRequest model);
    void Delete(int id);
  }

  public class ProcessTypeService : IProcessTypeService
  {
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;
    private readonly IProcessService _processService;

    public ProcessTypeService(DataContext context, IMapper mapper, IOptions<AppSettings> appSettings, IProcessService processService)
    {
      _context = context;
      _mapper = mapper;
      _appSettings = appSettings.Value;
      _processService = processService;
    }
    public ProcessTypeResponse Create(CreateRequest model)
    {
      // validate
      if (_context.ProcessTypes.Any(x => x.Name == model.Name))
        throw new AppException($"Prozesstyp '{model.Name}' besteht bereits");

      var processType = _mapper.Map<ProcessType>(model);

      _context.ProcessTypes.Add(processType);
      _context.SaveChanges();

      return _mapper.Map<ProcessTypeResponse>(processType);
    }
    public IEnumerable<ProcessTypeResponse> GetByCurriculum(int curriculumId)
    {
      var processTypes = _context.ProcessTypes
        .Include(p => p.Curricula.Where(c => c.Id == curriculumId));
      var processTypeMapped = _mapper.Map<IList<ProcessTypeResponse>>(processTypes);
      foreach (ProcessTypeResponse processType in processTypeMapped)
      {
        processType.Processes = _processService.GetByProcessTypeAndCurriculum(processType.Id, curriculumId);
      }
      return _mapper.Map<IList<ProcessTypeResponse>>(processTypes);
    }

    public ProcessTypeResponse Update(int id, UpdateRequest model)
    {
      var processType = getProcessType(id);

      _mapper.Map(model, processType);
      _context.ProcessTypes.Update(processType);
      _context.SaveChanges();

      return _mapper.Map<ProcessTypeResponse>(processType);
    }

    public void Delete(int id)
    {
      var processType = getProcessType(id);
      _context.ProcessTypes.Remove(processType);
      _context.SaveChanges();
    }
    private ProcessType getProcessType(int id)
    {
      var processType = _context.ProcessTypes.Find(id);
      if (processType == null) throw new KeyNotFoundException("Prozesstyp nicht gefunden");
      return processType;
    }
  }
}