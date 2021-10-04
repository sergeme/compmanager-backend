using System;
using CompManager.Models.Accounts;

namespace CompManager.Models.Comments
{
  public class CommentResponse
  {
    public int Id { get; set; }
    public DateTime Changed { get; set; }
    public AuthorResponse Author { get; set; }
    public string Content { get; set; }
  }
}