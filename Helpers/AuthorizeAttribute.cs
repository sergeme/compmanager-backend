using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Entities;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
  private readonly IList<Role> _roles;

  public AuthorizeAttribute(params Role[] roles)
  {
    _roles = roles ?? new Role[] { };
  }

  public void OnAuthorization(AuthorizationFilterContext context)
  {
    var account = (Account)context.HttpContext.Items["Account"];
    if (account == null || (_roles.Any() && !CheckRoles(_roles, account)))
    {
      // not logged in or role not authorized
      context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
    }
  }

  public bool CheckRoles(IList<Role> _roles, Account account)
  {
    if (account != null)
    {
      int roleSum = _roles.Sum(x => Convert.ToInt32(x));
      int role = Convert.ToInt32(account.Role);
      return Convert.ToBoolean(roleSum & role);
    }
    else return false;
  }
}