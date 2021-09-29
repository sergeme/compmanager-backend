using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
  public class Review
  {
    public int Id { get; set; }
    public int CompetenceId { get; set; }
    public Competence Competence { get; set; }
    public int Reviewer { get; set; }

  }
}