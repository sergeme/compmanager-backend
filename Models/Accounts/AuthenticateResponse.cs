using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using CompManager.Models.Classes;

namespace CompManager.Models.Accounts
{
  public class AuthenticateResponse
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public int Role { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    public bool IsVerified { get; set; }
    public string JwtToken { get; set; }

    [JsonIgnore] // refresh token is returned in http only cookie
    public string RefreshToken { get; set; }
    public IEnumerable<StudentClassesResponse> Classes { get; set; }
  }
}