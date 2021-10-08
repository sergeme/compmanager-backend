using AutoMapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using CompManager.Entities;
using CompManager.Helpers;
using CompManager.Models.Locations;
using Microsoft.EntityFrameworkCore;

namespace CompManager.Services
{
  public interface ILocationService
  {
    LocationResponse Create(CreateRequest model);
    IEnumerable<LocationResponse> GetAll();
    LocationResponse GetById(int id, int courseId);
    LocationResponse Update(UpdateRequest model);
    void Delete(int id);
    Location GetLocation(int id);
  }

  public class LocationService : ILocationService
  {
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;

    public LocationService(DataContext context, IMapper mapper, IOptions<AppSettings> appSettings)
    {
      _context = context;
      _mapper = mapper;
      _appSettings = appSettings.Value;
    }

    public LocationResponse Create(CreateRequest model)
    {
      // validate
      if (_context.Locations.Any(x => x.Name == model.Name))
        throw new AppException($"Standort '{model.Name}' besteht bereits");

      var location = _mapper.Map<Location>(model);
      _context.Locations.Add(location);
      _context.SaveChanges();

      var newLocation = _context.Locations.Include(l => l.Courses).Single(l => l.Id == location.Id);
      var course = _context.Courses.Single(c => c.Id == model.CourseId);
      newLocation.Courses.Add(course);
      _context.SaveChanges();

      return _mapper.Map<LocationResponse>(location);
    }

    public IEnumerable<LocationResponse> GetAll()
    {
      var locations = _context.Locations.Include(l => l.Classes);
      return _mapper.Map<IList<LocationResponse>>(locations);
    }

    public LocationResponse GetById(int id, int courseId)
    {
      var locations = _context.Locations.Where(l => l.Id == id)
      .Select(l => new Location
      {
        Id = l.Id,
        Name = l.Name,
        Classes = l.Classes
        .Where(c => c.CourseId == courseId)
        .ToList()
      }).First();

      return _mapper.Map<LocationResponse>(locations);
    }

    public LocationResponse Update(UpdateRequest model)
    {
      var location = _context.Locations
      .Include(l => l.Classes)
      .Where(l => l.Id == model.Id).First();

      _mapper.Map(model, location);
      _context.Locations.Update(location);
      _context.SaveChanges();

      return _mapper.Map<LocationResponse>(GetLocation(model.Id));
    }

    public void Delete(int id)
    {
      var location = GetLocation(id);
      _context.Locations.Remove(location);
      _context.SaveChanges();
    }
    public Location GetLocation(int id)
    {
      var location = _context.Locations.Find(id);
      if (location == null) throw new KeyNotFoundException("Standort nicht gefunden");
      return location;
    }
  }
}