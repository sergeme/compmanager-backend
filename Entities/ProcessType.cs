using System.Collections.Generic;
using Newtonsoft.Json;

namespace CompManager.Entities
{
  public class ProcessType
  {
    public int Id { get; set; }
    public string Name { get; set; }
    [JsonIgnore]
    public ICollection<Curriculum> Curricula { get; set; }
#nullable enable    
    [JsonIgnore]
    public List<Process>? Processes { get; set; }
    public int? CourseId { get; set; }
    public Course? Course { get; set; }
#nullable disable
  }
}