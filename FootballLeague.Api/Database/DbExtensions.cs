using Microsoft.EntityFrameworkCore;

namespace FootballLeague.Api.Database;

public static class DbExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        using FootballDbContext dbContext = scope.ServiceProvider.GetRequiredService<FootballDbContext>();

        try
        {
            await dbContext.Database.MigrateAsync();
            Console.WriteLine("Migrations were applied successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while applying migrations: {ex.Message}");
            throw;
        }
    }
}
