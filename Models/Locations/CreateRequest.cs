using System.ComponentModel.DataAnnotations;
using CompManager.Entities;

namespace CompManager.Models.Locations
{
  public class CreateRequest
  {
    [Required]
    public string Name { get; set; }
  }
}