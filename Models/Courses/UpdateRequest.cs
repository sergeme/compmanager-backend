using System.ComponentModel.DataAnnotations;
using CompManager.Entities;
using System.Collections.Generic;

namespace CompManager.Models.Courses
{
  public class UpdateRequest
  {
    [Required]
    public string Name { get; set; }
#nullable enable
    public List<Class>? Classes { get; set; }
#nullable disable
  }
}