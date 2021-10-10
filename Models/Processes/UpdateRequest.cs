using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.Processes
{
  public class UpdateRequest
  {
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Content { get; set; }
  }
}