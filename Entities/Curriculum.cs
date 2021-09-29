using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
  public class Curriculum
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; }
    public ICollection<ProcessType> ProcessTypes { get; set; }
    public List<Process> Processes { get; set; }
    public List<Class> Classes { get; set; }
  }
}