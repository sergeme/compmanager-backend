using System;
using System.Collections.Generic;
using CompManager.Models.Accounts;

namespace CompManager.Models.Classes
{
  public class ClassResponse
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int CurriculumId { get; set; }
    public int CourseId { get; set; }
    public int LocationId { get; set; }
#nullable enable
    public IEnumerable<StudentResponse>? Students { get; set; }
#nullable disable
  }
}