using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.Tags
{
  public class RemoveRequest
  {
    [Required]
    public int AccountId { get; set; }
    [Required]
    public int CompetenceId { get; set; }
    [Required]
    public int TagId { get; set; }
  }
}