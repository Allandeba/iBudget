using iBudget.Business;
using iBudget.Controllers;
using iBudget.DAO;
using iBudget.Models;
using iBudget.Repository;
using iBudget.Framework;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.HttpOverrides;

namespace iBudget;

public class Program
{
    public static void Main(string[] args)
    {
        _ = DotNetEnv.Env.Load();

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });

        // Add services to the container.
        _ = builder.Services.AddControllersWithViews(
            config => config.Filters.Add(typeof(CustomExceptionFilter))
        );
        _ = builder.Services
            .AddControllers()
            .AddJsonOptions(
                x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
            );
        _ = builder.Services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/" + nameof(LoginController.Login);
                options.ExpireTimeSpan = TimeSpan.FromHours(8);
                options.Cookie.Name = "authCookie";
            });
        _ = builder.Services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        string syncfusionKey = "";
        string connectionString = "";
        var environment = builder.Configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT");
        switch (environment)
        {
            case Constants.EnvironmentDevelopment:
                syncfusionKey = builder.Configuration.GetConnectionString("SYNC_FUSION_LICENSING");
                connectionString = builder.Configuration.GetConnectionString("DB_CONNECTION");
                SystemManager.IsDevelopment = true;
                break;
                
            case Constants.EnvironmentProduction:
                syncfusionKey = Environment.GetEnvironmentVariable("SYNC_FUSION_LICENSING");
                if (syncfusionKey.IsNullOrEmpty())
                {
                    throw new Exception(
                        "Chave Syncfusion não encontrada - " + environment.ToString()
                    );
                }
                connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");
                if (connectionString.IsNullOrEmpty())
                {
                    throw new Exception(
                        "Configuração de banco de dados não encontrada - " + environment.ToString()
                    );
                }
                break;

            default:
                throw new Exception("Environment não implementado para inicializar o sistema");
        }

        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionKey);
        builder.Services.AddDbContext<ApplicationDBContext>(
            options => options.UseNpgsql(connectionString)
        );
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        _ = builder.Services.AddScoped<CatalogRepository>();
        _ = builder.Services.AddScoped<CatalogBusiness>();
        _ = builder.Services.AddScoped<ItemRepository>();
        _ = builder.Services.AddScoped<ItemBusiness>();
        _ = builder.Services.AddScoped<PersonRepository>();
        _ = builder.Services.AddScoped<PersonBusiness>();
        _ = builder.Services.AddScoped<ProposalRepository>();
        _ = builder.Services.AddScoped<ProposalBusiness>();
        _ = builder.Services.AddScoped<ProposalHistoryRepository>();
        _ = builder.Services.AddScoped<ProposalHistoryBusiness>();
        _ = builder.Services.AddScoped<CompanyBusiness>();
        _ = builder.Services.AddScoped<CompanyRepository>();
        _ = builder.Services.AddScoped<LoginBusiness>();
        _ = builder.Services.AddScoped<LoginRepository>();

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            _ = app.UseHsts();
        }

        app.UseForwardedHeaders();
        _ = app.UseHttpsRedirection();
        _ = app.UseStaticFiles();

        _ = app.UseRouting();

        _ = app.UseAuthentication();
        _ = app.UseAuthorization();

        _ = app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}"
        );

        _ = app.InitializeDB();

        app.Run();
    }
}
