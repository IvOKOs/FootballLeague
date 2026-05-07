using FootballLeague.Api.Database;
using FootballLeague.Api.Database.Entities;
using FootballLeague.Api.DTOs.Matches;
using FootballLeague.Api.Exceptions;
using FootballLeague.Api.Services.Matches;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FootballLeagueApiTests;

[TestFixture]
public class MatchServiceTests
{
    private FootballDbContext _dbContext = null!;
    private MatchService _matchService = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<FootballDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new FootballDbContext(options);
        _matchService = new MatchService(_dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }

    [Test]
    public async Task GetMatchByIdAsync_ShouldReturnMatchDto_IfMatchWithGivenIdExists()
    {
        var homeTeam = new Team { Name = "Arsenal" };
        var awayTeam = new Team { Name = "Chelsea" };

        _dbContext.Teams.AddRange(homeTeam, awayTeam);
        await _dbContext.SaveChangesAsync();

        var match = new Match
        {
            HomeTeamId = homeTeam.Id,
            AwayTeamId = awayTeam.Id,
            HomeScore = 2,
            AwayScore = 1,
            PlayedAtUtc = DateTime.UtcNow,
            CreatedAtUtc = DateTime.UtcNow
        };

        _dbContext.Matches.Add(match);
        await _dbContext.SaveChangesAsync();

        var result = await _matchService.GetMatchByIdAsync(match.Id, CancellationToken.None);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(match.Id));
    }

    [Test]
    public void GetMatchByIdAsync_ShouldThrowNotFoundException_IfMatchWithGivenIdIsNotFound()
    {
        Assert.That(
            (Func<Task>)(async () => await _matchService.GetMatchByIdAsync(999, CancellationToken.None)),
            Throws.TypeOf<NotFoundException>());
    }

    [Test]
    public async Task CreateMatchAsync_ShouldCreateMatch_IfCreateMatchDtoIsValid()
    {
        var homeTeam = new Team { Name = "Arsenal" };
        var awayTeam = new Team { Name = "Chelsea" };

        _dbContext.Teams.AddRange(homeTeam, awayTeam);
        await _dbContext.SaveChangesAsync();

        var dto = new CreateMatchDto
        {
            HomeTeamId = homeTeam.Id,
            AwayTeamId = awayTeam.Id,
            HomeScore = 2,
            AwayScore = 1,
            PlayedAtUtc = DateTime.UtcNow
        };

        var result = await _matchService.CreateMatchAsync(dto, CancellationToken.None);

        Assert.That(result, Is.Not.Null);
        Assert.That(_dbContext.Matches.Count(), Is.EqualTo(1));
    }

    [Test]
    public void CreateMatchAsync_ShouldThrowConflictException_IfHomeTeamIdIsEqualToAwayTeamId()
    {
        var dto = new CreateMatchDto
        {
            HomeTeamId = 1,
            AwayTeamId = 1,
            HomeScore = 1,
            AwayScore = 0,
            PlayedAtUtc = DateTime.UtcNow
        };

        Assert.That(
            (Func<Task>)(async () => await _matchService.CreateMatchAsync(dto, CancellationToken.None)),
            Throws.TypeOf<ConflictException>()
        );
    }

    [Test]
    public async Task CreateMatchAsync_ShouldThrowNotFoundException_IfHomeTeamWithInvalidIdIsNotFound()
    {
        var awayTeam = new Team { Name = "Chelsea" };
        _dbContext.Teams.Add(awayTeam);
        await _dbContext.SaveChangesAsync();

        var dto = new CreateMatchDto
        {
            HomeTeamId = 999,
            AwayTeamId = awayTeam.Id,
            HomeScore = 1,
            AwayScore = 0,
            PlayedAtUtc = DateTime.UtcNow
        };

        Assert.That(
            (Func<Task>)(async () => await _matchService.CreateMatchAsync(dto, CancellationToken.None)),
            Throws.TypeOf<NotFoundException>()
        );
    }

    [Test]
    public async Task CreateMatchAsync_ShouldThrowNotFoundException_IfAwayTeamWithInvalidIdIsNotFound()
    {
        var homeTeam = new Team { Name = "Arsenal" };
        _dbContext.Teams.Add(homeTeam);
        await _dbContext.SaveChangesAsync();

        var dto = new CreateMatchDto
        {
            HomeTeamId = homeTeam.Id,
            AwayTeamId = 999,
            HomeScore = 1,
            AwayScore = 0,
            PlayedAtUtc = DateTime.UtcNow
        };

        Assert.That(
            (Func<Task>)(async () => await _matchService.CreateMatchAsync(dto, CancellationToken.None)),
            Throws.TypeOf<NotFoundException>()
        );
    }

    [Test]
    public async Task UpdateMatchAsync_ShouldUpdateMatch_IfUpdateMatchDtoIsValid()
    {
        var homeTeam = new Team { Name = "Arsenal" };
        var awayTeam = new Team { Name = "Chelsea" };

        _dbContext.Teams.AddRange(homeTeam, awayTeam);
        await _dbContext.SaveChangesAsync();

        var match = new Match
        {
            HomeTeamId = homeTeam.Id,
            AwayTeamId = awayTeam.Id,
            HomeScore = 1,
            AwayScore = 0,
            PlayedAtUtc = DateTime.UtcNow,
            CreatedAtUtc = DateTime.UtcNow
        };

        _dbContext.Matches.Add(match);
        await _dbContext.SaveChangesAsync();

        var updateMatchDto = new UpdateMatchDto
        {
            HomeTeamId = homeTeam.Id,
            AwayTeamId = awayTeam.Id,
            HomeScore = 3,
            AwayScore = 2,
            PlayedAtUtc = DateTime.UtcNow,
        };

        await _matchService.UpdateMatchAsync(match.Id, updateMatchDto, CancellationToken.None);

        var updated = await _dbContext.Matches.FindAsync(match.Id);

        Assert.That(updated!.HomeScore, Is.EqualTo(3));
        Assert.That(updated.AwayScore, Is.EqualTo(2));
    }

    [Test]
    public void UpdateMatchAsync_ShouldThrowNotFoundException_IfMatchWithGivenIdIsNotFound()
    {
        var updateMatchDto = new UpdateMatchDto
        {
            HomeTeamId = 1,
            AwayTeamId = 2,
            HomeScore = 1,
            AwayScore = 0,
            PlayedAtUtc = DateTime.UtcNow
        };

        Assert.That(
            (Func<Task>)(async () => await _matchService.UpdateMatchAsync(999, updateMatchDto, CancellationToken.None)),
            Throws.TypeOf<NotFoundException>()
        );
    }

    [Test]
    public async Task DeleteMatchAsync_ShouldDeleteMatch_IfIdIsValid()
    {
        var homeTeam = new Team { Name = "Arsenal" };
        var awayTeam = new Team { Name = "Chelsea" };

        _dbContext.Teams.AddRange(homeTeam, awayTeam);
        await _dbContext.SaveChangesAsync();

        var match = new Match
        {
            HomeTeamId = homeTeam.Id,
            AwayTeamId = awayTeam.Id,
            HomeScore = 1,
            AwayScore = 0,
            PlayedAtUtc = DateTime.UtcNow,
            CreatedAtUtc = DateTime.UtcNow
        };

        _dbContext.Matches.Add(match);
        await _dbContext.SaveChangesAsync();

        await _matchService.DeleteMatchAsync(match.Id, CancellationToken.None);

        var deleted = await _dbContext.Matches.FindAsync(match.Id);

        Assert.That(deleted, Is.Null);
    }
}
