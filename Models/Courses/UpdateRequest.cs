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
    public List<Course>? Courses { get; set; }
#nullable disable
  }
}