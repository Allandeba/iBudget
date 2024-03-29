﻿using System.Security.Claims;
using iBudget.Business;
using iBudget.DAO.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace iBudget.Controllers;

public class LoginController : BaseController
{
    private readonly LoginBusiness _business;

    public LoginController(LoginBusiness business)
    {
        _business = business;
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
    public async Task<IActionResult> Login(LoginModel login)
    {
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
        return View(new LoginModel());
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(LoginModel login)
    {
        if (login == null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return View(login);

        await _business.AddAsync(login);
        await StartAuthentication(login);
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    private async Task StartAuthentication(LoginModel login)
    {
        List<Claim> claims =
            new()
            {
                new Claim(ClaimTypes.Name, login.Username),
                new Claim(ClaimTypes.Role, "User")
            };

        ClaimsIdentity claimsIdentity =
            new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        ClaimsPrincipal claimsPrincipal = new(claimsIdentity);
        AuthenticationProperties authenticationProperties =
            new() { IsPersistent = login.Remember };

        await HttpContext.SignInAsync(claimsPrincipal, authenticationProperties);
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