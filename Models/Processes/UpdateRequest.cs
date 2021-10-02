using System.ComponentModel.DataAnnotations;
using CompManager.Entities;

namespace CompManager.Models.Processes
{
  public class UpdateRequest
  {
    [Required]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
  }
}