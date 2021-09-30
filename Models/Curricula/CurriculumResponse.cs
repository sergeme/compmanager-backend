using System;
using System.Collections.Generic;
using CompManager.Models.ProcessTypes;

namespace CompManager.Models.Curricula
{
  public class CurriculumResponse
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; }
#nullable enable
    public IEnumerable<ProcessTypeResponse>? ProcessTypes { get; set; }
#nullable disable

  }
}