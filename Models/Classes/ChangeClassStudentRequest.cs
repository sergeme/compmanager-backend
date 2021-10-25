using System.ComponentModel.DataAnnotations;

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