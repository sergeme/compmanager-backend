using System;
using CompManager.Models.Curricula;
using CompManager.Models.Courses;

namespace CompManager.Models.Classes
{
  public class StudentClassesResponse
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int CourseId { get; set; }
    public CourseResponse Course { get; set; }
    public CurriculumResponse Curriculum { get; set; }
  }
}