using System.ComponentModel.DataAnnotations;
using CompManager.Entities;
using System.Collections.Generic;
using CompManager.Models.Processes;

namespace CompManager.Models.ProcessTypes
{
  public class ProcessTypeResponse
  {
    public int Id { get; set; }
    public string Name { get; set; }
#nullable enable
    public IEnumerable<ProcessResponse>? Processes { get; set; }
#nullable disable

  }
}