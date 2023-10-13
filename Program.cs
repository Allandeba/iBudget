﻿using iBudget.Business;
using iBudget.Controllers;
using iBudget.DAO;
using iBudget.Models;
using iBudget.Repository;
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
        _ = builder.Services.AddControllersWithViews();
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

        // Add SyncfusionKey
        string? syncfusionKey = Environment.GetEnvironmentVariable("SYNC_FUSION_LICENSING");
        if (syncfusionKey.IsNullOrEmpty())
        {
            syncfusionKey = builder.Configuration.GetConnectionString("SYNC_FUSION_LICENSING");
        }
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionKey);

        // Add PostgreSQL connection.
        string? connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");
        if (connectionString.IsNullOrEmpty())
        {
            connectionString = builder.Configuration.GetConnectionString("DB_CONNECTION");
        }
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

        WebApplication app = builder.Build();

        app.UseForwardedHeaders();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            _ = app.UseExceptionHandler(CustomErrorViewModel =>
            {
                CustomErrorViewModel.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        await context.Response.WriteAsync(
                            new CustomErrorViewModel()
                            {
                                Code = 500,
                                ErrorMessage = error.Error.Message
                            }.ToString(),
                            Encoding.UTF8
                        );
                    }
                });
            });
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            _ = app.UseHsts();
        }

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
