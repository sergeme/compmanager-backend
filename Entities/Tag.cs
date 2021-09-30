using System;
using System.Collections.Generic;

namespace CompManager.Entities
{
  public class Tag
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Competence> Competences { get; set; }
    public int VocableId { get; set; }
    public Vocable Vocable { get; set; }
  }
}