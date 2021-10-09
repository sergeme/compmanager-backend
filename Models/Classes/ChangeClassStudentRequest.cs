using System.ComponentModel.DataAnnotations;
using CompManager.Entities;
using System.Collections.Generic;

namespace CompManager.Models.Classes
{
  public class ChangeClassStudentRequest
  {
    [Required]
    public int Id { get; set; }
    [Required]
    public int StudentId { get; set; }
  }
}