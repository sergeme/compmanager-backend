using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
  public class Process
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public int ProcessTypeId { get; set; }
    public ProcessType ProcessType { get; set; }
    public int CurriculumId { get; set; }
    public Curriculum Curriculum { get; set; }
  }
}