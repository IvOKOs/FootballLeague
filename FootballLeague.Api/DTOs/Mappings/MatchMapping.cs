using FootballLeague.Api.Database.Entities;
using FootballLeague.Api.DTOs.Matches;
using FootballLeague.Api.DTOs.Teams;
using System.Linq.Expressions;

namespace FootballLeague.Api.DTOs.Mappings;

public static class MatchMapping
{
    public static Expression<Func<Match, MatchDto>> ProjectToListDto()
    {
        return m => new MatchDto
        {
            Id = m.Id,
            HomeTeamId = m.HomeTeamId,
            AwayTeamId = m.AwayTeamId,
            HomeScore = m.HomeScore,
            AwayScore = m.AwayScore,
            PlayedAtUtc = m.PlayedAtUtc,
        };
    }

    public static Expression<Func<Match, MatchDto>> ProjectToDto()
    {
        return m => new MatchDto
        {
            Id = m.Id,
            HomeTeamId = m.HomeTeamId,
            AwayTeamId = m.AwayTeamId,
            HomeScore = m.HomeScore,
            AwayScore = m.AwayScore,
            PlayedAtUtc = m.PlayedAtUtc,
            HomeTeam = new TeamDto 
            { 
                Id = m.HomeTeam.Id,
                Name = m.HomeTeam.Name,
                CreatedAtUtc = m.HomeTeam.CreatedAtUtc,
            },
            AwayTeam = new TeamDto 
            { 
                Id = m.AwayTeam.Id,
                Name = m.AwayTeam.Name,
                CreatedAtUtc = m.AwayTeam.CreatedAtUtc,
            }
        };
    }

    public static MatchDto ToDto(this Match match)
    {
        return new MatchDto
        {
            Id = match.Id,
            HomeTeamId = match.HomeTeamId,
            AwayTeamId = match.AwayTeamId,
            HomeScore = match.HomeScore,
            AwayScore = match.AwayScore,
            PlayedAtUtc = match.PlayedAtUtc,
        };
    }

    public static Match ToEntity(this CreateMatchDto createMatchDto)
    {
        return new Match
        {
            HomeTeamId = createMatchDto.HomeTeamId,
            AwayTeamId = createMatchDto.AwayTeamId,
            HomeScore = createMatchDto.HomeScore,
            AwayScore = createMatchDto.AwayScore,
            PlayedAtUtc = createMatchDto.PlayedAtUtc,
            CreatedAtUtc = DateTime.UtcNow,
        };
    }

    public static void UpdateFromDto(this Match match, UpdateMatchDto updateMatchDto)
    {
        match.HomeTeamId = updateMatchDto.HomeTeamId;
        match.AwayTeamId = updateMatchDto.AwayTeamId;
        match.HomeScore = updateMatchDto.HomeScore;
        match.AwayScore = updateMatchDto.AwayScore;
        match.PlayedAtUtc = updateMatchDto.PlayedAtUtc;
        match.UpdatedAtUtc = DateTime.UtcNow;
    }
}
