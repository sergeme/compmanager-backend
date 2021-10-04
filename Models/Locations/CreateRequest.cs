using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.Locations
{
  public class CreateRequest
  {
    [Required]
    public int CourseId { get; set; }
    [Required]
    public string Name { get; set; }
  }
}