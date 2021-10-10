using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CompManager.Entities;
using CompManager.Helpers;
using CompManager.Models.Curricula;

namespace CompManager.Services
{
  public interface ICurriculumService
  {
    CurriculumResponse Create(CreateRequest model);
    IEnumerable<CurriculumResponse> GetAll();
    CurriculumResponse GetById(int id);
    CurriculumResponse Update(UpdateRequest model);
    CurriculumResponse AddProcessType(ChangeCurriculumProcessTypeRequest model);
    CurriculumResponse RemoveProcessType(ChangeCurriculumProcessTypeRequest model);
    Curriculum GetCurriculum(int id);
    void Delete(int id);
  }

  public class CurriculumService : ICurriculumService
  {
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;
    private readonly IProcessTypeService _processTypeService;

    public CurriculumService(DataContext context, IMapper mapper, IOptions<AppSettings> appSettings, IProcessTypeService processTypeService)
    {
      _context = context;
      _mapper = mapper;
      _appSettings = appSettings.Value;
      _processTypeService = processTypeService;
    }
    public CurriculumResponse Create(CreateRequest model)
    {
      // validate
      if (_context.Curricula.Any(x => x.Name == model.Name))
        throw new AppException($"Lehrplan '{model.Name}' besteht bereits");

      var curriculum = _mapper.Map<Curriculum>(model);
      curriculum.Created = DateTime.UtcNow;
      _context.Curricula.Add(curriculum);
      _context.SaveChanges();

      return _mapper.Map<CurriculumResponse>(curriculum);
    }
    public IEnumerable<CurriculumResponse> GetAll()
    {
      var curricula = _context.Curricula.Select(c => new Curriculum
      {
        Id = c.Id,
        Name = c.Name,
        Created = c.Created,
        ProcessTypes = c.ProcessTypes.Select(pt => new ProcessType
        {
          Id = pt.Id,
          Name = pt.Name,
          Processes = pt.Processes.Where(p => p.CurriculumId == c.Id).ToList()
        }).ToList()
      });

      return _mapper.Map<IList<CurriculumResponse>>(curricula);
    }

    public CurriculumResponse GetById(int id)
    {
      var curriculum = _context.Curricula.Where(c => c.Id == id)
      .Select(c => new Curriculum
      {
        Id = c.Id,
        Name = c.Name,
        Created = c.Created,
        ProcessTypes = c.ProcessTypes.Select(pt => new ProcessType
        {
          Id = pt.Id,
          Name = pt.Name,
          Processes = pt.Processes.Where(p => p.CurriculumId == id).ToList()
        }).ToList()
      }).First();

      return _mapper.Map<CurriculumResponse>(curriculum);
    }

    public CurriculumResponse Update(UpdateRequest model)
    {
      var curriculum = _context.Curricula
      .Include(c => c.ProcessTypes)
      .Where(c => c.Id == model.Id).First();

      _mapper.Map(model, curriculum);
      _context.Curricula.Update(curriculum);
      _context.SaveChanges();

      return _mapper.Map<CurriculumResponse>(GetById(model.Id));
    }

    public CurriculumResponse AddProcessType(ChangeCurriculumProcessTypeRequest model)
    {
      var curriculum = _context.Curricula
      .Include(c => c.ProcessTypes)
      .Where(c => c.Id == model.CurriculumId).First();

      var processType = _processTypeService.GetProcessType(model.ProcessTypeId);

      curriculum.ProcessTypes.Add(processType);

      _mapper.Map(model, curriculum);
      _context.Curricula.Update(curriculum);
      _context.SaveChanges();

      return _mapper.Map<CurriculumResponse>(GetById(model.CurriculumId));
    }

    public CurriculumResponse RemoveProcessType(ChangeCurriculumProcessTypeRequest model)
    {
      var curriculum = _context.Curricula
      .Include(c => c.ProcessTypes)
      .Where(c => c.Id == model.CurriculumId).First();

      var processType = _processTypeService.GetProcessType(model.ProcessTypeId);

      curriculum.ProcessTypes.Remove(processType);

      _mapper.Map(model, curriculum);
      _context.Curricula.Update(curriculum);
      _context.SaveChanges();

      return _mapper.Map<CurriculumResponse>(GetById(model.CurriculumId));
    }

    public void Delete(int id)
    {
      var curriculum = GetCurriculum(id);

      _context.Curricula.Remove(curriculum);
      _context.SaveChanges();
    }

    public Curriculum GetCurriculum(int id)
    {
      var curriculum = _context.Curricula.Find(id);
      if (curriculum == null) throw new KeyNotFoundException("Lehrplan nicht gefunden");
      return curriculum;
    }
  }
}