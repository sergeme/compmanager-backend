using System.Collections.Generic;
using Newtonsoft.Json;

namespace CompManager.Entities
{
  public class Department
  {
    public int Id { get; set; }
    public string Name { get; set; }
    [JsonIgnore]
    public List<Course> Courses { get; set; }
  }
}