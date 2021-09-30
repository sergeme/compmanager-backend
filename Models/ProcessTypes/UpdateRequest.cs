using System.ComponentModel.DataAnnotations;
using CompManager.Entities;

namespace CompManager.Models.ProcessTypes
{
  public class UpdateRequest
  {
    [Required]
    public string Name { get; set; }
  }
}