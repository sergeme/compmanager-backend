using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.Competences
{
  public class ChangeCompetenceTagRequest
  {
    [Required]
    public int AccountId { get; set; }
    [Required]
    public int CompetenceId { get; set; }
    [Required]
    public int TagId { get; set; }
  }
}