using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.Curricula
{
  public class UpdateRequest
  {
    [Required]
    public string Name { get; set; }
  }
}