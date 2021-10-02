using System.ComponentModel.DataAnnotations;
using CompManager.Entities;

namespace CompManager.Models.CurriculaProcessTypes
{
  public class CreateRequest
  {
    public int CurriculumId { get; set; }
    public int ProcessTypeId { get; set; }

  }
}