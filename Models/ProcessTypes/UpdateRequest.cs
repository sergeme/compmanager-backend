using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.ProcessTypes
{
  public class UpdateRequest
  {
    [Required]
    public int CurriculumId { get; set; }
    [Required]
    public string Name { get; set; }
  }
}