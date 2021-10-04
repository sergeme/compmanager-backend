using System;
using System.Collections.Generic;

namespace CompManager.Entities
{
  public class Account
  {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public bool AcceptTerms { get; set; }
    public Role Role { get; set; }
    public string VerificationToken { get; set; }
    public DateTime? Verified { get; set; }
    public bool IsVerified => Verified.HasValue || PasswordReset.HasValue;
    public string ResetToken { get; set; }
    public DateTime? ResetTokenExpires { get; set; }
    public DateTime? PasswordReset { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; }
    public ICollection<Class> Classes { get; set; }
    public List<Competence> Competences { get; set; }
    public List<Tag> Tags { get; set; }
    public List<Review> Reviews { get; set; }
    public List<Comment> Comments { get; set; }
    public CompetenceType CompetenceType { get; set; } //decides which editor is being used

    public bool OwnsToken(string token)
    {
      return this.RefreshTokens?.Find(x => x.Token == token) != null;
    }
  }
}