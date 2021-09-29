using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
  public class Competence
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int ProcessId { get; set; }
    public Process Process { get; set; }
    public int AccountId { get; set; }
    public Account Account { get; set; }
    public string Action { get; set; }
    public string Context { get; set; }
    public string Description { get; set; }
    public string FinalResults { get; set; }
    public string SuccessCriteria { get; set; }
    public string BasicKnowledge { get; set; }
    public CompetenceType CompetenceType { get; set; }
    public List<Review> Reviews { get; set; }
    public ICollection<Tag> Tags { get; set; }

  }
}