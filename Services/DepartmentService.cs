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
using CompManager.Models.Departments;

namespace CompManager.Services
{
  public interface IDepartmentService
  {
    DepartmentResponse Create(CreateRequest model);
    DepartmentResponse GetById(int id);
    IEnumerable<DepartmentResponse> GetAll();

    DepartmentResponse Update(int id, UpdateRequest model);
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
    public DepartmentResponse GetById(int id)
    {
      var department = getDepartment(id);
      var departmentMapped = _mapper.Map<DepartmentResponse>(department);
      departmentMapped.Courses = _courseService.GetByDepartmentId(id);
      return _mapper.Map<DepartmentResponse>(department);
    }
    public IEnumerable<DepartmentResponse> GetAll()
    {
      var departments = _context.Departments;
      var departmentsMapped = _mapper.Map<IList<DepartmentResponse>>(departments);
      foreach (DepartmentResponse department in departmentsMapped)
      {
        department.Courses = _courseService.GetByDepartmentId(department.Id);
      }
      return _mapper.Map<IList<DepartmentResponse>>(departments);
    }

    public DepartmentResponse Update(int id, UpdateRequest model)
    {
      var department = getDepartment(id);

      _mapper.Map(model, department);
      _context.Departments.Update(department);
      _context.SaveChanges();

      return _mapper.Map<DepartmentResponse>(department);
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