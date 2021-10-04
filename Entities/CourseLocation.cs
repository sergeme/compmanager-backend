namespace CompManager.Entities
{
  public class CourseLocation
  {
    public int CourseId { get; set; }
    public int LocationId { get; set; }

    public Course Course { get; private set; }
    public Location Location { get; private set; }
  }
}