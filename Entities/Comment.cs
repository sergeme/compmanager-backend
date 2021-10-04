using System;

namespace CompManager.Entities
{
  public class Comment
  {
    public int Id { get; set; }
    public int AccountId { get; set; }
    public Account Account { get; set; }
    public DateTime Changed { get; set; }
    public int CompetenceId { get; set; }
    public Competence Competence { get; set; }
    public string Content { get; set; }
  }
}