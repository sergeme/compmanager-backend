using AutoMapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using CompManager.Entities;
using CompManager.Helpers;
using CompManager.Models.Departments;
using Microsoft.EntityFrameworkCore;

namespace CompManager.Services
{
  public interface IDepartmentService
  {
    DepartmentResponse Create(CreateRequest model);
    DepartmentResponse GetById(int id);
    IEnumerable<DepartmentResponse> GetAll();
    DepartmentResponse Update(UpdateRequest model);
    DepartmentResponse AddCourse(ChangeDepartmentCourseRequest model);
    DepartmentResponse RemoveCourse(ChangeDepartmentCourseRequest model);
    void Delete(int id);
  }

  public class DepartmentService : IDepartmentService
  {
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;
    private readonly ICourseService _courseService;

    public DepartmentService(DataContext context, IMapper mapper, IOptions<AppSettings> appSettings, ICourseService courseService)
    {
      _context = context;
      _mapper = mapper;
      _appSettings = appSettings.Value;
      _courseService = courseService;
    }

    public DepartmentResponse Create(CreateRequest model)
    {
      // validate
      if (_context.Departments.Any(x => x.Name == model.Name))
        throw new AppException($"Bereich '{model.Name}' besteht bereits");

      var department = _mapper.Map<Department>(model);

      _context.Departments.Add(department);
      _context.SaveChanges();

      return _mapper.Map<DepartmentResponse>(department);
    }
    public IEnumerable<DepartmentResponse> GetAll()
    {
      var departments = _context.Departments.Select(d => new Department
      {
        Id = d.Id,
        Name = d.Name,
        Courses = d.Courses.Select(c => new Course
        {
          Id = c.Id,
          Name = c.Name,
          Locations = c.Locations.Select(l => new Location
          {
            Id = l.Id,
            Name = l.Name,
            Classes = l.Classes.Where(cl => cl.CourseId == c.Id).ToList()
          }).ToList()
        }).ToList()
      });

      return _mapper.Map<IList<DepartmentResponse>>(departments);
    }

    public DepartmentResponse GetById(int id)
    {
      var department = _context.Departments.Where(d => d.Id == id)
      .Select(d => new Department
      {
        Id = d.Id,
        Name = d.Name,
        Courses = d.Courses.Select(c => new Course
        {
          Id = c.Id,
          Name = c.Name,
          Locations = c.Locations.Select(l => new Location
          {
            Id = l.Id,
            Name = l.Name,
            Classes = l.Classes.Where(cl => cl.CourseId == c.Id).ToList()
          }).ToList()
        }).ToList()
      }).First();

      return _mapper.Map<DepartmentResponse>(department);

    }

    public DepartmentResponse Update(UpdateRequest model)
    {
      var department = getDepartment(model.Id);

      _mapper.Map(model, department);
      _context.Departments.Update(department);
      _context.SaveChanges();

      return _mapper.Map<DepartmentResponse>(department);
    }

    public DepartmentResponse AddCourse(ChangeDepartmentCourseRequest model)
    {
      var department = _context.Departments
      .Include(d => d.Courses)
      .Where(d => d.Id == model.DepartmentId).First();

      var course = _courseService.GetCourse(model.CourseId);

      department.Courses.Add(course);

      _mapper.Map(model, department);
      _context.Departments.Update(department);
      _context.SaveChanges();

      return _mapper.Map<DepartmentResponse>(GetById(model.DepartmentId));
    }

    public DepartmentResponse RemoveCourse(ChangeDepartmentCourseRequest model)
    {
      var department = _context.Departments
      .Include(d => d.Courses)
      .Where(d => d.Id == model.DepartmentId).First();

      var course = _courseService.GetCourse(model.CourseId);

      department.Courses.Remove(course);

      _mapper.Map(model, department);
      _context.Departments.Update(department);
      _context.SaveChanges();

      return _mapper.Map<DepartmentResponse>(GetById(model.DepartmentId));
    }

    public void Delete(int id)
    {
      var department = getDepartment(id);
      _context.Departments.Remove(department);
      _context.SaveChanges();
    }
    private Department getDepartment(int id)
    {
      var department = _context.Departments.Find(id);
      if (department == null) throw new KeyNotFoundException("Bereich nicht gefunden");
      return department;
    }
  }
}