using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.Locations
{
  public class UpdateRequest
  {
    [Required]
    public string Name { get; set; }
    [Required]
    public int CourseId { get; set; }
  }
}