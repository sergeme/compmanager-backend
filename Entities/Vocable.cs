using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
  public class Vocable
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Tag> Tags { get; set; }
  }
}