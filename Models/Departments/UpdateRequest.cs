using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.Departments
{
  public class UpdateRequest
  {
    [Required]
    public string Name { get; set; }
  }
}