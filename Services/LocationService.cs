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
using CompManager.Models.Locations;

namespace CompManager.Services
{
  public interface ILocationService
  {
    LocationResponse Create(CreateRequest model);
    IEnumerable<LocationResponse> GetAll();
    IEnumerable<LocationResponse> GetLocationsAndClassesByCourse(int courseId);

    LocationResponse Update(int id, UpdateRequest model);
    void Delete(int id);
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

      return _mapper.Map<LocationResponse>(location);
    }

    public IEnumerable<LocationResponse> GetAll()
    {
      var locations = _context.Locations;
      return _mapper.Map<IList<LocationResponse>>(locations);
    }

    public IEnumerable<LocationResponse> GetLocationsAndClassesByCourse(int courseId)
    {
      var locations = _context.Locations
        .Where(l => l.Classes
          .Any(c => c.CourseId == courseId));

      return _mapper.Map<IList<LocationResponse>>(locations);
    }

    public LocationResponse Update(int id, UpdateRequest model)
    {
      var location = getLocation(id);

      // copy model to account and save
      _mapper.Map(model, location);
      _context.Locations.Update(location);
      _context.SaveChanges();

      return _mapper.Map<LocationResponse>(location);
    }

    public void Delete(int id)
    {
      var location = getLocation(id);
      _context.Locations.Remove(location);
      _context.SaveChanges();
    }
    private Location getLocation(int id)
    {
      var location = _context.Locations.Find(id);
      if (location == null) throw new KeyNotFoundException("Standort nicht gefunden");
      return location;
    }
  }
}