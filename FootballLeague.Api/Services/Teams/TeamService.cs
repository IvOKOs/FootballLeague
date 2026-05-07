using FootballLeague.Api.Database;
using FootballLeague.Api.DTOs.Teams;
using FootballLeague.Api.Exceptions;
using Microsoft.EntityFrameworkCore;
using static FootballLeague.Api.DTOs.Mappings.TeamMappings;

namespace FootballLeague.Api.Services.Teams;

public class TeamService : ITeamService
{
    private readonly IFootballDbContext _dbContext;

    public TeamService(IFootballDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TeamsCollectionDto> GetTeamsAsync(CancellationToken token)
    {
        var teamsDto = await _dbContext.Teams
            .Select(ProjectToListDto())
            .ToListAsync(token);

        return new TeamsCollectionDto 
        { 
            Items = teamsDto 
        };
    }

    public async Task<TeamDto> GetTeamByIdAsync(int id, CancellationToken token)
    {
        var teamDto = await _dbContext.Teams
            .Where(t => t.Id == id)
            .Select(ProjectToDto())
            .FirstOrDefaultAsync(token);

        if(teamDto is null)
        {
            throw new NotFoundException($"Team with id {id} was not found.");
        }

        return teamDto;
    }

    public async Task<TeamDto> CreateTeamAsync(CreateTeamDto createTeamDto, CancellationToken token)
    {
        var team = createTeamDto.ToEntity();

        if (await _dbContext.Teams.AnyAsync(t => t.Name.ToLower() == createTeamDto.Name.ToLower(), token))
        {
            throw new ConflictException($"Team with name {createTeamDto.Name} already exists.");
        }

        _dbContext.Teams.Add(team);
        await _dbContext.SaveChangesAsync(token);

        var teamDto = team.ToDto();
        return teamDto;
    }

    public async Task UpdateTeamAsync(int id, UpdateTeamDto updateTeamDto, CancellationToken token)
    {
        var team = await _dbContext.Teams.FindAsync([id], token);

        if (team is null)
        {
            throw new NotFoundException($"Team with id {id} was not found.");
        }

        team.UpdateFromDto(updateTeamDto);
        await _dbContext.SaveChangesAsync(token);
    }

    public async Task DeleteTeamAsync(int id, CancellationToken token)
    {
        var team = await _dbContext.Teams.FindAsync([id], token);

        if (team is null)
        {
            throw new NotFoundException($"Team with id {id} was not found.");
        }

        _dbContext.Teams.Remove(team);
        await _dbContext.SaveChangesAsync(token);
    }
}
