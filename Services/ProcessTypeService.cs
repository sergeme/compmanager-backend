using AutoMapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using CompManager.Entities;
using CompManager.Helpers;
using CompManager.Models.ProcessTypes;
using Microsoft.EntityFrameworkCore;

namespace CompManager.Services
{
  public interface IProcessTypeService
  {
    ProcessTypeResponse Create(CreateRequest model);
    IEnumerable<ProcessTypeResponse> GetAll();
    ProcessTypeResponse GetById(int id, int curriculumId);
    ProcessTypeResponse Update(int id, UpdateRequest model);
    void Delete(int id);
    ProcessType GetProcessType(int id);
  }

  public class ProcessTypeService : IProcessTypeService
  {
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;

    public ProcessTypeService(DataContext context, IMapper mapper, IOptions<AppSettings> appSettings)
    {
      _context = context;
      _mapper = mapper;
      _appSettings = appSettings.Value;
    }
    public ProcessTypeResponse Create(CompManager.Models.ProcessTypes.CreateRequest model)
    {
      // validate
      if (_context.ProcessTypes.Any(x => x.Name == model.Name))
        throw new AppException($"Prozesstyp '{model.Name}' besteht bereits");
      var processType = _mapper.Map<ProcessType>(model);
      _context.ProcessTypes.Add(processType);
      _context.SaveChanges();

      var newProcessType = _context.ProcessTypes.Include(pt => pt.Curricula).Single(pt => pt.Id == processType.Id);
      var curriculum = _context.Curricula.Single(c => c.Id == model.CurriculumId);
      newProcessType.Curricula.Add(curriculum);
      _context.SaveChanges();
      return _mapper.Map<ProcessTypeResponse>(processType);
    }

    public IEnumerable<ProcessTypeResponse> GetAll()
    {
      var processTypes = _context.ProcessTypes.Include(pt => pt.Processes);
      return _mapper.Map<IList<ProcessTypeResponse>>(processTypes);
    }
    public ProcessTypeResponse GetById(int id, int curriculumId)
    {
      var processType = _context.ProcessTypes.Where(pt => pt.Id == id)
      .Select(c => new ProcessType
      {
        Id = c.Id,
        Name = c.Name,
        Processes = c.Processes
        .Where(p => p.CurriculumId == curriculumId /*&& p.ProcessTypeId == id*/)
        .ToList()
      }).First();

      return _mapper.Map<ProcessTypeResponse>(processType);

    }

    public ProcessTypeResponse Update(int id, UpdateRequest model)
    {
      var processType = _context.ProcessTypes
      .Include(pt => pt.Processes)
      .Where(pt => pt.Id == id).First();

      _mapper.Map(model, processType);
      _context.ProcessTypes.Update(processType);
      _context.SaveChanges();

      return _mapper.Map<ProcessTypeResponse>(GetById(id, model.CurriculumId));
    }

    public void Delete(int id)
    {
      var processType = GetProcessType(id);
      _context.ProcessTypes.Remove(processType);
      _context.SaveChanges();
    }

    public ProcessType GetProcessType(int id)
    {
      var processType = _context.ProcessTypes.Find(id);
      if (processType == null) throw new KeyNotFoundException("Prozesstyp nicht gefunden");
      return processType;
    }
  }
}