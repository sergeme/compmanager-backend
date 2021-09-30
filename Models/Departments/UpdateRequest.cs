using System.ComponentModel.DataAnnotations;
using CompManager.Entities;
using System.Collections.Generic;

namespace CompManager.Models.Departments
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