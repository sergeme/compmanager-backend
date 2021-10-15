using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.ProcessTypes
{
  public class UpdateRequest
  {
    [Required]
    public int Id { get; set; }
    [Required]
    public int CourseId { get; set; }
    [Required]
    public string Name { get; set; }
  }
}