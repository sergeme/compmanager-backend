using System.ComponentModel.DataAnnotations;
using CompManager.Entities;
using System.Collections.Generic;

namespace CompManager.Models.Classes
{
  public class ClassesByLocationAndCourseRequest
  {
    [Required]
    public int LocationId { get; set; }
    [Required]
    public int CourseId { get; set; }
  }
}