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
using CompManager.Models.CurriculaProcessTypes;
using Microsoft.EntityFrameworkCore;

namespace CompManager.Services
{
  public interface IProcessTypeService
  {
    ProcessTypeResponse Create(CompManager.Models.ProcessTypes.CreateRequest model);
    IEnumerable<ProcessTypeResponse> GetByCurriculum(int curriculumId);
    IEnumerable<ProcessTypeResponse> GetAll();
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
    public ProcessTypeResponse Create(CompManager.Models.ProcessTypes.CreateRequest model)
    {
      // validate
      if (_context.ProcessTypes.Any(x => x.Name == model.Name))
        throw new AppException($"Prozesstyp '{model.Name}' besteht bereits");

      var processType = _mapper.Map<ProcessType>(model);
      _context.ProcessTypes.Add(processType);
      _context.SaveChanges();

      var curriculum = _context.Curricula.Find(model.Curricula);

      _context.CurriculumProcessTypes.Add(new CurriculumProcessType
      {
        Curriculum = curriculum,
        ProcessType = processType
      });
      _context.SaveChanges();
      return _mapper.Map<ProcessTypeResponse>(processType);
    }
    public IEnumerable<ProcessTypeResponse> GetByCurriculum(int curriculumId)
    {
      var curriculum = _context.Curricula
      .Where(c => c.CurriculumProcessTypes
      .Any(cpt => cpt.CurriculaId == c.Id))
      .Include(c => c.Processes);

      var processTypes = _context.ProcessTypes
      .Where(pt => pt.CurriculumProcessType
      .Any(cpt => cpt.CurriculaId == curriculumId))
      .Include(pt => pt.Processes);

      return _mapper.Map<IList<ProcessTypeResponse>>(processTypes);
    }

    public IEnumerable<ProcessTypeResponse> GetAll()
    {
      var processTypes = _context.ProcessTypes;
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