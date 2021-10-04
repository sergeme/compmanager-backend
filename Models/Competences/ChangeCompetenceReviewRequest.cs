using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.Competences
{
  public class ChangeCompetenceReviewRequest
  {
    [Required]
    public int AccountId { get; set; }
    [Required]
    public int CompetenceId { get; set; }
    [Required]
    public int ReviewId { get; set; }
  }
}