using System.Collections.Generic;
using CompManager.Models.Reviews;
using CompManager.Models.Processes;
using CompManager.Models.Comments;
using CompManager.Models.Tags;

namespace CompManager.Models.Competences
{
  public class CompetenceResponse
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
    public IEnumerable<ReviewResponse>? Reviews { get; set; }
    public IEnumerable<CommentResponse>? Comments { get; set; }
    public IEnumerable<TagResponse>? Tags { get; set; }
#nullable disable
  }
}