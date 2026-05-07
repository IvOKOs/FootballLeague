using FootballLeague.Api.Database;
using FootballLeague.Api.DTOs.Mappings;
using FootballLeague.Api.DTOs.Matches;
using FootballLeague.Api.Exceptions;
using Microsoft.EntityFrameworkCore;
using static FootballLeague.Api.DTOs.Mappings.MatchMapping;

namespace FootballLeague.Api.Services.Matches;

public class MatchService : IMatchService
{
    private readonly IFootballDbContext _dbContext;

    public MatchService(IFootballDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MatchesCollectionDto> GetMatchesAsync(CancellationToken token)
    {
        var matchesDto = await _dbContext.Matches
            .Select(ProjectToListDto())
            .ToListAsync(token);

        return new MatchesCollectionDto
        {
            Items = matchesDto
        };
    }

    public async Task<MatchDto> GetMatchByIdAsync(int id, CancellationToken token)
    {
        var matchDto = await _dbContext.Matches
            .Where(m => m.Id == id)
            .Select(ProjectToDto())
            .FirstOrDefaultAsync(token);

        if (matchDto is null)
        {
            throw new NotFoundException($"Match with id {id} was not found.");
        }

        return matchDto;
    }

    public async Task<MatchDto> CreateMatchAsync(CreateMatchDto createMatchDto, CancellationToken token)
    {
        if(createMatchDto.HomeTeamId == createMatchDto.AwayTeamId)
        {
            throw new ConflictException("Ids of home team and away team must be distinct.");
        }
        var homeTeamExists = await _dbContext.Teams.AnyAsync(t => t.Id == createMatchDto.HomeTeamId, token);
        if (!homeTeamExists)
        {
            throw new NotFoundException($"Team with id {createMatchDto.HomeTeamId} was not found.");
        }
        var awayTeamExists = await _dbContext.Teams.AnyAsync(t => t.Id == createMatchDto.AwayTeamId, token);
        if (!awayTeamExists)
        {
            throw new NotFoundException($"Team with id {createMatchDto.AwayTeamId} was not found.");
        } 

        var match = createMatchDto.ToEntity();

        _dbContext.Matches.Add(match);
        await _dbContext.SaveChangesAsync(token);

        var matchDto = match.ToDto();
        return matchDto;
    }

    public async Task UpdateMatchAsync(int id, UpdateMatchDto updateMatchDto, CancellationToken token)
    {
        var match = await _dbContext.Matches.FindAsync(id, token);

        if (match is null)
        {
            throw new NotFoundException($"Match with id {id} was not found.");
        }

        match.UpdateFromDto(updateMatchDto);
        await _dbContext.SaveChangesAsync(token);
    }

    public async Task DeleteMatchAsync(int id, CancellationToken token)
    {
        var match = await _dbContext.Matches.FindAsync(id, token);

        if (match is null)
        {
            throw new NotFoundException($"Match with id {id} was not found.");
        }

        _dbContext.Remove(match);
        await _dbContext.SaveChangesAsync(token);
    }
}
