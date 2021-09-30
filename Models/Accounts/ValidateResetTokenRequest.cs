using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.Accounts
{
  public class ValidateResetTokenRequest
  {
    [Required]
    public string Token { get; set; }
  }
}