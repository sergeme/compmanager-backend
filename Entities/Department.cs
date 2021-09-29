using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
  public class Department
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Course> Courses { get; set; }
  }
}