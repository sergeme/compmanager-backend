using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.Competences
{
  public class ChangeCompetenceCommentRequest
  {
    [Required]
    public int AccountId { get; set; }
    [Required]
    public int CompetenceId { get; set; }
    [Required]
    public int CommentId { get; set; }
  }
}