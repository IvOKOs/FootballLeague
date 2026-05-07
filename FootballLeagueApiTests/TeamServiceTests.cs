using FootballLeague.Api.Database;
using FootballLeague.Api.Database.Entities;
using FootballLeague.Api.DTOs.Teams;
using FootballLeague.Api.Exceptions;
using FootballLeague.Api.Services.Teams;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FootballLeagueApiTests;

[TestFixture]
public class TeamServiceTests
{
    private FootballDbContext _dbContext = null!;
    private TeamService _teamService = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<FootballDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new FootballDbContext(options);
        _teamService = new TeamService(_dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }

    [Test]
    public async Task GetTeamByIdAsync_ShouldReturnTeam_WhenTeamWithGivenIdExists()
    {
        var team = new Team
        {
            Name = "Arsenal"
        };

        _dbContext.Teams.Add(team);
        await _dbContext.SaveChangesAsync();

        var result = await _teamService.GetTeamByIdAsync(team.Id, CancellationToken.None);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(team.Id));
        Assert.That(result.Name, Is.EqualTo(team.Name));
    }

    [Test]
    public void GetTeamByIdAsync_ShouldThrowNotFoundException_WhenTeamWithGivenIdDoesNotExist()
    {
        var invalidId = 999;

        Assert.ThrowsAsync<NotFoundException>((Func<Task>)(async () =>
                    await _teamService.GetTeamByIdAsync(invalidId, CancellationToken.None)));

    }

    [Test]
    public async Task CreateTeamAsync_ShouldCreateTeam_WhenNameIsUnique()
    {
        var dto = new CreateTeamDto
        {
            Name = "Chelsea"
        };

        var result = await _teamService.CreateTeamAsync(dto, CancellationToken.None);

        Assert.That(result, Is.Not.Null);

        var teamInDb = await _dbContext.Teams.FirstOrDefaultAsync();

        Assert.That(teamInDb, Is.Not.Null);
        Assert.That(teamInDb!.Name, Is.EqualTo(dto.Name));
    }

    [Test]
    public void CreateTeamAsync_ShouldThrowConflictException_WhenTeamAlreadyExists()
    {
        var existingTeam = new Team
        {
            Name = "Liverpool"
        };

        _dbContext.Teams.Add(existingTeam);
        _dbContext.SaveChanges();

        var dto = new CreateTeamDto
        {
            Name = "Liverpool"
        };

        Assert.ThrowsAsync<ConflictException>((Func<Task>)(async () =>
            await _teamService.CreateTeamAsync(dto, CancellationToken.None)));
    }

    [Test]
    public async Task UpdateTeamAsync_ShouldUpdateTeam_WhenTeamExists()
    {
        var team = new Team
        {
            Name = "Old Name"
        };

        _dbContext.Teams.Add(team);
        await _dbContext.SaveChangesAsync();

        var dto = new UpdateTeamDto
        {
            Name = "New Name"
        };

        await _teamService.UpdateTeamAsync(team.Id, dto, CancellationToken.None);

        var updatedTeam = await _dbContext.Teams.FindAsync(team.Id);
        Assert.That(updatedTeam, Is.Not.Null);
        Assert.That(updatedTeam!.Name, Is.EqualTo("New Name"));
    }

    [Test]
    public async Task DeleteTeamAsync_ShouldDeleteTeam_WhenTeamExists()
    {
        var team = new Team
        {
            Name = "Delete Me"
        };

        _dbContext.Teams.Add(team);
        await _dbContext.SaveChangesAsync();

        await _teamService.DeleteTeamAsync(team.Id, CancellationToken.None);

        var deletedTeam = await _dbContext.Teams.FindAsync(team.Id);
        Assert.That(deletedTeam, Is.Null);
    }
}
