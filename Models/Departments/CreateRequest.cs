using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.Departments
{
  public class CreateRequest
  {
    [Required]
    public string Name { get; set; }
  }
}