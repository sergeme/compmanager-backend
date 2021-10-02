using AutoMapper;
using BC = BCrypt.Net.BCrypt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using CompManager.Entities;
using CompManager.Helpers;
using CompManager.Models.Curricula;

namespace CompManager.Services
{
  public interface ICurriculumService
  {
    CurriculumResponse Create(CreateRequest model);
    IEnumerable<CurriculumResponse> GetAll();

    CurriculumResponse Update(int id, UpdateRequest model);
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
      var curricula = _context.Curricula;
      var curriculaMapped = _mapper.Map<IList<CurriculumResponse>>(curricula);
      foreach (CurriculumResponse curriculum in curriculaMapped)
      {
        curriculum.ProcessTypes = _processTypeService.GetByCurriculum(curriculum.Id);
      }
      return _mapper.Map<IList<CurriculumResponse>>(curriculaMapped);
    }

    public CurriculumResponse Update(int id, UpdateRequest model)
    {
      var department = getDepartment(id);

      _mapper.Map(model, department);
      _context.Departments.Update(department);
      _context.SaveChanges();

      return _mapper.Map<CurriculumResponse>(department);
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