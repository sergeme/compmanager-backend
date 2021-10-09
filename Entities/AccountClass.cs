namespace CompManager.Entities
{
  public class AccountClass
  {
    public int ClassId { get; set; }
    public int AccountId { get; set; }

    public Class Class { get; private set; }
    public Account Account { get; private set; }
  }
}