using CompManager.Models.Accounts;

namespace CompManager.Models.Reviews
{
  public class ReviewResponse
  {
    public int Id { get; set; }
    public ReviewerResponse Reviewer { get; set; }
  }
}