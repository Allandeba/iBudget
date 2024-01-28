using iBudget.Business;
using iBudget.Controllers;
using iBudget.Models;
using iBudget.DAO;
using iBudget.DAO.Entities;
using iBudget.Repository;
using iBudget.Framework;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
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

        _ = builder.Services.AddControllersWithViews(config =>
        {
            config.Filters.Add(typeof(CustomExceptionFilter));
        });

        builder.Services
            .AddControllers()
            .AddJsonOptions(
                x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
            );

        builder.Services
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

        string syncfusionKey;
        string connectionString;

        if (builder.Environment.IsDevelopment())
        {
            syncfusionKey =
                builder.Configuration.GetConnectionString("SYNC_FUSION_LICENSING");
            if (string.IsNullOrEmpty(syncfusionKey))
                syncfusionKey = Environment.GetEnvironmentVariable("SYNC_FUSION_LICENSING");
            connectionString = builder.Configuration.GetConnectionString("DB_CONNECTION");
            SystemManager.IsDevelopment = true;
        }
        else
        {
            syncfusionKey = Environment.GetEnvironmentVariable("SYNC_FUSION_LICENSING");
            if (string.IsNullOrEmpty(syncfusionKey))
                throw new Exception("Chave Syncfusion não encontrada para produção");

            connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");
            if (string.IsNullOrEmpty(connectionString))
                throw new Exception("Configuração de banco de dados não encontrada para produção");
        }

        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionKey);

        _ = builder.Services.AddDbContext<ApplicationDBContext>(
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
        _ = builder.Services.AddScoped<DatabaseInitialize>();

        if (SystemManager.IsDevelopment)
            _ = builder.Services.AddHostedService<DatabaseResetWorker>();

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

        using (var scope = app.Services.CreateScope())
        {
            var databaseInitialize = scope.ServiceProvider.GetRequiredService<DatabaseInitialize>();
            databaseInitialize.InitializeDB().Wait();
        }

        app.Run();
    }
}
