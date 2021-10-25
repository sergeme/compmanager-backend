using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.Classes
{
  public class ClassesByLocationAndCourseRequest
  {
    [Required]
    public int LocationId { get; set; }
    [Required]
    public int CourseId { get; set; }
  }
}