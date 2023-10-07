﻿using getQuote.Business;
using getQuote.Framework;
using getQuote.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace getQuote.Controllers
{
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
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Index));
            }
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

        private async Task StartAuthentication(LoginModel login)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, login.Username),
                new Claim(ClaimTypes.Role, "User"),
            };

            ClaimsIdentity claimsIdentity = new(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );
            ClaimsPrincipal claimsPrincipal = new(claimsIdentity);
            AuthenticationProperties authenticationProperties = new()
            {
                IsPersistent = login.Remember
            };

            await HttpContext.SignInAsync(claimsPrincipal, authenticationProperties);
        }

        private LoginLogModel GetLoginLog(LoginModel login, LoginLogStatus loginLogStatus)
        {
            LoginLogModel loginLog = new()
            {
                Username = login.Username,
                Password = login.Password,
                Hostname = Dns.GetHostEntry(HttpContext.Connection.RemoteIpAddress).HostName,
                RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                DateTime = DateTime.Now,
                Status = loginLogStatus
            };

            return loginLog;
        }
    }
}
