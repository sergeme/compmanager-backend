using AutoMapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CompManager.Entities;
using CompManager.Helpers;
using CompManager.Models.Courses;

namespace CompManager.Services
{
  public interface ICourseService
  {
    CourseResponse Create(CreateRequest model);
    IEnumerable<CourseResponse> GetAll();
    CourseResponse GetById(int id);
    CourseResponse Update(int id, UpdateRequest model);
    CourseResponse AddLocation(ChangeCourseLocationRequest model);
    CourseResponse RemoveLocation(ChangeCourseLocationRequest model);
    Course GetCourse(int id);
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

    public IEnumerable<CourseResponse> GetAll()
    {
      var courses = _context.Courses.Select(c => new Course
      {
        Id = c.Id,
        Name = c.Name,
        Locations = c.Locations.Select(l => new Location
        {
          Id = l.Id,
          Name = l.Name,
          Classes = l.Classes.Where(cl => cl.CourseId == c.Id).ToList()
        }).ToList()
      });
      return _mapper.Map<IList<CourseResponse>>(courses);
    }

    public CourseResponse GetById(int id)
    {
      var course = _context.Courses.Where(c => c.Id == id)
      .Select(c => new Course
      {
        Id = c.Id,
        Name = c.Name,
        Locations = c.Locations.Select(l => new Location
        {
          Id = l.Id,
          Name = l.Name,
          Classes = l.Classes.Where(cl => cl.CourseId == id).ToList()
        }).ToList()
      }).First();

      return _mapper.Map<CourseResponse>(course);
    }

    public CourseResponse Update(int id, UpdateRequest model)
    {
      var course = _context.Courses
      .Include(c => c.Locations)
      .Where(c => c.Id == id).First();

      _mapper.Map(model, course);
      _context.Courses.Update(course);
      _context.SaveChanges();

      return _mapper.Map<CourseResponse>(GetById(id));
    }

    public CourseResponse AddLocation(ChangeCourseLocationRequest model)
    {
      var course = _context.Courses
      .Include(c => c.Locations)
      .Where(c => c.Id == model.CourseId).First();

      var location = _locationService.GetLocation(model.LocationId);

      course.Locations.Add(location);

      _mapper.Map(model, course);
      _context.Courses.Update(course);
      _context.SaveChanges();

      return _mapper.Map<CourseResponse>(GetById(model.CourseId));
    }

    public CourseResponse RemoveLocation(ChangeCourseLocationRequest model)
    {
      var course = _context.Courses
      .Include(c => c.Locations)
      .Where(c => c.Id == model.CourseId).First();

      var location = _locationService.GetLocation(model.LocationId);

      course.Locations.Remove(location);

      _mapper.Map(model, course);
      _context.Courses.Update(course);
      _context.SaveChanges();

      return _mapper.Map<CourseResponse>(GetById(model.CourseId));
    }

    public void Delete(int id)
    {
      var course = GetCourse(id);
      _context.Courses.Remove(course);
      _context.SaveChanges();
    }

    public Course GetCourse(int id)
    {
      var course = _context.Courses.Find(id);
      if (course == null) throw new KeyNotFoundException("Lehrgang nicht gefunden");
      return course;
    }
  }
}