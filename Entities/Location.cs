using System;
using System.Collections.Generic;

namespace CompManager.Entities
{
  public class Location
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Class> Classes { get; set; }
  }
}