using System.Collections.Generic;
using Newtonsoft.Json;

namespace CompManager.Entities
{
  public class Tag
  {
    public int Id { get; set; }
    public int AccountId { get; set; }
    [JsonIgnore]
    public Account Account { get; set; }
    [JsonIgnore]
    public ICollection<Competence> Competences { get; set; }
    public int VocableId { get; set; }
    [JsonIgnore]
    public Vocable Vocable { get; set; }
  }
}