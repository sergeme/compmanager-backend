using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.Curricula
{
  public class ChangeCurriculumProcessTypeRequest
  {
    [Required]
    public int CurriculumId { get; set; }
    [Required]
    public int ProcessTypeId { get; set; }
  }
}