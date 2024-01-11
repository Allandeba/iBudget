using Bogus;
using iBudget.DAO;
using iBudget.Framework;
using iBudget.Models.FakeModels;
using iBudget.Models.FakeModels.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace iBudget.Models
{
    public static class DatabaseInitialize
    {
        public static async Task<WebApplication> InitializeDB(this WebApplication app)
        {
            using IServiceScope scope = app.Services.CreateScope();
            using ApplicationDBContext context =
                scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
            try
            {
                await context.Database.MigrateAsync();
                _ = await context.Database.EnsureCreatedAsync();

                if (!await context.Login.AnyAsync())
                    await CreateAdminLogin(context, app);

                if (app.Environment.IsDevelopment())
                    await CreateFakeData(context);

                _ = await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return app;
        }

        private static async Task CreateAdminLogin(ApplicationDBContext context, WebApplication app)
        {
            Cryptography cryptography = new();

            string password;
            if (app.Environment.IsProduction())
                password = Environment.GetEnvironmentVariable("USER_PASSWORD");
            else
                password = app.Configuration.GetConnectionString("USER_PASSWORD");

            if (password.IsNullOrEmpty())
                throw new Exception(
                    "Não foi possível encontrar variaveis de sistema para DatabaseInitialize"
                );

            await context.Login.AddRangeAsync(
                new LoginModel { Username = "admin", Password = cryptography.GetHash(password) }
            );
        }

        private static async Task CreateFakeData(ApplicationDBContext context)
        {
            if (!context.Company.Any())
            {
                // var companyFakeList = await CompanyFakeModel.GetCompanyFakeModelList(1);
                // await context.Company.AddRangeAsync(companyFakeList);
                CompanyModel company =
                    new()
                    {
                        CompanyName = "Allan Debastiani",
                        CNPJ = "00.000.000.0000/00",
                        Address = "Centro, Chapecó - SC, 89801-230",
                        Email = "allandeba@yahoo.com.br",
                        Phone = "5549988494737",
                        ImageFile = await FakerHelper.GetRandomImage()
                    };
                await context.Company.AddAsync(company);
            }

            if (!await context.Person.AnyAsync())
            {
                var personFakeList = PersonFakeModel.GetPersonFakeModelList(10);
                await context.Person.AddRangeAsync(personFakeList);
            }

            if (!await context.Item.AnyAsync())
            {
                var itemFakeList = await ItemFakeModel.GetItemFakeModelList(10);
                await context.Item.AddRangeAsync(itemFakeList);
            }

            if (!await context.Proposal.AnyAsync())
            {
                var proposalFakeList = ProposalFakeModel.GetProposalFakeModelList(10);
                await context.Proposal.AddRangeAsync(proposalFakeList);
            }
        }
    }
}
