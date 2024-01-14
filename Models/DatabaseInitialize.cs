using iBudget.DAO;
using iBudget.Framework;
using iBudget.Models.FakeModels;
using iBudget.Models.FakeModels.Helpers;
using Microsoft.EntityFrameworkCore;

namespace iBudget.Models;

public class DatabaseInitialize
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDBContext _context;
    private readonly IHostEnvironment _env;
    private readonly ILogger<DatabaseInitialize> _logger;

    public DatabaseInitialize(
        IConfiguration configuration,
        ApplicationDBContext context,
        IHostEnvironment env,
        ILogger<DatabaseInitialize> logger
    )
    {
        _configuration = configuration;
        _context = context;
        _env = env;
        _logger = logger;
    }

    public async Task InitializeDB()
    {
        try
        {
            await _context.Database.MigrateAsync();

            if (!await _context.Database.CanConnectAsync())
            {
                _logger.LogError("Não foi possível conectar ao banco de dados.");
                return;
            }

            if (!await _context.Login.AnyAsync())
                await CreateAdminLogin();

            if (_env.IsDevelopment())
                await CreateFakeData();

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante a inicialização do banco de dados");
            throw;
        }
    }

    private async Task CreateAdminLogin()
    {
        Cryptography cryptography = new Cryptography();

        var password = _env.IsProduction()
            ? Environment.GetEnvironmentVariable("USER_PASSWORD")
            : _configuration.GetConnectionString("USER_PASSWORD");

        if (string.IsNullOrEmpty(password))
            throw new Exception(
                "Não foi possível encontrar variáveis de sistema para DatabaseInitialize"
            );

        await _context.Login.AddAsync(
            new LoginModel { Username = "admin", Password = cryptography.GetHash(password) }
        );
    }

    private async Task CreateFakeData()
    {
        await CreateFakeCompany();
        await CreateFakePersons();
        await CreateFakeItems();
        await CreateFakeProposals();
    }

    private async Task CreateFakeCompany()
    {
        if (!_context.Company.Any())
        {
            CompanyModel company = new CompanyModel
            {
                CompanyName = "Allan Debastiani",
                CNPJ = "00.000.000.0000/00",
                Address = "Centro, Chapecó - SC, 89801-230",
                Email = "allandeba@yahoo.com.br",
                Phone = "5549988494737",
                ImageFile = await FakerHelper.GetRandomImage()
            };

            await _context.Company.AddAsync(company);
        }
    }

    private async Task CreateFakePersons()
    {
        if (!await _context.Person.AnyAsync())
        {
            var personFakeList = PersonFakeModel.GetPersonFakeModelList(
                Constants.QtInitialFakeRegisters
            );
            await _context.Person.AddRangeAsync(personFakeList);
        }
    }

    private async Task CreateFakeItems()
    {
        if (!await _context.Item.AnyAsync())
        {
            var itemFakeList = await ItemFakeModel.GetItemFakeModelList(
                Constants.QtInitialFakeRegisters
            );
            await _context.Item.AddRangeAsync(itemFakeList);
        }
    }

    private async Task CreateFakeProposals()
    {
        if (!await _context.Proposal.AnyAsync())
        {
            var proposalFakeList = ProposalFakeModel.GetProposalFakeModelList(
                Constants.QtInitialFakeRegisters
            );
            await _context.Proposal.AddRangeAsync(proposalFakeList);
        }
    }
}
