using System.Security.Claims;
using iBudget.Business;
using iBudget.DAO.Entities;
using iBudget.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace iBudget.Controllers;

public class LoginController : BaseController
{
    private readonly LoginBusiness _business;
    private readonly DemoAccess _demoAccess;
    private readonly ILogger<LoginController> _logger;
    private readonly TimeProvider _timeProvider;

    public LoginController(LoginBusiness business, DemoAccess demoAccess, ILogger<LoginController> logger,
        TimeProvider timeProvider)
    {
        _business = business;
        _demoAccess = demoAccess;
        _logger = logger;
        _timeProvider = timeProvider;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        if (!User.Identity!.IsAuthenticated)
            return View(new LoginModel());

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Demo()
    {
        if (!_demoAccess.IsEnabled)
            return NotFound();

        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction(nameof(HomeController.Index), "Home");

        var previousCookies = Response.Headers.SetCookie;
        var now = _timeProvider.GetUtcNow();
        try
        {
            await StartAuthentication("Visitante", new AuthenticationProperties
            {
                IsPersistent = false,
                AllowRefresh = false,
                IssuedUtc = now,
                ExpiresUtc = now.AddHours(8)
            }, isDemo: true);
        }
        catch (Exception exception)
        {
            Response.Headers.SetCookie = previousCookies;
            _logger.LogError(new EventId(1001, "DemoSessionFailed"), exception,
                "Demo session failed at {Timestamp}", now);
            ViewData["DemoError"] = "Não foi possível abrir a demonstração. Tente novamente.";
            Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            return View(nameof(Index), new LoginModel());
        }

        _logger.LogInformation(new EventId(1000, "DemoSessionStarted"),
            "Demo session started at {Timestamp}", now);
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginModel login)
    {
        if (_demoAccess.IsEnabled)
            return NotFound();

        if (!ModelState.IsValid)
            return View(nameof(Index));

        try
        {
            await _business.Login(login);
        }
        catch (Exception)
        {
            await _business.SaveLoginLog(GetLoginLog(login, LoginLogStatus.Failed));
            throw;
        }

        await _business.SaveLoginLog(GetLoginLog(login, LoginLogStatus.Success));

        await StartAuthentication(login);

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Index));
    }

    [AllowAnonymous]
    public IActionResult Register()
    {
        if (_demoAccess.IsEnabled)
            return NotFound();

        return View(new LoginModel());
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(LoginModel login)
    {
        if (_demoAccess.IsEnabled)
            return NotFound();

        if (login == null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return View(login);

        await _business.AddAsync(login);
        await StartAuthentication(login);
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    private Task StartAuthentication(LoginModel login)
    {
        return StartAuthentication(login.Username, new AuthenticationProperties { IsPersistent = login.Remember });
    }

    private async Task StartAuthentication(string username, AuthenticationProperties properties, bool isDemo = false)
    {
        List<Claim> claims =
            new()
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "User")
            };

        if (isDemo)
            claims.Add(new Claim(DemoAccess.ClaimType, "true"));

        ClaimsIdentity claimsIdentity =
            new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        ClaimsPrincipal claimsPrincipal = new(claimsIdentity);
        await HttpContext.SignInAsync(claimsPrincipal, properties);
    }

    private LoginLogModel GetLoginLog(LoginModel login, LoginLogStatus loginLogStatus)
    {
        LoginLogModel loginLog =
            new()
            {
                Username = login.Username,
                Password = login.Password,
                RemoteIpAddress = login.IpAddress,
                DateTime = DateTime.UtcNow,
                Status = loginLogStatus
            };

        return loginLog;
    }
}
