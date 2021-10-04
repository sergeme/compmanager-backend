using AutoMapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using CompManager.Entities;
using CompManager.Helpers;
using CompManager.Models.Comments;

namespace CompManager.Services
{
  public interface ICommentService
  {
    Comment Create(CreateRequest model);
    Comment GetComment(int commentId);
  }

  public class CommentService : ICommentService
  {
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;

    public CommentService(DataContext context, IMapper mapper, IOptions<AppSettings> appSettings)
    {
      _context = context;
      _mapper = mapper;
      _appSettings = appSettings.Value;
    }

    public Comment Create(CreateRequest model)
    {
      Comment comment = _mapper.Map<Comment>(model);

      _context.Comments.Add(comment);
      _context.SaveChanges();

      return comment;
    }

    public Comment Update(int id, UpdateRequest model)
    {
      Comment comment = GetComment(id);

      _mapper.Map(model, comment);
      _context.Comments.Update(comment);
      _context.SaveChanges();

      return comment;
    }

    public Comment GetComment(int commentId)
    {
      var comment = _context.Comments.Where(r => r.Id == commentId).SingleOrDefault();

      if (comment == null) throw new KeyNotFoundException("Comment nicht gefunden");
      return comment;
    }
  }
}