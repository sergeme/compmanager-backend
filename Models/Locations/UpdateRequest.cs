using System.ComponentModel.DataAnnotations;
using CompManager.Entities;

namespace CompManager.Models.Locations
{
  public class UpdateRequest
  {
    [Required]
    public string Name { get; set; }
  }
}