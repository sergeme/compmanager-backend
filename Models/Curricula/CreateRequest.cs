using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.Curricula
{
  public class CreateRequest
  {
    [Required]
    public string Name { get; set; }
  }
}