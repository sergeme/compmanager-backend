using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CompManager.Entities;

namespace CompManager.Controllers
{
  [Controller]
  public abstract class BaseController : ControllerBase
  {
    // returns the current authenticated account (null if not logged in)
    public Account Account => (Account)HttpContext.Items["Account"];
  }
}
