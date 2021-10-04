namespace CompManager.Entities
{
  public class CurriculumProcessType
  {
    public int CurriculumId { get; set; }
    public int ProcessTypeId { get; set; }

    public Curriculum Curriculum { get; private set; }
    public ProcessType ProcessType { get; private set; }
  }
}