using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.Locations
{
  public class UpdateRequest
  {
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
  }
}