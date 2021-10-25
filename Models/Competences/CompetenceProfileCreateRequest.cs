using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.Competences
{
  public class CompetenceProfileCreateRequest
  {
    [Required]
    public int[] Competences { get; set; }
  }
}