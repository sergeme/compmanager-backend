using System;
using System.Collections.Generic;

namespace CompManager.Entities
{
  public class Class
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; }
    public int CurriculumId { get; set; }
    public Curriculum Curriculum { get; set; }
    public int LocationId { get; set; }
    public Location Location { get; set; }
    public ICollection<Account> Accounts { get; set; }
  }
}