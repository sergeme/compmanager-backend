using System.Collections.Generic;

namespace CompManager.Models.Competences
{
  public class CompetencesToReviewResponse
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
#nullable enable
    public IEnumerable<CompetenceForTeacherResponse>? Competences { get; set; }
#nullable disable
  }
}