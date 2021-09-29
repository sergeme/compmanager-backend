using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
  public class Course
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int DepartmentId { get; set; }
    public Department Department { get; set; }
    public List<Class> Classes { get; set; }
    public List<ProcessType> ProcessTypes { get; set; }

  }
}