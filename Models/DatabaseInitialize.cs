using iBudget.DAO;
using iBudget.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace iBudget.Models
{
    public static class DatabaseInitialize
    {
        public static WebApplication InitializeDB(this WebApplication app)
        {
            using IServiceScope scope = app.Services.CreateScope();
            using ApplicationDBContext context =
                scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
            try
            {
                context.Database.Migrate();
                _ = context.Database.EnsureCreated();

                LoginModel login = context.Login.FirstOrDefault();
                if (login == null)
                {
                    Cryptography cryptography = new();

                    string password = "";
                    if (app.Environment.IsProduction())
                        password = Environment.GetEnvironmentVariable("USER_PASSWORD");
                    else
                        password = app.Configuration.GetConnectionString("USER_PASSWORD");

                    if (password.IsNullOrEmpty())
                        throw new Exception(
                            "Não foi possível encontrar variaveis de sistema para DatabaseInitialize"
                        );

                    context.Login.AddRange(
                        new LoginModel
                        {
                            Username = "admin",
                            Password = cryptography.GetHash(password)
                        }
                    );

                    _ = context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return app;
        }
    }
}
