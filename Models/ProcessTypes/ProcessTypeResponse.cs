using System.Collections.Generic;
using CompManager.Models.Processes;

namespace CompManager.Models.ProcessTypes
{
  public class ProcessTypeResponse
  {
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string Name { get; set; }

  }
}