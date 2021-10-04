using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.ProcessTypes
{
  public class CreateRequest
  {
    [Required]
    public int CurriculumId { get; set; }
    [Required]
    public string Name { get; set; }
  }
}