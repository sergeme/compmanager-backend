using System.Collections.Generic;

namespace CompManager.Entities
{
  public class Competence
  {
    public int Id { get; set; }
    public int ProcessId { get; set; }
    public Process Process { get; set; }
    public int AccountId { get; set; }
    public Account Account { get; set; }
    public string Action { get; set; }
    public string Context { get; set; }
    //properties below not used right now
    public string Description { get; set; }
    public string FinalResults { get; set; }
    public string SuccessCriteria { get; set; }
    public string BasicKnowledge { get; set; }
    //end of unused properties
    public List<Review> Reviews { get; set; }
    public List<Comment> Comments { get; set; }
    public ICollection<Tag> Tags { get; set; }

  }
}