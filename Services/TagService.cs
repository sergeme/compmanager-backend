using AutoMapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using CompManager.Entities;
using CompManager.Helpers;
using Microsoft.EntityFrameworkCore;
using CompManager.Models.Tags;

namespace CompManager.Services
{
  public interface ITagService
  {
    Tag Create(CreateRequest model);
    IEnumerable<TagResponse> GetByAccount(int accountId);
    IEnumerable<TagResponse> GetByCompetence(int competenceId, int accountId);
    Tag GetTag(int tagId, int accountId);
  }

  public class TagService : ITagService
  {
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;

    public TagService(DataContext context, IMapper mapper, IOptions<AppSettings> appSettings)
    {
      _context = context;
      _mapper = mapper;
      _appSettings = appSettings.Value;
    }

    public Tag Create(CreateRequest model)
    {

      Vocable vocable = GetExistingVocable(model);
      Tag tag = GetExistingTag(model, vocable.Id);
      vocable.Tags.Add(tag);
      _context.SaveChanges();

      return tag;
    }

    public IEnumerable<TagResponse> GetByAccount(int accountId)
    {
      var tags = _context.Tags.Where(t => t.AccountId == accountId)
      .Select(t => new Tag
      {
        Id = t.Id,
        Vocable = t.Vocable
      }).ToList();

      return _mapper.Map<IList<TagResponse>>(tags);
    }

    public IEnumerable<TagResponse> GetByCompetence(int competenceId, int accountId)
    {
      var tags = _context.Tags.Where(t => t.Competences.Any(c => c.Id == competenceId) && t.AccountId == accountId)
      .Select(t => new Tag
      {
        Id = t.Id,
        Vocable = t.Vocable
      }).ToList();

      return _mapper.Map<IList<TagResponse>>(tags);
    }

    public Tag GetTag(int tagId, int accountId)
    {
      var tag = _context.Tags.Where(t => t.Id == tagId && t.AccountId == accountId).First();
      if (tag == null) throw new KeyNotFoundException("Tag nicht gefunden");
      return tag;
    }

    private Vocable GetExistingVocable(CreateRequest model)
    {
      Vocable vocable;
      Vocable existingVocable = _context.Vocables.Include(v => v.Tags).SingleOrDefault(v => v.Name == model.Name);
      if (existingVocable == null)
      {
        vocable = _mapper.Map<Vocable>(model);
        _context.Vocables.Add(vocable);
        _context.SaveChanges();
      }
      else
      {
        vocable = existingVocable;
      }
      return vocable;
    }

    private Tag GetExistingTag(CreateRequest model, int vocableId)
    {
      Tag tag;
      Tag existingTag = _context.Tags.Include(t => t.Competences).SingleOrDefault(t => t.AccountId == model.AccountId && t.VocableId == vocableId);
      if (existingTag == null)
      {
        tag = new Tag
        {
          AccountId = model.AccountId,
          VocableId = vocableId
        };

        _context.Tags.Add(tag);
        _context.SaveChanges();
      }
      else
      {
        tag = existingTag;
      }
      return tag;
    }
  }
}