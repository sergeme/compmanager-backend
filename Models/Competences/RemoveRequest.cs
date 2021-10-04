using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.Competences
{
  public class RemoveRequest
  {
    [Required]
    public int AccountId { get; set; }
    [Required]
    public int CompetenceId { get; set; }
  }
}