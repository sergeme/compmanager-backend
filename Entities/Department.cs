using System;
using System.Collections.Generic;

namespace CompManager.Entities
{
  public class Department
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Course> Courses { get; set; }
  }
}