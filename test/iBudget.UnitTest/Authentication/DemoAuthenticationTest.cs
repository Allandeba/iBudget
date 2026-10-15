using System.Net;
using System.Text.RegularExpressions;
using iBudget.DAO;
using iBudget.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Shared;

namespace iBudget.UnitTest.Authentication;

public class DemoAuthenticationTest
{
    [Theory]
    [InlineData("Development")]
    [InlineData("Staging")]
    [InlineData("Production")]
    public async Task ShouldRenderConventionalLoginWithoutPassword(string environment)
    {
        using var factory = new DemoApplicationFactory(enabled: false, environment: environment);
        using var client = factory.CreateBrowserClient();
        var entry = await client.GetStringAsync("/Login");
        var input = Regex.Match(entry, "<input[^>]+name=\"Password\"[^>]*>").Value;
        Assert.NotEmpty(input);
        Assert.DoesNotContain(DemoApplicationFactory.TestPassword, entry);
        Assert.Equal(string.Empty, Regex.Match(input, "value=\"([^\"]*)\"").Groups[1].Value);
    }

    [Fact]
    public async Task ShouldRenderPasswordFreeEntryAndReachHomeWithServerIdentity()
    {
        using var factory = new DemoApplicationFactory();
        using var client = factory.CreateBrowserClient();
        var entry = await client.GetStringAsync("/Login");
        Assert.Contains("Explorar demonstração", entry);
        Assert.Contains("id=\"demo_badge\"", entry);
        Assert.DoesNotContain("name=\"Password\"", entry);
        Assert.DoesNotContain("name=\"Username\"", entry);
        Assert.DoesNotContain("name=\"Remember\"", entry);
        Assert.DoesNotContain("ipify", entry);

        var response = await PostEntry(client, entry, "/Login/Demo", new()
        {
            ["Username"] = "injected-user",
            ["Password"] = "injected-password",
            ["Remember"] = "true",
            ["returnUrl"] = "https://example.com",
            ["role"] = "Administrator"
        });
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Equal("/", response.Headers.Location?.OriginalString);
        var cookie = Assert.Single(response.Headers.GetValues("Set-Cookie"), value => value.StartsWith("authCookie="));
        Assert.DoesNotContain("expires=", cookie, StringComparison.OrdinalIgnoreCase);
        var cookieValue = cookie.Split(';')[0]["authCookie=".Length..];
        var options = factory.Services.GetRequiredService<IOptionsMonitor<CookieAuthenticationOptions>>()
            .Get(CookieAuthenticationDefaults.AuthenticationScheme);
        var ticket = options.TicketDataFormat.Unprotect(cookieValue);
        Assert.NotNull(ticket);
        Assert.Equal("Visitante", ticket.Principal.Identity?.Name);
        Assert.True(ticket.Principal.IsInRole("User"));
        Assert.False(ticket.Principal.IsInRole("Administrator"));
        Assert.True(DemoAccess.IsDemo(ticket.Principal));
        Assert.False(ticket.Properties.IsPersistent);
        Assert.False(ticket.Properties.AllowRefresh);
        Assert.Equal(TimeSpan.FromHours(8), ticket.Properties.ExpiresUtc - ticket.Properties.IssuedUtc);

        var home = await client.GetStringAsync("/Home");
        Assert.Contains("id=\"IndexMenu\"", home);
        Assert.Contains("id=\"demoSessionNotice\"", home);
        var existingSession = await client.GetAsync("/Login");
        Assert.Equal(HttpStatusCode.Redirect, existingSession.StatusCode);
        Assert.Equal("/", existingSession.Headers.Location?.OriginalString);

        using var scope = factory.Services.CreateScope();
        Assert.Empty(scope.ServiceProvider.GetRequiredService<ApplicationDBContext>().LoginLog);
        Assert.Single(scope.ServiceProvider.GetRequiredService<ApplicationDBContext>().Login);
    }

