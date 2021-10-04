using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.Courses
{
  public class CreateRequest
  {
    [Required]
    public int DepartmentId { get; set; }
    [Required]
    public string Name { get; set; }
  }
}