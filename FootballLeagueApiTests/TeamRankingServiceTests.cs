using FootballLeague.Api.Database;
using FootballLeague.Api.Database.Entities;
using FootballLeague.Api.Services.TeamRankings;
using FootballLeague.Api.Services.TeamRankings.PointsCalculation;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FootballLeagueApiTests;

[TestFixture]
public class TeamRankingServiceTests
{
    private FootballDbContext _dbContext = null!;
    private TeamRankingService _teamRankingService = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<FootballDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new FootballDbContext(options);

        var pointsStrategy = new StandardPointsStrategy();

        _teamRankingService = new TeamRankingService(
            _dbContext,
            pointsStrategy);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }

    [Test]
    public async Task GetTeamRankingsAsync_ShouldReturnEmptyList_IfNoMatchesExist()
    {
        var result = await _teamRankingService.GetTeamRankingsAsync(CancellationToken.None);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task GetTeamRankingsAsync_ShouldCalculateRankingCorrectly()
    {
        var arsenal = new Team { Name = "Arsenal" };
        var chelsea = new Team { Name = "Chelsea" };

        _dbContext.Teams.AddRange(arsenal, chelsea);
        await _dbContext.SaveChangesAsync();

        var match = new Match
        {
            HomeTeamId = arsenal.Id,
            AwayTeamId = chelsea.Id,
            HomeScore = 2,
            AwayScore = 1,
            PlayedAtUtc = DateTime.UtcNow,
            CreatedAtUtc = DateTime.UtcNow
        };

        _dbContext.Matches.Add(match);
        await _dbContext.SaveChangesAsync();

        var result = await _teamRankingService.GetTeamRankingsAsync(CancellationToken.None);

        var arsenalRanking = result.First(r => r.TeamId == arsenal.Id);
        var chelseaRanking = result.First(r => r.TeamId == chelsea.Id);

        Assert.That(arsenalRanking.Wins, Is.EqualTo(1));
        Assert.That(arsenalRanking.Points, Is.EqualTo(3));
        Assert.That(arsenalRanking.GoalsFor, Is.EqualTo(2));
        Assert.That(arsenalRanking.GoalsAgainst, Is.EqualTo(1));

        Assert.That(chelseaRanking.Losses, Is.EqualTo(1));
        Assert.That(chelseaRanking.Points, Is.EqualTo(0));
    }

    [Test]
    public async Task GetTeamRankingsAsync_ShouldCalculateDrawCorrectly()
    {
        var arsenal = new Team { Name = "Arsenal" };
        var chelsea = new Team { Name = "Chelsea" };

        _dbContext.Teams.AddRange(arsenal, chelsea);
        await _dbContext.SaveChangesAsync();

        var match = new Match
        {
            HomeTeamId = arsenal.Id,
            AwayTeamId = chelsea.Id,
            HomeScore = 1,
            AwayScore = 1,
            PlayedAtUtc = DateTime.UtcNow,
            CreatedAtUtc = DateTime.UtcNow
        };

        _dbContext.Matches.Add(match);
        await _dbContext.SaveChangesAsync();

        var result = await _teamRankingService.GetTeamRankingsAsync(CancellationToken.None);

        var arsenalRanking = result.First(r => r.TeamId == arsenal.Id);
        var chelseaRanking = result.First(r => r.TeamId == chelsea.Id);

        Assert.That(arsenalRanking.Draws, Is.EqualTo(1));
        Assert.That(arsenalRanking.Points, Is.EqualTo(1));

        Assert.That(chelseaRanking.Draws, Is.EqualTo(1));
        Assert.That(chelseaRanking.Points, Is.EqualTo(1));
    }

    [Test]
    public async Task GetTeamRankingsAsync_ShouldOrderTeamsByPoints()
    {
        var arsenal = new Team { Name = "Arsenal" };
        var chelsea = new Team { Name = "Chelsea" };
        var liverpool = new Team { Name = "Liverpool" };

        _dbContext.Teams.AddRange(arsenal, chelsea, liverpool);
        await _dbContext.SaveChangesAsync();

        var matches = new List<Match>
        {
            new Match
            {
                HomeTeamId = arsenal.Id,
                AwayTeamId = chelsea.Id,
                HomeScore = 2,
                AwayScore = 0,
                PlayedAtUtc = DateTime.UtcNow,
                CreatedAtUtc = DateTime.UtcNow
            },
            new Match
            {
                HomeTeamId = liverpool.Id,
                AwayTeamId = arsenal.Id,
                HomeScore = 1,
                AwayScore = 1,
                PlayedAtUtc = DateTime.UtcNow,
                CreatedAtUtc = DateTime.UtcNow
            }
        };

        _dbContext.Matches.AddRange(matches);
        await _dbContext.SaveChangesAsync();

        var result = await _teamRankingService.GetTeamRankingsAsync(CancellationToken.None);

        Assert.That(result[0].TeamId, Is.EqualTo(arsenal.Id));
    }

    [Test]
    public async Task GetTeamRankingsAsync_ShouldOrderByGoalDifference_IfPointsAreEqual()
    {
        var arsenal = new Team { Name = "Arsenal" };
        var chelsea = new Team { Name = "Chelsea" };
        var liverpool = new Team { Name = "Liverpool" };

        _dbContext.Teams.AddRange(arsenal, chelsea, liverpool);
        await _dbContext.SaveChangesAsync();

        var matches = new List<Match>
        {
            new Match
            {
                HomeTeamId = arsenal.Id,
                AwayTeamId = chelsea.Id,
                HomeScore = 3,
                AwayScore = 0,
                PlayedAtUtc = DateTime.UtcNow,
                CreatedAtUtc = DateTime.UtcNow
            },
            new Match
            {
                HomeTeamId = liverpool.Id,
                AwayTeamId = chelsea.Id,
                HomeScore = 1,
                AwayScore = 0,
                PlayedAtUtc = DateTime.UtcNow,
                CreatedAtUtc = DateTime.UtcNow
            }
        };

        _dbContext.Matches.AddRange(matches);
        await _dbContext.SaveChangesAsync();

        var result = await _teamRankingService.GetTeamRankingsAsync(CancellationToken.None);

        Assert.That(result[0].GoalDifference, Is.GreaterThan(result[1].GoalDifference));
    }
}
