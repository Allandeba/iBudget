using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace iBudget.DAO.Extensions;

public static class DatabaseExtensions
{
    public static async Task<List<string>> GetTablesAsync(this DatabaseFacade database)
    {
        var tables = new List<string>();
        try
        {
            var connection = database.GetDbConnection();

            if (connection.State != System.Data.ConnectionState.Open)
                await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText =
                "SELECT TABLE_NAME "
                + "  FROM INFORMATION_SCHEMA.TABLES "
                + " WHERE TABLE_TYPE = 'BASE TABLE' ";

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                    tables.Add(reader.GetString(0));
            }

            return tables;
        }
        catch
        {
            throw;
        }
    }
}
