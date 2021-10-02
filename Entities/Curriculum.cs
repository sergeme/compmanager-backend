using System;
using System.Collections.Generic;

namespace CompManager.Entities
{
  public class Curriculum
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; }
    public List<CurriculumProcessType> CurriculumProcessTypes { get; set; }
    public List<Process> Processes { get; set; }
    public List<Class> Classes { get; set; }
  }
}