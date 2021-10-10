namespace CompManager.Models.Processes
{
  public class ProcessResponse
  {
    public int Id { get; set; }
    public int ProcessTypeId { get; set; }
    public int CurriculumId { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
  }
}