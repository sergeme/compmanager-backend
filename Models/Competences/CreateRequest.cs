using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.Competences
{
  public class CreateRequest
  {
    [Required]
    public int ProcessId { get; set; }
    [Required]
    public int AccountId { get; set; }
    public string Action { get; set; }
    public string Context { get; set; }
    //properties below not used
    public string Description { get; set; }
    public string FinalResults { get; set; }
    public string SuccessCriteria { get; set; }
    public string BasicKnowledge { get; set; }
  }
}