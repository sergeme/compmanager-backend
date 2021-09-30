using System.ComponentModel.DataAnnotations;
using CompManager.Entities;

namespace CompManager.Models.ProcessTypes
{
  public class CreateRequest
  {
    [Required]
    public int Curricula { get; set; }
    [Required]
    public string Name { get; set; }
  }
}