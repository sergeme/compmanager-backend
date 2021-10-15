using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;
using CompManager.Entities;
using CompManager.Helpers;
using CompManager.Models.Classes;

namespace CompManager.Services
{
  public interface IClassService
  {
    IEnumerable<ClassResponse> Create(CreateRequest[] model);
    IEnumerable<ClassResponse> GetAll();
    IEnumerable<ClassResponse> GetByLocationAndCourse(ClassesByLocationAndCourseRequest model);
    ClassResponse Update(UpdateRequest model);
    void Delete(int id);
    ClassResponse AddStudent(ChangeClassStudentRequest model);
    ClassResponse RemoveStudent(ChangeClassStudentRequest model);
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
    public IEnumerable<ClassResponse> Create(CreateRequest[] model)
    {
      List<Class> classList = new List<Class>();
      foreach (CreateRequest classObj in model)
      {
        Class obj = new Class();
        if (!_context.Classes.Any(x => x.Name == classObj.Name))
        {
          obj = _mapper.Map<Class>(classObj);
          _context.Classes.Add(obj);
          _context.SaveChanges();
        }
        classList.Add(obj);

      }

      return GetAll();
    }


    public ClassResponse AddStudent(ChangeClassStudentRequest model)
    {
      Class classObj = _context.Classes.Include(cl => cl.Accounts)
      .Where(cl => cl.Id == model.Id)
      .First();

      Account student = _context.Accounts
      .Where(st => st.Id == model.StudentId).First();

      classObj.Accounts.Add(student);
      _context.Update(classObj);
      _context.SaveChanges();

      return _mapper.Map<ClassResponse>(classObj);
    }

    public ClassResponse RemoveStudent(ChangeClassStudentRequest model)
    {
      Class classObj = _context.Classes.Include(cl => cl.Accounts)
      .Where(cl => cl.Id == model.Id)
      .First();

      Account student = _context.Accounts
      .Where(st => st.Id == model.StudentId).First();

      classObj.Accounts.Remove(student);
      _context.Update(classObj);
      _context.SaveChanges();

      return _mapper.Map<ClassResponse>(classObj);
    }

    public IEnumerable<ClassResponse> GetAll()
    {
      var classes = _context.Classes.Include(cl => cl.Accounts);
      return _mapper.Map<IList<ClassResponse>>(classes);
    }

    public IEnumerable<ClassResponse> GetByLocationAndCourse(ClassesByLocationAndCourseRequest model)
    {
      var classes = _context.Classes
      .Include(cl => cl.Accounts)
      .Where(c => c.LocationId == model.LocationId && c.CourseId == model.CourseId);
      return _mapper.Map<IList<ClassResponse>>(classes);
    }

    public ClassResponse Update(UpdateRequest model)
    {
      var classObj = GetClass(model.Id);

      _mapper.Map(model, classObj);
      _context.Classes.Update(classObj);
      _context.SaveChanges();

      return _mapper.Map<ClassResponse>(classObj);
    }

    public void Delete(int id)
    {
      var classObj = GetClass(id);
      _context.Classes.Remove(classObj);
      _context.SaveChanges();
    }

    private Class GetClass(int id)
    {
      var classObj = _context.Classes.Find(id);
      if (classObj == null) throw new KeyNotFoundException("Klasse nicht gefunden");
      return classObj;
    }
  }
}