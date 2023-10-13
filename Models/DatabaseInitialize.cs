using iBudget.DAO;
using iBudget.Framework;
using Microsoft.EntityFrameworkCore;

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

                LoginModel? login = context.Login.FirstOrDefault();
                if (login == null)
                {
                    Cryptography cryptography = new();
                    context.Login.AddRange(
                        new LoginModel
                        {
                            Username = "admin",
                            Password = cryptography.GetHash(
                                Environment.GetEnvironmentVariable("USER_PASSWORD")
                            )
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
