using System.Collections.Generic;
using CompManager.Models.Courses;

namespace CompManager.Models.Departments
{
  public class DepartmentResponse
  {
    public int Id { get; set; }
    public string Name { get; set; }
#nullable enable
    public IEnumerable<CourseResponse>? Courses { get; set; }
#nullable disable

  }
}