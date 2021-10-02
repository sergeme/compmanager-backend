using System.ComponentModel.DataAnnotations;
using CompManager.Entities;
using System.Collections.Generic;
using CompManager.Models.Classes;

namespace CompManager.Models.Processes
{
  public class ProcessResponse
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
  }
}