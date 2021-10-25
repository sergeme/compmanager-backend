using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.Classes
{
  public class UpdateRequest
  {
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
  }
}