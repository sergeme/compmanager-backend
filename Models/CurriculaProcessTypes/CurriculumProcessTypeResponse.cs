using System.ComponentModel.DataAnnotations;
using CompManager.Entities;

using System.Collections.Generic;
using CompManager.Models.ProcessTypes;

namespace CompManager.Models.CurriculaProcessTypes
{
  public class CurriculumProcessTypeResponse
  {
#nullable enable
    public IEnumerable<ProcessType>? ProcessTypes { get; set; }
#nullable disable

  }
}