using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.Comments
{
  public class UpdateRequest
  {
    [Required]
    public int Id { get; set; }
    [Required]
    public int AccountId { get; set; }
    [Required]
    public int CompetenceId { get; set; }
    [Required]
    public string Content { get; set; }
  }
}