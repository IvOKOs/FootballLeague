using FootballLeague.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace FootballLeague.Api.Database;

public class FootballDbContext : DbContext, IFootballDbContext
{
    public FootballDbContext(DbContextOptions<FootballDbContext> options) : base(options) { }

    public DbSet<Team> Teams { get; set; }
    public DbSet<Match> Matches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasOne(m => m.HomeTeam)
                  .WithMany(t => t.HomeMatches)
                  .HasForeignKey(m => m.HomeTeamId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(m => m.AwayTeam)
                  .WithMany(t => t.AwayMatches)
                  .HasForeignKey(m => m.AwayTeamId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.Property(t => t.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.HasIndex(t => new { t.Name }).IsUnique();
        });

        modelBuilder.Entity<Team>().HasData(
            new Team { Id = 1, Name = "Manchester City", CreatedAtUtc = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Team { Id = 2, Name = "Arsenal", CreatedAtUtc = new DateTime(2026, 1, 2, 0, 0, 0, DateTimeKind.Utc) },
            new Team { Id = 3, Name = "Liverpool", CreatedAtUtc = new DateTime(2026, 1, 3, 0, 0, 0, DateTimeKind.Utc) },
            new Team { Id = 4, Name = "Chelsea", CreatedAtUtc = new DateTime(2026, 1, 4, 0, 0, 0, DateTimeKind.Utc) },
            new Team { Id = 5, Name = "Barcelona", CreatedAtUtc = new DateTime(2026, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
        );
    }

    void IFootballDbContext.Remove<T>(T entity)
    {
        base.Remove(entity);
    }
}
