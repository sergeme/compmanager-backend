using System.ComponentModel.DataAnnotations;
using CompManager.Entities;

namespace CompManager.Models.Curricula
{
  public class CreateRequest
  {
    [Required]
    public string Name { get; set; }
  }
}