using System.ComponentModel.DataAnnotations;
using CompManager.Entities;
using System.Collections.Generic;
using CompManager.Models.Classes;

namespace CompManager.Models.Locations
{
  public class LocationResponse
  {
    public int Id { get; set; }
    public string Name { get; set; }
#nullable enable
    public IEnumerable<ClassResponse>? Classes { get; set; }
#nullable disable
  }
}