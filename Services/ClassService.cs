using AutoMapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using CompManager.Entities;
using CompManager.Helpers;
using CompManager.Models.Classes;

namespace CompManager.Services
{
  public interface IClassService
  {
    ClassResponse Create(CreateRequest model);
    IEnumerable<ClassResponse> GetAll();
    IEnumerable<ClassResponse> GetByLocationAndCourse(int locationId, int courseId);
    ClassResponse Update(int id, UpdateRequest model);
    IEnumerable<ClassResponse> Delete(int id);
  }

  public class ClassService : IClassService
  {
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;

    public ClassService(DataContext context, IMapper mapper, IOptions<AppSettings> appSettings)
    {
      _context = context;
      _mapper = mapper;
      _appSettings = appSettings.Value;
    }
    public ClassResponse Create(CreateRequest model)
    {
      // validate
      if (_context.Classes.Any(x => x.Name == model.Name))
        throw new AppException($"Klasse '{model.Name}' besteht bereits");

      var classObj = _mapper.Map<Class>(model);

      _context.Classes.Add(classObj);
      _context.SaveChanges();

      return _mapper.Map<ClassResponse>(classObj);
    }

    public IEnumerable<ClassResponse> GetAll()
    {
      var classes = _context.Classes;
      return _mapper.Map<IList<ClassResponse>>(classes);
    }

    public IEnumerable<ClassResponse> GetByLocationAndCourse(int locationId, int courseId)
    {
      var classes = _context.Classes
        .Where(c => c.LocationId == locationId && c.CourseId == courseId);
      return _mapper.Map<IList<ClassResponse>>(classes);
    }

    public ClassResponse Update(int id, UpdateRequest model)
    {
      var classObj = GetClass(id);

      _mapper.Map(model, classObj);
      _context.Classes.Update(classObj);
      _context.SaveChanges();

      return _mapper.Map<ClassResponse>(classObj);
    }

    public IEnumerable<ClassResponse> Delete(int id)
    {
      var classObj = GetClass(id);
      _context.Classes.Remove(classObj);
      _context.SaveChanges();

      return _mapper.Map<IList<ClassResponse>>(GetByLocationAndCourse(classObj.LocationId, classObj.CourseId));

    }
    private Class GetClass(int id)
    {
      var classObj = _context.Classes.Find(id);
      if (classObj == null) throw new KeyNotFoundException("Klasse nicht gefunden");
      return classObj;
    }
  }
}