    [Theory]
    [InlineData(false, "Development")]
    [InlineData(null, "Development")]
    [InlineData(true, "Production")]
    [InlineData(true, "Staging")]
    public async Task ShouldRejectDirectDemoAccessUnlessExplicitlyEnabledInDevelopment(bool? enabled, string environment)
    {
        using var factory = new DemoApplicationFactory(enabled, environment);
        using var client = factory.CreateBrowserClient();
        var entry = await client.GetStringAsync("/Login");
        Assert.Contains("name=\"Password\"", entry);
        Assert.DoesNotContain("id=\"demoLoginButton\"", entry);
        var response = await PostEntry(client, entry, "/Login/Demo");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.False(response.Headers.Contains("Set-Cookie"));
    }

    [Fact]
    public async Task ShouldRejectMissingOrInvalidAntiforgeryTokensAndNeverSignInOnGet()
    {
        using var factory = new DemoApplicationFactory();
        using var client = factory.CreateBrowserClient();
        await client.GetAsync("/Login");
        foreach (var fields in new[] { new Dictionary<string, string>(), new() { ["__RequestVerificationToken"] = "invalid" } })
        {
            var response = await client.PostAsync("/Login/Demo", new FormUrlEncodedContent(fields));
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.False(response.Headers.Contains("Set-Cookie"));
        }

        var get = await client.GetAsync("/Login/Demo");
        Assert.False(get.Headers.Contains("Set-Cookie"));
        Assert.NotEqual("/Home", get.Headers.Location?.OriginalString);
        Assert.Equal(HttpStatusCode.Redirect, (await client.GetAsync("/Home")).StatusCode);
    }

    [Fact]
    public async Task ShouldKeepApplicationModulesProtectedAndPublicPrintEndpointsAnonymous()
    {
        using var factory = new DemoApplicationFactory();
        using var client = factory.CreateBrowserClient();
        foreach (var path in new[] { "/Home", "/Person", "/Item", "/Proposal", "/Catalog" })
        {
            var response = await client.GetAsync(path);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.StartsWith("https://localhost/Login?", response.Headers.Location?.OriginalString);
        }

        var endpoints = factory.Services.GetRequiredService<EndpointDataSource>().Endpoints;
        foreach (var action in new[] { "Print", "ExportToPDF" })
            Assert.Contains(endpoints, endpoint =>
                endpoint.Metadata.GetMetadata<ControllerActionDescriptor>() is { ControllerName: "Proposal" } descriptor
                && descriptor.ActionName == action && endpoint.Metadata.GetMetadata<IAllowAnonymous>() != null);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task ShouldInvalidateExistingDemoSessionWhenFlagOrEnvironmentChanges(bool changeEnvironment)
    {
        using var factory = new DemoApplicationFactory();
        using var client = factory.CreateBrowserClient();
        await EnterDemo(client);
        if (changeEnvironment)
            factory.Services.GetRequiredService<IHostEnvironment>().EnvironmentName = "Production";
        else
            factory.SetDemoEnabled(false);

        var response = await client.GetAsync("/Home");
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.StartsWith("https://localhost/Login?", response.Headers.Location?.OriginalString);
    }

    [Fact]
    public async Task ShouldExpireDemoSessionAndAllowAnotherEntryAfterLogout()
    {
        using var factory = new DemoApplicationFactory();
        using var client = factory.CreateBrowserClient();
        await EnterDemo(client);
        factory.Clock.Advance(TimeSpan.FromHours(5));
        var activeSession = await client.GetAsync("/Home");
        Assert.Equal(HttpStatusCode.OK, activeSession.StatusCode);
        Assert.False(activeSession.Headers.Contains("Set-Cookie"));
        factory.Clock.Advance(TimeSpan.FromHours(4));
        Assert.Equal(HttpStatusCode.Redirect, (await client.GetAsync("/Home")).StatusCode);
        await EnterDemo(client);
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/Home")).StatusCode);
        Assert.Equal(HttpStatusCode.Redirect, (await client.GetAsync("/Login/Logout")).StatusCode);
        Assert.Equal(HttpStatusCode.Redirect, (await client.GetAsync("/Home")).StatusCode);
        await EnterDemo(client);
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/Home")).StatusCode);
    }

    [Fact]
    public async Task ShouldBlockAccountRegistrationAndCompanyMutationsOnDirectRequests()
    {
        using var factory = new DemoApplicationFactory();
        using var client = factory.CreateBrowserClient();
        Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync("/Login/Register")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound,
            (await client.PostAsync("/Login/Register", new FormUrlEncodedContent(new Dictionary<string, string>()))).StatusCode);
        await EnterDemo(client);
        Assert.Equal(HttpStatusCode.Forbidden, (await client.GetAsync("/Company/Create")).StatusCode);
        foreach (var path in new[] { "/Company/Create", "/Company/Update" })
            Assert.Equal(HttpStatusCode.Forbidden,
                (await client.PostAsync(path, new FormUrlEncodedContent(new Dictionary<string, string>()))).StatusCode);
    }

