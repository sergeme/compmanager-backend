using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.Courses
{
  public class ChangeCourseLocationRequest
  {
    [Required]
    public int CourseId { get; set; }
    [Required]
    public int LocationId { get; set; }
  }
}