using System.Collections.Generic;

namespace CompManager.Entities
{
  public class Course
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int DepartmentId { get; set; }
    public Department Department { get; set; }
    public ICollection<Location> Locations { get; set; }
    public List<ProcessType> Processtypes { get; set; }

  }
}