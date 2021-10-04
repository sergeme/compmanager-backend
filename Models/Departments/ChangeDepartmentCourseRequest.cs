using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.Departments
{
  public class ChangeDepartmentCourseRequest
  {
    [Required]
    public int DepartmentId { get; set; }
    [Required]
    public int CourseId { get; set; }
  }
}