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
using CompManager.Models.Courses;

namespace CompManager.Services
{
  public interface ICourseService
  {
    CourseResponse Create(CreateRequest model);
    IEnumerable<CourseResponse> GetAll();
    IEnumerable<CourseResponse> GetByDepartmentId(int id);
    CourseResponse Update(int id, UpdateRequest model);
    void Delete(int id);
  }

  public class CourseService : ICourseService
  {
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;
    private readonly ILocationService _locationService;

    public CourseService(DataContext context, IMapper mapper, IOptions<AppSettings> appSettings, ILocationService locationService)
    {
      _context = context;
      _mapper = mapper;
      _appSettings = appSettings.Value;
      _locationService = locationService;
    }

    public CourseResponse Create(CreateRequest model)
    {
      // validate
      if (_context.Courses.Any(x => x.Name == model.Name))
        throw new AppException($"Lehrgang '{model.Name}' besteht bereits");

      var course = _mapper.Map<Course>(model);

      _context.Courses.Add(course);
      _context.SaveChanges();

      return _mapper.Map<CourseResponse>(course);
    }

    public IEnumerable<CourseResponse> GetByDepartmentId(int id)
    {
      var courses = _context.Courses.Where(c => c.DepartmentId == id);
      var coursesMapped = _mapper.Map<IList<CourseResponse>>(courses);
      foreach (CourseResponse course in coursesMapped)
      {
        course.Locations = _locationService.GetLocationsAndClassesByCourse(course.Id);
      }
      return _mapper.Map<IList<CourseResponse>>(courses);
    }

    public IEnumerable<CourseResponse> GetAll()
    {
      var courses = _context.Courses;
      return _mapper.Map<IList<CourseResponse>>(courses);
    }

    public CourseResponse Update(int id, UpdateRequest model)
    {
      var course = getCourse(id);

      // copy model to account and save
      _mapper.Map(model, course);
      _context.Courses.Update(course);
      _context.SaveChanges();

      return _mapper.Map<CourseResponse>(course);
    }

    public void Delete(int id)
    {
      var course = getCourse(id);
      _context.Courses.Remove(course);
      _context.SaveChanges();
    }
    private Course getCourse(int id)
    {
      var course = _context.Courses.Find(id);
      if (course == null) throw new KeyNotFoundException("Lehrgang nicht gefunden");
      return course;
    }
  }
}