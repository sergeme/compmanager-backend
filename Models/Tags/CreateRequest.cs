using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.Tags
{
  public class CreateRequest
  {
    [Required]
    public int AccountId { get; set; }
    [Required]
    public int CompetenceId { get; set; }
    [Required]
    public string Vocable { get; set; }
  }
}