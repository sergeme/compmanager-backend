using System.ComponentModel.DataAnnotations;
using CompManager.Entities;
using System.Collections.Generic;

namespace CompManager.Models.Courses
{
  public class UpdateRequest
  {
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
  }
}