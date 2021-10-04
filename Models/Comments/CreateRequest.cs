namespace CompManager.Models.Comments
{
  public class CreateRequest
  {
    public int AccountId { get; set; }
    public int CompetenceId { get; set; }
    public string Content { get; set; }
  }
}