using AutoMapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using CompManager.Entities;
using CompManager.Helpers;
using Microsoft.EntityFrameworkCore;
using CompManager.Models.Competences;

namespace CompManager.Services
{
  public interface ICompetenceService
  {
    CompetenceResponse Create(CreateRequest model);
    IEnumerable<CompetenceResponse> GetByAccount(int accountId);
    IEnumerable<CompetencesToReviewResponse> GetByTeacher(int accountId);
    CompetenceResponse GetById(int competenceId, int accountId);
    CompetenceResponse Update(int competenceId, UpdateRequest model);
    CompetenceResponse AddTag(CompManager.Models.Tags.CreateRequest model);
    CompetenceResponse RemoveTag(ChangeCompetenceTagRequest model);
    CompetenceResponse AddReview(CompManager.Models.Reviews.CreateRequest model);
    CompetenceResponse RemoveReview(ChangeCompetenceReviewRequest model);
    IEnumerable<CompetencesToReviewResponse> RemoveReviewByTeacher(ChangeCompetenceReviewRequest model, int accountId);
    CompetenceResponse AddComment(CompManager.Models.Comments.CreateRequest model);
    CompetenceResponse UpdateComment(CompManager.Models.Comments.UpdateRequest model);
    IEnumerable<CompetencesToReviewResponse> AddCommentByTeacher(CompManager.Models.Comments.CreateRequest model, int accountId);
    CompetenceResponse RemoveComment(ChangeCompetenceCommentRequest model);
    IEnumerable<CompetenceResponse> Delete(int competenceId, int accountId);
  }

  public class CompetenceService : ICompetenceService
  {
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;
    private readonly ITagService _tagService;
    private readonly IReviewService _reviewService;
    private readonly ICommentService _commentService;

    public CompetenceService(
      DataContext context,
      IMapper mapper,
      IOptions<AppSettings> appSettings,
      ITagService tagService,
      IReviewService reviewService,
      ICommentService commentService)
    {
      _context = context;
      _mapper = mapper;
      _appSettings = appSettings.Value;
      _tagService = tagService;
      _reviewService = reviewService;
      _commentService = commentService;
    }

    public CompetenceResponse Create(CreateRequest model)
    {
      var competence = _mapper.Map<Competence>(model);

      _context.Competences.Add(competence);
      _context.SaveChanges();

      return _mapper.Map<CompetenceResponse>(competence);
    }

    public IEnumerable<CompetenceResponse> GetByAccount(int accountId)
    {
      var competences = _context.Competences.Where(c => c.AccountId == accountId)
      .Select(comp => new Competence
      {
        Id = comp.Id,
        Name = comp.Name,
        Action = comp.Action,
        Context = comp.Context,
        Description = comp.Description,
        FinalResults = comp.FinalResults,
        SuccessCriteria = comp.SuccessCriteria,
        BasicKnowledge = comp.BasicKnowledge,
        Reviews = comp.Reviews.Select(r => new Review
        {
          Id = r.Id,
          Account = r.Account
        }).ToList(),
        Comments = comp.Comments.Select(c => new Comment
        {
          Id = c.Id,
          Changed = c.Changed,
          Account = c.Account,
          Content = c.Content
        }).ToList(),
        Tags = comp.Tags.Select(t => new Tag
        {
          Id = t.Id,
          Vocable = t.Vocable
        }).ToList()
      });
      return _mapper.Map<IList<CompetenceResponse>>(competences);
    }

    public IEnumerable<CompetencesToReviewResponse> GetByTeacher(int accountId)
    {
      var competences = _context.Accounts.Where(a => a.Competences.Any(c => c.Reviews.Any(r => r.AccountId == accountId)))
      .Select(a => new Account
      {
        FirstName = a.FirstName,
        LastName = a.LastName,
        Competences = a.Competences.Where(c => c.Reviews.Any(r => r.AccountId == accountId))
        .Select(c => new Competence
        {
          Id = c.Id,
          Name = c.Name,
          Process = c.Process,
          Action = c.Action,
          Context = c.Context,
          Description = c.Description,
          FinalResults = c.FinalResults,
          SuccessCriteria = c.SuccessCriteria,
          BasicKnowledge = c.BasicKnowledge
        }).ToList()
      }).ToList();

      return _mapper.Map<IList<CompetencesToReviewResponse>>(competences);
    }

