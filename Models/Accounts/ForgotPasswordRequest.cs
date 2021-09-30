using System.ComponentModel.DataAnnotations;

namespace CompManager.Models.Accounts
{
  public class ForgotPasswordRequest
  {
    [Required]
    [EmailAddress]
    public string Email { get; set; }
  }
}