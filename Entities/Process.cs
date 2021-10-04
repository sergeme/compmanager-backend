using Newtonsoft.Json;

namespace CompManager.Entities
{
  public class Process
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public int ProcessTypeId { get; set; }
    [JsonIgnore]
    public ProcessType ProcessType { get; set; }
    public int CurriculumId { get; set; }
    [JsonIgnore]
    public Curriculum Curriculum { get; set; }
  }
}