using AutoMapper;
using BC = BCrypt.Net.BCrypt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Microsoft.EntityFrameworkCore;
using CompManager.Entities;
using CompManager.Helpers;
using CompManager.Models.Accounts;
using CompManager.Models.Classes;

namespace CompManager.Services
{
  public interface IAccountService
  {
    AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress);
    AuthenticateResponse RefreshToken(string token, string ipAddress);
    void RevokeToken(string token, string ipAddress);
    void Register(RegisterRequest model, string origin);
    void VerifyEmail(string token);
    void ForgotPassword(ForgotPasswordRequest model, string origin);
    void ValidateResetToken(ValidateResetTokenRequest model);
    void ResetPassword(ResetPasswordRequest model);
    IEnumerable<AccountResponse> GetAll();
    AccountResponse GetById(int id);
    AccountResponse Create(CompManager.Models.Accounts.CreateRequest model);
    AccountResponse Update(int id, CompManager.Models.Accounts.UpdateRequest model);
    void Delete(int id);
  }

  public class AccountService : IAccountService
  {
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;
    private readonly IEmailService _emailService;

    public AccountService(
        DataContext context,
        IMapper mapper,
        IOptions<AppSettings> appSettings,
        IEmailService emailService)
    {
      _context = context;
      _mapper = mapper;
      _appSettings = appSettings.Value;
      _emailService = emailService;
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress)
    {
      var account = _context.Accounts.SingleOrDefault(x => x.Email == model.Email);

      if (account == null || !account.IsVerified || !BC.Verify(model.Password, account.PasswordHash))
        throw new AppException("E-Mail oder Passwort fehlerhaft");

      // authentication successful so generate jwt and refresh tokens
      var jwtToken = generateJwtToken(account);
      var refreshToken = generateRefreshToken(ipAddress);
      account.RefreshTokens.Add(refreshToken);

      // remove old refresh tokens from account
      removeOldRefreshTokens(account);

      // save changes to db
      _context.Update(account);
      _context.SaveChanges();

      var response = _mapper.Map<AuthenticateResponse>(account);
      response.JwtToken = jwtToken;
      response.RefreshToken = refreshToken.Token;
      return response;
    }

    public AuthenticateResponse RefreshToken(string token, string ipAddress)
    {
      var (refreshToken, account) = getRefreshToken(token);

      // replace old refresh token with a new one and save
      var newRefreshToken = generateRefreshToken(ipAddress);
      refreshToken.Revoked = DateTime.UtcNow;
      refreshToken.RevokedByIp = ipAddress;
      refreshToken.ReplacedByToken = newRefreshToken.Token;
      account.RefreshTokens.Add(newRefreshToken);

      removeOldRefreshTokens(account);

      _context.Update(account);
      _context.SaveChanges();

      // generate new jwt
      var jwtToken = generateJwtToken(account);

      var response = _mapper.Map<AuthenticateResponse>(account);
      response.JwtToken = jwtToken;
      response.RefreshToken = newRefreshToken.Token;
      return response;
    }

    public void RevokeToken(string token, string ipAddress)
    {
      var (refreshToken, account) = getRefreshToken(token);

      // revoke token and save
      refreshToken.Revoked = DateTime.UtcNow;
      refreshToken.RevokedByIp = ipAddress;
      _context.Update(account);
      _context.SaveChanges();
    }

    public void Register(RegisterRequest model, string origin)
    {
      // validate
      if (_context.Accounts.Any(x => x.Email == model.Email))
      {
        // send already registered error in email to prevent account enumeration
        sendAlreadyRegisteredEmail(model.Email, origin);
        return;
      }

      // map model to new account object
      var account = _mapper.Map<Account>(model);
      // first registered account is an admin
      var isFirstAccount = _context.Accounts.Count() == 0;
      account.Role = isFirstAccount ? Role.ROLE_ADMIN ^ Role.ROLE_TEACHER ^ Role.ROLE_STUDENT : Role.ROLE_STUDENT;
      account.Created = DateTime.UtcNow;
      account.VerificationToken = randomTokenString();

      // hash password
      account.PasswordHash = BC.HashPassword(model.Password);
      // save account
      _context.Accounts.Add(account);
      _context.SaveChanges();
      Console.WriteLine(model.ClassId);
      if (model.ClassId != 0)
      {
        Class classObj = _context.Classes.Include(cl => cl.Accounts).Where(cl => cl.Id == model.ClassId).First();
        classObj.Accounts.Add(account);
        _context.SaveChanges();
      }
      // send email
      sendVerificationEmail(account, origin);
    }

    public void VerifyEmail(string token)
    {
      var account = _context.Accounts.SingleOrDefault(x => x.VerificationToken == token);

      if (account == null) throw new AppException("Verifikation fehlgeschlagen");

      account.Verified = DateTime.UtcNow;
      account.VerificationToken = null;

      _context.Accounts.Update(account);
      _context.SaveChanges();
    }

    public void ForgotPassword(ForgotPasswordRequest model, string origin)
    {
      var account = _context.Accounts.SingleOrDefault(x => x.Email == model.Email);

      // always return ok response to prevent email enumeration
      if (account == null) return;

      // create reset token that expires after 1 day
      account.ResetToken = randomTokenString();
      account.ResetTokenExpires = DateTime.UtcNow.AddDays(1);

      _context.Accounts.Update(account);
      _context.SaveChanges();

      // send email
      sendPasswordResetEmail(account, origin);
    }

    public void ValidateResetToken(ValidateResetTokenRequest model)
    {
      var account = _context.Accounts.SingleOrDefault(x =>
          x.ResetToken == model.Token &&
          x.ResetTokenExpires > DateTime.UtcNow);

      if (account == null)
        throw new AppException("Ungültiger Token");
    }

    public void ResetPassword(ResetPasswordRequest model)
    {
      var account = _context.Accounts.SingleOrDefault(x =>
          x.ResetToken == model.Token &&
          x.ResetTokenExpires > DateTime.UtcNow);

      if (account == null)
        throw new AppException("Ungültiger Token");

      // update password and remove reset token
      account.PasswordHash = BC.HashPassword(model.Password);
      account.PasswordReset = DateTime.UtcNow;
      account.ResetToken = null;
      account.ResetTokenExpires = null;

      _context.Accounts.Update(account);
      _context.SaveChanges();
    }

    public IEnumerable<AccountResponse> GetAll()
    {
      var accounts = _context.Accounts;
      return _mapper.Map<IList<AccountResponse>>(accounts);
    }

    public AccountResponse GetById(int id)
    {
      var account = getAccount(id);
      return _mapper.Map<AccountResponse>(account);
    }

    public AccountResponse Create(CompManager.Models.Accounts.CreateRequest model)
    {
      // validate
      if (_context.Accounts.Any(x => x.Email == model.Email))
        throw new AppException($"E-Mail '{model.Email}' ist bereits registriert");

      // map model to new account object
      var account = _mapper.Map<Account>(model);
      account.Created = DateTime.UtcNow;
      account.Verified = DateTime.UtcNow;

      // hash password
      account.PasswordHash = BC.HashPassword(model.Password);

      // save account
      _context.Accounts.Add(account);
      _context.SaveChanges();

      return _mapper.Map<AccountResponse>(account);
    }

    public AccountResponse Update(int id, CompManager.Models.Accounts.UpdateRequest model)
    {
      var account = getAccount(id);

      // validate
      if (account.Email != model.Email && _context.Accounts.Any(x => x.Email == model.Email))
        throw new AppException($"Email '{model.Email}' is already taken");

      // hash password if it was entered
      if (!string.IsNullOrEmpty(model.Password))
        account.PasswordHash = BC.HashPassword(model.Password);

      // copy model to account and save
      _mapper.Map(model, account);
      account.Updated = DateTime.UtcNow;
      _context.Accounts.Update(account);
      _context.SaveChanges();

      return _mapper.Map<AccountResponse>(account);
    }

    public void Delete(int id)
    {
      var account = getAccount(id);
      _context.Accounts.Remove(account);
      _context.SaveChanges();
    }

    // helper methods

    private Account getAccount(int id)
    {
      var account = _context.Accounts.Find(id);
      if (account == null) throw new KeyNotFoundException("Account not found");
      return account;
    }

    private (RefreshToken, Account) getRefreshToken(string token)
    {
      var account = _context.Accounts.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
      if (account == null) throw new AppException("Ungültiger Token");
      var refreshToken = account.RefreshTokens.Single(x => x.Token == token);
      if (!refreshToken.IsActive) throw new AppException("Ungültiger Token");
      return (refreshToken, account);
    }

    private string generateJwtToken(Account account)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new[] { new Claim("id", account.Id.ToString()) }),
        Expires = DateTime.UtcNow.AddMinutes(15),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }

    private RefreshToken generateRefreshToken(string ipAddress)
    {
      return new RefreshToken
      {
        Token = randomTokenString(),
        Expires = DateTime.UtcNow.AddDays(7),
        Created = DateTime.UtcNow,
        CreatedByIp = ipAddress
      };
    }

    private void removeOldRefreshTokens(Account account)
    {
      account.RefreshTokens.RemoveAll(x =>
          !x.IsActive &&
          x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
    }

    private string randomTokenString()
    {
      using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
      var randomBytes = new byte[40];
      rngCryptoServiceProvider.GetBytes(randomBytes);
      // convert random bytes to hex string
      return BitConverter.ToString(randomBytes).Replace("-", "");
    }

    private void sendVerificationEmail(Account account, string origin)
    {
      string message;
      var verifyUrl = $"{origin}/verify/{account.VerificationToken}";
      message = $@"<p>Bitte klicke auf den untenstehenden Link um die Registration abzuschliessen:</p>
        <p><a href=""{verifyUrl}"">{verifyUrl}</a></p>";

      _emailService.Send(
        to: account.Email,
        subject: "Kompetenz-Manager - E-Mail verifizieren",
        html: $@"<h4>E-Mail verifizieren</h4>
          <p>Danke fürs registrieren!</p>
          {message}"
      );
    }

    private void sendAlreadyRegisteredEmail(string email, string origin)
    {
      var forgotPasswordUrl = $"{origin}/forgot-password";
      string message = $@"<p>Falls du dein Passwort vergessen hast, besuche die <a href=""{forgotPasswordUrl}"">Passwort vergessen</a> page.</p>";


      _emailService.Send(
          to: email,
          subject: "Kompetenz-Manager - E-Mail bereits registriert",
          html: $@"<h4>E-Mail bereits registriert</h4>
            <p>Deine E-Mailadresse <strong>{email}</strong> ist bereits registriert.</p>
            {message}"
      );
    }

    private void sendPasswordResetEmail(Account account, string origin)
    {
      var resetUrl = $"{origin}/reset-password/{account.ResetToken}";
      string message = $@"<p>Bitte klicke auf untenstehenden Link, um dein Passwort zurückzusetzen, er ist für 24 Stunden gültig:</p>
        <p><a href=""{resetUrl}"">{resetUrl}</a></p>";

      _emailService.Send(
      to: account.Email,
          subject: "Kompetenz-Manager - Passwort zurücksetzen",
          html: $@"<h4>Passwort zurücksetzen</h4>
          {message}"
      );
    }
  }
}
