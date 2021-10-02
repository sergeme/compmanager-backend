using System;
using System.Collections.Generic;

namespace CompManager.Entities
{
  public class ProcessType
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public List<CurriculumProcessType> CurriculumProcessType { get; set; }
#nullable enable    
    public List<Process>? Processes { get; set; }
    public int? CourseId { get; set; }
    public Course? Course { get; set; }
#nullable disable



  }
}