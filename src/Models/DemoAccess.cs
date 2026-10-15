using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace iBudget.Models;

public class DemoAccess
{
    public const string ClaimType = "demo";

    private readonly IHostEnvironment _environment;
    private readonly IOptionsMonitor<DemoOptions> _options;

    public DemoAccess(IHostEnvironment environment, IOptionsMonitor<DemoOptions> options)
    {
        _environment = environment;
        _options = options;
    }

    public bool IsEnabled => _environment.IsDevelopment() && _options.CurrentValue.Enabled;

    public static bool IsDemo(ClaimsPrincipal user) => user.HasClaim(ClaimType, "true");
}
