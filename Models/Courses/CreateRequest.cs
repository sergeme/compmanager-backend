using System.ComponentModel.DataAnnotations;
using CompManager.Entities;

namespace CompManager.Models.Courses
{
  public class CreateRequest
  {
    [Required]
    public string Name { get; set; }
    public int DepartmentId { get; set; }
  }
}