namespace CompManager.Entities
{
  public class CompetenceTag
  {
    public int CompetenceId { get; set; }
    public int TagId { get; set; }

    public Competence Competence { get; private set; }
    public Tag Tag { get; private set; }
  }
}