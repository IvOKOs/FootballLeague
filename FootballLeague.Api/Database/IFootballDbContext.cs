using FootballLeague.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace FootballLeague.Api.Database;

public interface IFootballDbContext
{
    DbSet<Team> Teams { get; }
    DbSet<Match> Matches { get; }
    void Remove<T>(T entity) where T : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
