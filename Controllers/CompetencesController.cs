using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using CompManager.Entities;
using CompManager.Helpers;
using CompManager.Models.Competences;
using CompManager.Services;

namespace CompManager.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class CompetencesController : BaseController
  {
    private readonly ICompetenceService _competenceService;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;

    public CompetencesController(
        ICompetenceService competenceService,
        IMapper mapper,
        IOptions<AppSettings> appSettings)
    {
      _competenceService = competenceService;
      _appSettings = appSettings.Value;
      _mapper = mapper;
    }

    [Authorize(Role.ROLE_STUDENT)]
    [HttpPost]
    public ActionResult<CompetenceResponse> Create(CreateRequest model)
    {
      var competence = _competenceService.Create(model);
      return Ok(competence);
    }

    [Authorize(Role.ROLE_STUDENT)]
    [HttpGet]
    public ActionResult<IEnumerable<CompetenceResponse>> GetByAccount()
    {
      var competence = _competenceService.GetByAccount(Account.Id);
      return Ok(competence);
    }

    [Authorize(Role.ROLE_STUDENT)]
    [HttpGet("{id:int}")]
    public ActionResult<IEnumerable<CompetenceResponse>> GetById(int id)
    {
      var competence = _competenceService.GetById(id, Account.Id);
      return Ok(competence);
    }

    [Authorize(Role.ROLE_TEACHER)]
    [HttpGet("to-review")]
    public ActionResult<IEnumerable<CompetencesToReviewResponse>> GetByTeacher()
    {
      var competence = _competenceService.GetByTeacher(Account.Id);
      return Ok(competence);
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public ActionResult<CompetenceResponse> Update(int id, UpdateRequest model)
    {
      var competence = _competenceService.Update(id, model);
      return Ok(competence);
    }

    [Authorize(Role.ROLE_STUDENT)]
    [HttpPost("add-tag")]
    public ActionResult<CompetenceResponse> AddTag(CompManager.Models.Tags.CreateRequest model)
    {
      if (model.AccountId != Account.Id)
        return Unauthorized(new { message = "Unauthorized" });
      var competence = _competenceService.AddTag(model);
      return Ok(competence);
    }

    [Authorize(Role.ROLE_STUDENT)]
    [HttpDelete("remove-tag")]
    public ActionResult<CompetenceResponse> RemoveTag(ChangeCompetenceTagRequest model)
    {
      if (model.AccountId != Account.Id)
        return Unauthorized(new { message = "Unauthorized" });

      var competence = _competenceService.RemoveTag(model);
      return Ok(competence);
    }

    [Authorize(Role.ROLE_STUDENT)]
    [HttpPost("add-review")]
    public ActionResult<CompetenceResponse> AddReview(CompManager.Models.Reviews.CreateRequest model)
    {
      if (model.AccountId != Account.Id)
        return Unauthorized(new { message = "Unauthorized" });

      var competence = _competenceService.AddReview(model);
      return Ok(competence);
    }

    [Authorize(Role.ROLE_STUDENT)]
    [HttpDelete("remove-review")]
    public ActionResult<CompetenceResponse> RemoveReview(ChangeCompetenceReviewRequest model)
    {
      if (model.AccountId != Account.Id)
        return Unauthorized(new { message = "Unauthorized" });

      var competence = _competenceService.RemoveReview(model);
      return Ok(competence);
    }

    [Authorize(Role.ROLE_TEACHER)]
    [HttpDelete("remove-review-by-teacher")]
    public ActionResult<CompetencesToReviewResponse> RemoveReviewByTeacher(ChangeCompetenceReviewRequest model)
    {
      if (model.AccountId != Account.Id)
        return Unauthorized(new { message = "Unauthorized" });

      var competence = _competenceService.RemoveReviewByTeacher(model, Account.Id);
      return Ok(competence);
    }

    [Authorize(Role.ROLE_STUDENT)]
    [HttpPost("add-comment")]
    public ActionResult<CompetenceResponse> AddComment(CompManager.Models.Comments.CreateRequest model)
    {
      if (model.AccountId != Account.Id)
        return Unauthorized(new { message = "Unauthorized" });

      var competence = _competenceService.AddComment(model);
      return Ok(competence);
    }

    [Authorize(Role.ROLE_STUDENT)]
    [HttpPut("update-comment")]
    public ActionResult<CompetenceResponse> UpdateComment(CompManager.Models.Comments.UpdateRequest model)
    {
      if (model.AccountId != Account.Id)
        return Unauthorized(new { message = "Unauthorized" });

      var competence = _competenceService.UpdateComment(model);
      return Ok(competence);
    }

    [Authorize(Role.ROLE_TEACHER)]
    [HttpPost("add-comment-by-teacher")]
    public ActionResult<CompetenceResponse> AddCommentByTeacher(CompManager.Models.Comments.CreateRequest model)
    {
      if (model.AccountId != Account.Id)
        return Unauthorized(new { message = "Unauthorized" });

      var competence = _competenceService.AddCommentByTeacher(model, Account.Id);
      return Ok(competence);
    }

    [Authorize(Role.ROLE_STUDENT)]
    [HttpDelete("remove-comment")]
    public ActionResult<CompetenceResponse> RemoveComment(RemoveCompetenceCommentRequest model)
    {
      if (model.AccountId != Account.Id)
        return Unauthorized(new { message = "Unauthorized" });

      var competence = _competenceService.RemoveComment(model);
      return Ok(competence);
    }

    [Authorize(Role.ROLE_STUDENT)]
    [HttpDelete("{id:int}")]
    public IActionResult Delete(RemoveRequest model)
    {
      if (model.AccountId != Account.Id)
        return Unauthorized(new { message = "Unauthorized" });

      _competenceService.Delete(model.CompetenceId, Account.Id);
      return Ok(new { message = "Kompetenz gelöscht" });
    }
  }
}
