using System.ComponentModel.DataAnnotations;
using CompManager.Entities;
using System;
using System.Collections.Generic;

namespace CompManager.Models.Classes
{
  public class CreateRequest
  {
    [Required]
    public string Name { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    [Required]
    public int CourseId { get; set; }
    [Required]
    public int CurriculumId { get; set; }
    [Required]
    public int LocationId { get; set; }
  }
}