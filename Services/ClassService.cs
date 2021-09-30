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
using CompManager.Models.Classes;

namespace CompManager.Services
{
  public interface IClassService
  {
    ClassResponse Create(CreateRequest model);
    ClassResponse GetById(int id);
    IEnumerable<ClassResponse> GetAll();

    ClassResponse Update(int id, UpdateRequest model);
    void Delete(int id);
  }

  public class ClassService : IClassService
  {
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;
    private readonly IEmailService _emailService;

    public ClassService(DataContext context, IMapper mapper, IOptions<AppSettings> appSettings)
    {
      _context = context;
      _mapper = mapper;
      _appSettings = appSettings.Value;
    }
    public ClassResponse Create(CreateRequest model)
    {
      // validate
      if (_context.Departments.Any(x => x.Name == model.Name))
        throw new AppException($"Bereich '{model.Name}' besteht bereits");

      var department = _mapper.Map<Department>(model);

      _context.Departments.Add(department);
      _context.SaveChanges();

      return _mapper.Map<ClassResponse>(department);
    }
    public ClassResponse GetById(int id)
    {
      var classObj = getClass(id);
      return _mapper.Map<ClassResponse>(classObj);
    }

    public IEnumerable<ClassResponse> GetAll()
    {
      var classes = _context.Classes;
      return _mapper.Map<IList<ClassResponse>>(classes);
    }

    public ClassResponse Update(int id, UpdateRequest model)
    {
      var classObj = getClass(id);

      _mapper.Map(model, classObj);
      _context.Classes.Update(classObj);
      _context.SaveChanges();

      return _mapper.Map<ClassResponse>(classObj);
    }

    public void Delete(int id)
    {
      var classObj = getClass(id);
      _context.Classes.Remove(classObj);
      _context.SaveChanges();
    }
    private Class getClass(int id)
    {
      var classObj = _context.Classes.Find(id);
      if (classObj == null) throw new KeyNotFoundException("Klasse nicht gefunden");
      return classObj;
    }
  }
}