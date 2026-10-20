using System.Security.Claims;
using iBudget.Controllers;
using iBudget.DAO;
using iBudget.DAO.Entities;
using iBudget.Framework;
using iBudget.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared;

namespace iBudget.UnitTest.Authentication;

public sealed class DemoApplicationFactory : WebApplicationFactory<Program>
{
    public const string TestPassword = "Isolated-test-only!42";
    private readonly string _environment;
    private readonly bool? _enabled;
    private readonly bool _failSignIn;
    private readonly InMemoryDatabaseRoot _databaseRoot = new();
    private readonly string _databaseName = Guid.NewGuid().ToString();

    public DemoApplicationFactory(bool? enabled = true, string environment = "Development", bool failSignIn = false)
    {
        _enabled = enabled;
        _environment = environment;
        _failSignIn = failSignIn;
    }

    public DemoTestClock Clock { get; } = new();

    protected override IHost CreateHost(IHostBuilder builder)
    {
        // Program reads these settings before Build; host settings reach its entry-point arguments.
        builder.ConfigureHostConfiguration(configuration => configuration.AddInMemoryCollection(
            new Dictionary<string, string>
            {
                ["ConnectionStrings:DB_CONNECTION"] = "Host=unused;Database=isolated_tests"
            }));
        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(_environment);
        builder.ConfigureLogging(logging => logging.ClearProviders());
        builder.ConfigureAppConfiguration((_, configuration) => configuration.AddInMemoryCollection(
            new Dictionary<string, string>
            {
                ["ConnectionStrings:DB_CONNECTION"] = "Host=unused;Database=isolated_tests",
                ["Demo:Enabled"] = _enabled?.ToString()
            }));

        builder.ConfigureTestServices(services =>
        {
            foreach (var worker in services.Where(service => service.ServiceType == typeof(IHostedService)
                         && service.ImplementationType == typeof(DatabaseResetWorker)).ToArray())
                services.Remove(worker);

            services.RemoveAll<ApplicationDBContext>();
            services.AddScoped(_ => new ApplicationDBContext(new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(_databaseName, _databaseRoot).Options));

            services.RemoveAll<DatabaseInitialize>();
            services.AddScoped<DatabaseInitialize, TestDatabaseInitialize>();
            services.AddDataProtection().UseEphemeralDataProtectionProvider();
            services.RemoveAll<TimeProvider>();
            services.AddSingleton<TimeProvider>(Clock);
            services.PostConfigure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme,
                options => options.TimeProvider = Clock);

            if (_failSignIn)
            {
                services.RemoveAll<IAuthenticationService>();
                services.AddScoped<IAuthenticationService>(provider => new FailingSignInService(
                    ActivatorUtilities.CreateInstance<AuthenticationService>(provider)));
            }
        });
    }

    public HttpClient CreateBrowserClient() => CreateClient(new WebApplicationFactoryClientOptions
    {
        AllowAutoRedirect = false,
        BaseAddress = new Uri("https://localhost")
    });

    public void SetDemoEnabled(bool enabled)
    {
        var configuration = (IConfigurationRoot)Services.GetRequiredService<IConfiguration>();
        configuration["Demo:Enabled"] = enabled.ToString();
        configuration.Reload();
    }

    private sealed class TestDatabaseInitialize : DatabaseInitialize
    {
        private readonly ApplicationDBContext _context;

        public TestDatabaseInitialize(IConfiguration configuration, ApplicationDBContext context,
            IHostEnvironment environment, ILogger<DatabaseInitialize> logger)
            : base(configuration, context, environment, logger) => _context = context;

        public override async Task InitializeDB()
        {
            await _context.Database.EnsureCreatedAsync();
            _context.Login.Add(new LoginModel
            {
                Username = Constants.DefaultUsername,
                Password = new Cryptography().GetHash(TestPassword)
            });
            await _context.SaveChangesAsync();
        }
    }

    private sealed class FailingSignInService : IAuthenticationService
    {
        private readonly IAuthenticationService _inner;

        public FailingSignInService(IAuthenticationService inner) => _inner = inner;

        public Task<AuthenticateResult> AuthenticateAsync(Microsoft.AspNetCore.Http.HttpContext context, string scheme)
            => _inner.AuthenticateAsync(context, scheme);

        public Task ChallengeAsync(Microsoft.AspNetCore.Http.HttpContext context, string scheme, AuthenticationProperties properties)
            => _inner.ChallengeAsync(context, scheme, properties);

        public Task ForbidAsync(Microsoft.AspNetCore.Http.HttpContext context, string scheme, AuthenticationProperties properties)
            => _inner.ForbidAsync(context, scheme, properties);

        public Task SignOutAsync(Microsoft.AspNetCore.Http.HttpContext context, string scheme, AuthenticationProperties properties)
            => _inner.SignOutAsync(context, scheme, properties);

        public Task SignInAsync(Microsoft.AspNetCore.Http.HttpContext context, string scheme, ClaimsPrincipal principal,
            AuthenticationProperties properties)
        {
            context.Response.Headers.Append("Set-Cookie", "authCookie=partial-failed-session");
            throw new InvalidOperationException("Simulated internal sign-in failure");
        }
    }
}

public sealed class DemoTestClock : TimeProvider
{
    private DateTimeOffset _now = DateTimeOffset.UtcNow;
    public override DateTimeOffset GetUtcNow() => _now;
    public void Advance(TimeSpan duration) => _now += duration;
}