    [Fact]
    public async Task ShouldReturnRecoverableErrorWithoutCookieOrInternalException()
    {
        using var factory = new DemoApplicationFactory(failSignIn: true);
        using var client = factory.CreateBrowserClient();
        var response = await PostEntry(client, await client.GetStringAsync("/Login"), "/Login/Demo");
        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        Assert.False(response.Headers.Contains("Set-Cookie"));
        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("id=\"demoError\"", html);
        Assert.Contains("role=\"alert\"", html);
        Assert.Contains("id=\"demoLoginButton\"", html);
        Assert.DoesNotContain("Simulated internal sign-in failure", html);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task ShouldKeepConventionalCredentialValidationAndCookiesWhenDemoIsDisabled(bool wrongPassword)
    {
        using var factory = new DemoApplicationFactory(enabled: false);
        using var client = factory.CreateBrowserClient();
        var response = await PostEntry(client, await client.GetStringAsync("/Login"), "/Login/Login", new()
        {
            ["Username"] = Constants.DefaultUsername,
            ["Password"] = DemoApplicationFactory.TestPassword + (wrongPassword ? "wrong" : "")
        });
        if (wrongPassword)
        {
            Assert.False(response.Headers.Contains("Set-Cookie"));
            Assert.Contains(Messages.InvalidUsernameOrPassword,
                WebUtility.HtmlDecode(await response.Content.ReadAsStringAsync()));
            Assert.Equal(HttpStatusCode.Redirect, (await client.GetAsync("/Home")).StatusCode);
        }
        else
        {
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            factory.SetDemoEnabled(true);
            Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/Home")).StatusCode);
            factory.SetDemoEnabled(false);
            Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/Home")).StatusCode);
            Assert.DoesNotContain("id=\"demoSessionNotice\"", await client.GetStringAsync("/Home"));
        }
    }

    [Fact]
    public async Task ShouldRejectUnknownUserOutsideDemo()
    {
        using var factory = new DemoApplicationFactory(enabled: false);
        using var client = factory.CreateBrowserClient();
        var response = await PostEntry(client, await client.GetStringAsync("/Login"), "/Login/Login", new()
        {
            ["Username"] = "unknown-user",
            ["Password"] = DemoApplicationFactory.TestPassword
        });
        Assert.False(response.Headers.Contains("Set-Cookie"));
        Assert.Contains(Messages.InvalidUsernameOrPassword,
            WebUtility.HtmlDecode(await response.Content.ReadAsStringAsync()));
        Assert.Equal(HttpStatusCode.Redirect, (await client.GetAsync("/Home")).StatusCode);
    }

    private static async Task EnterDemo(HttpClient client)
    {
        var response = await PostEntry(client, await client.GetStringAsync("/Login"), "/Login/Demo");
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
    }

    private static Task<HttpResponseMessage> PostEntry(HttpClient client, string html, string path,
        Dictionary<string, string> fields = null)
    {
        fields ??= new Dictionary<string, string>();
        var input = Regex.Match(html, "<input[^>]+name=\"__RequestVerificationToken\"[^>]*>").Value;
        fields["__RequestVerificationToken"] = WebUtility.HtmlDecode(Regex.Match(input, "value=\"([^\"]+)\"").Groups[1].Value);
        Assert.NotEmpty(fields["__RequestVerificationToken"]);
        return client.PostAsync(path, new FormUrlEncodedContent(fields));
    }
}
