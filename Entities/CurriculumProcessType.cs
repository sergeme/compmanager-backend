using System;
using System.Collections.Generic;

namespace CompManager.Entities
{
  public class CurriculumProcessType
  {
    public int CurriculaId { get; set; }
    public Curriculum Curriculum { get; set; }
    public int ProcessTypesId { get; set; }
    public ProcessType ProcessType { get; set; }
  }
}