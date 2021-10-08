namespace CompManager.Models.Reviews
{
  public class CreateRequest
  {
    public int AccountId { get; set; }
    public int AuthorId { get; set; }
    public int CompetenceId { get; set; }
  }
}