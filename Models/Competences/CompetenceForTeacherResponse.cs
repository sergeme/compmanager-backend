using CompManager.Models.Processes;

namespace CompManager.Models.Competences
{
  public class CompetenceForTeacherResponse
  {
    public int Id { get; set; }
    public string Name { get; set; }
#nullable enable
    public ProcessResponse? Process { get; set; }
    public string? Action { get; set; }
    public string? Context { get; set; }
    public string? Description { get; set; }
    public string? FinalResults { get; set; }
    public string? SuccessCriteria { get; set; }
    public string? BasicKnowledge { get; set; }
#nullable disable
  }
}