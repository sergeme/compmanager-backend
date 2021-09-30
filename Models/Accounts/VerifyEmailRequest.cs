using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.Accounts
{
  public class VerifyEmailRequest
  {
    [Required]
    public string Token { get; set; }
  }
}