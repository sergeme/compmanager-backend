using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using CompManager.Entities;
using CompManager.Helpers;
using CompManager.Models.Accounts;
using CompManager.Services;

namespace CompManager.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class AccountsController : BaseController
  {
    private readonly IAccountService _accountService;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;

    public AccountsController(
        IAccountService accountService,
        IMapper mapper,
        IOptions<AppSettings> appSettings)
    {
      _accountService = accountService;
      _appSettings = appSettings.Value;
      _mapper = mapper;
    }

    [HttpPost("authenticate")]
    public ActionResult<AuthenticateResponse> Authenticate(AuthenticateRequest model)
    {
      var response = _accountService.Authenticate(model, ipAddress());
      setTokenCookie(response.RefreshToken);
      return Ok(response);
    }

    [HttpPost("refresh-token")]
    public ActionResult<AuthenticateResponse> RefreshToken()
    {
      var refreshToken = Request.Cookies["refreshToken"];
      var response = _accountService.RefreshToken(refreshToken, ipAddress());
      setTokenCookie(response.RefreshToken);
      return Ok(response);
    }

    [Authorize]
    [HttpPost("revoke-token")]
    public IActionResult RevokeToken(RevokeTokenRequest model)
    {
      // accept token from request body or cookie
      var token = model.Token ?? Request.Cookies["refreshToken"];

      if (string.IsNullOrEmpty(token))
        return BadRequest(new { message = "Token is required" });

      // users can revoke their own tokens and admins can revoke any tokens
      if (!Account.OwnsToken(token) && Account.Role != Role.ROLE_ADMIN)
        return Unauthorized(new { message = "Unauthorized" });

      _accountService.RevokeToken(token, ipAddress());
      return Ok(new { message = "Token revoked" });
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest model)
    {
      _accountService.Register(model, _appSettings.FrontendBaseUrl);
      return Ok(new { message = "Registration erfolgreich, prüfe bitte deinen Posteingang und bestätige die Registrierung." });
    }

    [HttpPost("verify-email")]
    public IActionResult VerifyEmail(VerifyEmailRequest model)
    {
      _accountService.VerifyEmail(model.Token);
      return Ok(new { message = "Erfolgreich verifiziert, du kannst dich nun anmelden." });
    }

    [HttpPost("forgot-password")]
    public IActionResult ForgotPassword(ForgotPasswordRequest model)
    {
      _accountService.ForgotPassword(model, Request.Headers["origin"]);
      return Ok(new { message = "E-Mail verschickt, prüfe bitte deinen Posteingang um weitere Instruktionen zu erhalten." });
    }

    [HttpPost("validate-reset-token")]
    public IActionResult ValidateResetToken(ValidateResetTokenRequest model)
    {
      _accountService.ValidateResetToken(model);
      return Ok(new { message = "Token ist gültig" });
    }

    [HttpPost("reset-password")]
    public IActionResult ResetPassword(ResetPasswordRequest model)
    {
      _accountService.ResetPassword(model);
      return Ok(new { message = "Passwort erfolgreich zurückgesetzt, du kannst dich nun anmelden." });
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpGet]
    public ActionResult<IEnumerable<AccountResponse>> GetAll()
    {
      var accounts = _accountService.GetAll();
      return Ok(accounts);
    }

    [Authorize(Role.ROLE_ADMIN, Role.ROLE_TEACHER)]
    [HttpGet("students")]
    public ActionResult<IEnumerable<AccountResponse>> GetStudents()
    {
      var accounts = _accountService.GetStudents();
      return Ok(accounts);
    }

    [Authorize(Role.ROLE_STUDENT)]
    [HttpGet("teachers")]
    public ActionResult<IEnumerable<AccountResponse>> GetTeachers()
    {
      var accounts = _accountService.GetTeachers();
      return Ok(accounts);
    }

    [Authorize(Role.ROLE_STUDENT, Role.ROLE_TEACHER, Role.ROLE_ADMIN)]
    [HttpGet("{id:int}")]
    public ActionResult<AccountResponse> GetById(int id)
    {
      // users can get their own account and admins can get any account
      if (id != Account.Id && Account.Role != Role.ROLE_ADMIN)
        return Unauthorized(new { message = "Unauthorized" });

      var account = _accountService.GetById(id);
      return Ok(account);
    }

    [Authorize(Role.ROLE_ADMIN)]
    [HttpPost]
    public ActionResult<AccountResponse> Create(CreateRequest model)
    {
      var account = _accountService.Create(model);
      return Ok(account);
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public ActionResult<AccountResponse> Update(int id, UpdateRequest model)
    {
      // users can update their own account and admins can update any account
      if (id != Account.Id && Account.Role != Role.ROLE_ADMIN)
        return Unauthorized(new { message = "Unauthorized" });

      // only admins can update role
      if (Account.Role != Role.ROLE_ADMIN)
        model.Role = null;

      var account = _accountService.Update(id, model);
      return Ok(account);
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
      // users can delete their own account and admins can delete any account
      if (id != Account.Id && Account.Role != Role.ROLE_ADMIN)
        return Unauthorized(new { message = "Unauthorized" });

      _accountService.Delete(id);
      return Ok(new { message = "Account erfolgreich gelöscht." });
    }

    // helper methods

    private void setTokenCookie(string token)
    {
      var cookieOptions = new CookieOptions
      {
        HttpOnly = true,
        Expires = DateTime.UtcNow.AddDays(7)
      };
      Response.Cookies.Append("refreshToken", token, cookieOptions);
    }

    private string ipAddress()
    {
      if (Request.Headers.ContainsKey("X-Forwarded-For"))
        return Request.Headers["X-Forwarded-For"];
      else
        return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
    }
  }
}