    public CompetenceResponse GetById(int competenceId, int accountId)
    {
      var competences = _context.Competences.Where(c => c.AccountId == accountId && c.Id == competenceId)
      .Select(comp => new Competence
      {
        Id = comp.Id,
        Name = comp.Name,
        Action = comp.Action,
        Context = comp.Context,
        Description = comp.Description,
        FinalResults = comp.FinalResults,
        SuccessCriteria = comp.SuccessCriteria,
        BasicKnowledge = comp.BasicKnowledge,
        Reviews = comp.Reviews.Select(r => new Review
        {
          Id = r.Id,
          Account = r.Account
        }).ToList(),
        Comments = comp.Comments.Select(c => new Comment
        {
          Id = c.Id,
          Changed = c.Changed,
          Account = c.Account,
          Content = c.Content
        }).ToList(),
        Tags = comp.Tags.Select(t => new Tag
        {
          Id = t.Id,
          Vocable = t.Vocable
        }).ToList()
      }).First();
      return _mapper.Map<CompetenceResponse>(competences);
    }
    public CompetenceResponse Update(int competenceId, UpdateRequest model)
    {
      Competence competence = _context.Competences
      .Include(c => c.Reviews)
      .Include(c => c.Comments)
      .Include(c => c.Tags)
      .Where(c => c.Id == competenceId).First();

      _mapper.Map(model, competence);
      _context.Competences.Update(competence);
      _context.SaveChanges();

      return _mapper.Map<CompetenceResponse>(GetById(competenceId, model.AccountId));
    }

    public CompetenceResponse AddTag(CompManager.Models.Tags.CreateRequest model)
    {
      _tagService.Create(model);

      return _mapper.Map<CompetenceResponse>(GetById(model.CompetenceId, model.AccountId));
    }

    public CompetenceResponse RemoveTag(ChangeCompetenceTagRequest model)
    {
      Tag tag = _tagService.GetTag(model.TagId, model.AccountId);
      Competence competence = GetCompetence(model.CompetenceId, model.AccountId);

      competence.Tags.Remove(tag);
      _context.SaveChanges();

      return _mapper.Map<CompetenceResponse>(GetById(model.CompetenceId, model.AccountId));
    }

    public CompetenceResponse AddReview(CompManager.Models.Reviews.CreateRequest model)
    {
      _reviewService.Create(model);

      return _mapper.Map<CompetenceResponse>(GetById(model.CompetenceId, model.AccountId));
    }

    public CompetenceResponse RemoveReview(ChangeCompetenceReviewRequest model)
    {
      Competence competence = GetCompetence(model.CompetenceId, model.AccountId);
      Review review = _reviewService.GetReview(model.ReviewId);

      competence.Reviews.Remove(review);

      _mapper.Map(model, competence);
      _context.Competences.Update(competence);
      _context.SaveChanges();

      return _mapper.Map<CompetenceResponse>(GetById(model.CompetenceId, model.AccountId));
    }

    public IEnumerable<CompetencesToReviewResponse> RemoveReviewByTeacher(ChangeCompetenceReviewRequest model, int accountId)
    {
      Review review = _reviewService.GetReview(model.ReviewId);

      _context.Reviews.Remove(review);
      _context.SaveChanges();
      return _mapper.Map<IList<CompetencesToReviewResponse>>(GetByTeacher(accountId));
    }

    public CompetenceResponse AddComment(CompManager.Models.Comments.CreateRequest model)
    {
      _commentService.Create(model);

      return _mapper.Map<CompetenceResponse>(GetById(model.CompetenceId, model.AccountId));
    }

    public CompetenceResponse UpdateComment(CompManager.Models.Comments.UpdateRequest model)
    {
      Comment comment = _context.Comments.Find(model.Id);
      _mapper.Map(model, comment);
      _context.Comments.Update(comment);
      _context.SaveChanges();

      return _mapper.Map<CompetenceResponse>(GetById(model.CompetenceId, model.AccountId));
    }

    public IEnumerable<CompetencesToReviewResponse> AddCommentByTeacher(CompManager.Models.Comments.CreateRequest model, int accountId)
    {
      Competence competence = GetCompetence(model.CompetenceId, model.AccountId);

      _mapper.Map(model, competence);
      _context.Competences.Update(competence);
      _context.SaveChanges();

      return _mapper.Map<IList<CompetencesToReviewResponse>>(GetByTeacher(accountId));
    }

    public CompetenceResponse RemoveComment(ChangeCompetenceCommentRequest model)
    {
      Competence competence = GetCompetence(model.CompetenceId, model.AccountId);
      Comment comment = _commentService.GetComment(model.CommentId);

      competence.Comments.Remove(comment);

      _mapper.Map(model, competence);
      _context.Competences.Update(competence);
      _context.SaveChanges();

      return _mapper.Map<CompetenceResponse>(GetById(model.CompetenceId, model.AccountId));
    }

    public IEnumerable<CompetenceResponse> Delete(int competenceId, int accountId)
    {
      Competence competence = GetCompetence(competenceId, accountId);
      _context.Competences.Remove(competence);
      _context.SaveChanges();

      return _mapper.Map<IList<CompetenceResponse>>(GetByAccount(competence.AccountId));
    }

    private Competence GetCompetence(int id, int accountId)
    {
      Competence competence = _context.Competences
      .Include(c => c.Reviews)
      .Include(c => c.Comments)
      .Include(c => c.Tags)
      .Where(c => c.Id == id && c.AccountId == accountId).First();

      if (competence == null) throw new KeyNotFoundException("Kompetenz nicht gefunden");
      return competence;
    }
  }
}