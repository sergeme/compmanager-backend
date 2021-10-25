using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using AutoMapper;
using CompManager.Entities;
using CompManager.Helpers;
using CompManager.Models.Competences;

namespace CompManager.Services
{
  public interface ICompetenceService
  {
    CompetenceResponse Create(CreateRequest model);
    IEnumerable<CompetenceResponse> GetByAccount(int accountId);
    IEnumerable<CompManager.Models.Tags.TagResponse> GetTagsByAccount(int accountId);
    IEnumerable<CompetencesToReviewResponse> GetByTeacher(int accountId);
    CompetenceResponse GetById(int competenceId, int accountId);
    CompetenceResponse Update(UpdateRequest model);
    CompetenceResponse AddTag(CompManager.Models.Tags.CreateRequest model);
    CompetenceResponse RemoveTag(ChangeCompetenceTagRequest model);
    CompetenceResponse AddReview(CompManager.Models.Reviews.CreateRequest model);
    CompetenceResponse RemoveReview(ChangeCompetenceReviewRequest model);
    IEnumerable<CompetencesToReviewResponse> RemoveReviewByTeacher(ChangeCompetenceReviewRequest model, int accountId);
    CompetenceResponse AddComment(CompManager.Models.Comments.CreateRequest model);
    CompetenceResponse UpdateComment(CompManager.Models.Comments.UpdateRequest model);
    IEnumerable<CompetencesToReviewResponse> AddCommentByTeacher(CompManager.Models.Comments.CreateRequest model, int accountId);
    CompetenceResponse RemoveComment(RemoveCompetenceCommentRequest model);
    IEnumerable<CompetenceResponse> Delete(RemoveRequest model);
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

      return _mapper.Map<CompetenceResponse>(GetById(competence.Id, model.AccountId));
    }

    public IEnumerable<CompetenceResponse> GetByAccount(int accountId)
    {
      var competences = _context.Competences.Where(c => c.AccountId == accountId)
      .Include(c => c.Reviews)
      .Include(c => c.Comments)
      .Include(c => c.Process)
      .Include(c => c.Tags)
      .ThenInclude(t => t.Vocable);
      return _mapper.Map<IList<CompetenceResponse>>(competences);
    }

    public IEnumerable<CompManager.Models.Tags.TagResponse> GetTagsByAccount(int accountId)
    {
      var tags = _tagService.GetByAccount(accountId);

      return _mapper.Map<IList<CompManager.Models.Tags.TagResponse>>(tags);
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
          Process = c.Process,
          Action = c.Action,
          Context = c.Context,
          Description = c.Description,
          FinalResults = c.FinalResults,
          SuccessCriteria = c.SuccessCriteria,
          BasicKnowledge = c.BasicKnowledge
        }).ToList()
      }).ToList();

      Console.WriteLine((competences[0].Competences[0].Reviews == null));

      return _mapper.Map<IList<CompetencesToReviewResponse>>(competences);
    }

    public CompetenceResponse GetById(int competenceId, int accountId)
    {
      var competence = GetCompetence(competenceId, accountId);
      return _mapper.Map<CompetenceResponse>(competence);
    }
    public CompetenceResponse Update(UpdateRequest model)
    {
      Competence competence = _context.Competences
      .Where(c => c.Id == model.Id)
      .Include(c => c.Reviews)
      .Include(c => c.Comments)
      .Include(c => c.Process)
      .Include(c => c.Tags)
      .ThenInclude(t => t.Vocable).First();

      Process process = _context.Processes
      .Where(p => p.Id == model.ProcessId).First();

      competence.Process = process;

      _mapper.Map(model, competence);
      _context.Competences.Update(competence);
      _context.SaveChanges();

      return _mapper.Map<CompetenceResponse>(competence);
    }

    public CompetenceResponse AddTag(CompManager.Models.Tags.CreateRequest model)
    {
      Tag tag = _tagService.Create(model);
      Competence competence = GetCompetence(model.CompetenceId, model.AccountId);

      competence.Tags.Add(tag);
      _context.SaveChanges();

      return _mapper.Map<CompetenceResponse>(competence);
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

      Competence competence = _context.Competences.Find(model.CompetenceId);
      _context.Competences.Update(competence);
      competence.Comments.Find(c => { return c.Id == model.Id; }).Content = model.Content;
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

    public CompetenceResponse RemoveComment(RemoveCompetenceCommentRequest model)
    {
      Competence competence = GetCompetence(model.CompetenceId, model.AccountId);
      Comment comment = _commentService.GetComment(model.CommentId);

      competence.Comments.Remove(comment);

      _mapper.Map(model, competence);
      _context.Competences.Update(competence);
      _context.SaveChanges();

      return _mapper.Map<CompetenceResponse>(GetById(model.CompetenceId, model.AccountId));
    }

    public IEnumerable<CompetenceResponse> Delete(RemoveRequest model)
    {
      Competence competence = GetCompetence(model.CompetenceId, model.AccountId);
      _context.Competences.Remove(competence);
      _context.SaveChanges();

      return _mapper.Map<IList<CompetenceResponse>>(GetByAccount(competence.AccountId));
    }

    private Competence GetCompetence(int id, int accountId)
    {
      Competence competence = _context.Competences
      .Include(c => c.Reviews.OrderBy(review => review.Id))
      .Include(c => c.Comments.OrderBy(comment => comment.Id))
      .Include(c => c.Process)
      .Include(c => c.Tags.OrderBy(tag => tag.Id))
      .ThenInclude(t => t.Vocable)
      .Where(c => c.Id == id && c.AccountId == accountId).First();

      if (competence == null) throw new KeyNotFoundException("Kompetenz nicht gefunden");
      return competence;
    }
  }
}