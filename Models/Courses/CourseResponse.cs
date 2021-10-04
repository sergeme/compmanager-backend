using System.Collections.Generic;
using CompManager.Models.Locations;

namespace CompManager.Models.Courses
{
  public class CourseResponse
  {
    public int Id { get; set; }
    public string Name { get; set; }
#nullable enable
    public IEnumerable<LocationResponse>? Locations { get; set; }
#nullable disable
  }
}