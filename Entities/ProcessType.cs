using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
  public class ProcessType
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Curriculum> Curricula { get; set; }
    public List<Process> Processes { get; set; }

#nullable enable
    public int? CourseId { get; set; }
    public Course? Course { get; set; }
#nullable disable



  }
}