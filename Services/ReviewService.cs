using AutoMapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using CompManager.Entities;
using CompManager.Helpers;
using CompManager.Models.Reviews;

namespace CompManager.Services
{
  public interface IReviewService
  {
    Review Create(CreateRequest model);
    Review GetReview(int reviewId);
  }

  public class ReviewService : IReviewService
  {
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;

    public ReviewService(DataContext context, IMapper mapper, IOptions<AppSettings> appSettings)
    {
      _context = context;
      _mapper = mapper;
      _appSettings = appSettings.Value;
    }

    public Review Create(CreateRequest model)
    {
      Review review = _mapper.Map<Review>(model);

      _context.Reviews.Add(review);
      _context.SaveChanges();

      return review;
    }

    public Review GetReview(int reviewId)
    {
      var review = _context.Reviews.Where(r => r.Id == reviewId).SingleOrDefault();

      if (review == null) throw new KeyNotFoundException("Review nicht gefunden");
      return review;
    }
  }
}