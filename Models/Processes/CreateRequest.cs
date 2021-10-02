using System.ComponentModel.DataAnnotations;
using CompManager.Entities;

namespace CompManager.Models.Processes
{
  public class CreateRequest
  {
    public int ProcessTypeId { get; set; }
    public int CurriculumId { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Content { get; set; }
  }
}