using iBudget.DAO;
using iBudget.DAO.Extensions;
using Microsoft.EntityFrameworkCore;

namespace iBudget.DAO.Entities;

public class DatabaseResetWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseResetWorker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromHours(8), stoppingToken);

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
                    await DropTables(context);

                    var databaseInitialize =
                        scope.ServiceProvider.GetRequiredService<DatabaseInitialize>();
                    await databaseInitialize.InitializeDB();
                }
            }
            catch
            {
                throw;
            }
        }
    }

    private async Task DropTables(ApplicationDBContext context)
    {
        var tables = await context.Database.GetTablesAsync();
        try
        {
            const string prefixPostgreTableName = "pg_";
            const string prefixGeneratedTables = "sql_";
            var appTables = tables
                .Where(
                    table =>
                        !table.Contains(prefixPostgreTableName)
                        && !table.Contains(prefixGeneratedTables)
                )
                .ToList();

            var truncateCommands = appTables
                .Select(table => $"DROP TABLE \"{table}\" CASCADE")
                .ToList();
            var sql = string.Join("; ", truncateCommands);

            await context.Database.ExecuteSqlRawAsync(sql);
        }
        catch
        {
            throw;
        }
    }
}
