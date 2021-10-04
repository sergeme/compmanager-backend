using AutoMapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using CompManager.Entities;
using CompManager.Helpers;
using CompManager.Models.Processes;

namespace CompManager.Services
{
  public interface IProcessService
  {
    ProcessResponse Create(CreateRequest model);
    IEnumerable<ProcessResponse> GetAll();
    IEnumerable<ProcessResponse> GetByProcessTypeAndCurriculum(int processTypeId, int curriculumId);
    ProcessResponse Update(int id, UpdateRequest model);
    IEnumerable<ProcessResponse> Delete(int id);
  }

  public class ProcessService : IProcessService
  {
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;

    public ProcessService(DataContext context, IMapper mapper, IOptions<AppSettings> appSettings)
    {
      _context = context;
      _mapper = mapper;
      _appSettings = appSettings.Value;
    }

    public ProcessResponse Create(CreateRequest model)
    {
      var process = _mapper.Map<Process>(model);

      _context.Processes.Add(process);
      _context.SaveChanges();

      return _mapper.Map<ProcessResponse>(process);
    }

    public IEnumerable<ProcessResponse> GetAll()
    {
      var processes = _context.Processes;
      return _mapper.Map<IList<ProcessResponse>>(processes);
    }

    public IEnumerable<ProcessResponse> GetByProcessTypeAndCurriculum(int processTypeId, int curriculumId)
    {
      var processes = _context.Processes
        .Where(p => p.ProcessTypeId == processTypeId && p.CurriculumId == curriculumId);
      return _mapper.Map<IList<ProcessResponse>>(processes);
    }

    public ProcessResponse Update(int id, UpdateRequest model)
    {
      var process = GetProcess(id);

      _mapper.Map(model, process);
      _context.Processes.Update(process);
      _context.SaveChanges();

      return _mapper.Map<ProcessResponse>(process);
    }

    public IEnumerable<ProcessResponse> Delete(int id)
    {
      var process = GetProcess(id);
      _context.Processes.Remove(process);
      _context.SaveChanges();

      return _mapper.Map<IList<ProcessResponse>>(GetByProcessTypeAndCurriculum(process.ProcessTypeId, process.CurriculumId));
    }

    private Process GetProcess(int id)
    {
      var process = _context.Processes.Find(id);
      if (process == null) throw new KeyNotFoundException("Prozess nicht gefunden");
      return process;
    }
  }
}