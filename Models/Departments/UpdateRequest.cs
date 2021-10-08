using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.Departments
{
  public class UpdateRequest
  {
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
  }
}