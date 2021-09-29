using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
  public class Comment
  {
    public int Id { get; set; }
    public int Reviewer { get; set; }
    public DateTime Created { get; set; }
    public int CompetenceId { get; set; }
    public Competence Competence { get; set; }
    public string Content { get; set; }
  }
}