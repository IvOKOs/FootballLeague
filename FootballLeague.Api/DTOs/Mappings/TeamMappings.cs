using FootballLeague.Api.Database.Entities;
using FootballLeague.Api.DTOs.Matches;
using FootballLeague.Api.DTOs.Teams;
using System.Linq.Expressions;

namespace FootballLeague.Api.DTOs.Mappings;

public static class TeamMappings
{
    public static Expression<Func<Team, TeamDto>> ProjectToListDto()
    {
        return t => new TeamDto
        {
            Id = t.Id,
            Name = t.Name,
            CreatedAtUtc = t.CreatedAtUtc,
        };
    }

    public static Expression<Func<Team, TeamDto>> ProjectToDto()
    {
        return t => new TeamDto
        {
            Id = t.Id,
            Name = t.Name,

            HomeMatches = t.HomeMatches.Select(m => new MatchDto
            {
                Id = m.Id,
                HomeTeamId = m.HomeTeamId,
                AwayTeamId = m.AwayTeamId,
                PlayedAtUtc = m.PlayedAtUtc,
                HomeScore = m.HomeScore,
                AwayScore = m.AwayScore
            }),

            AwayMatches = t.AwayMatches.Select(m => new MatchDto
            {
                Id = m.Id,
                HomeTeamId = m.HomeTeamId,
                AwayTeamId = m.AwayTeamId,
                PlayedAtUtc = m.PlayedAtUtc,
                HomeScore = m.HomeScore,
                AwayScore = m.AwayScore
            }),
            CreatedAtUtc = DateTime.UtcNow,
        };
    }
    

    public static TeamDto ToDto(this Team team)
    {
        return new TeamDto
        {
            Id = team.Id,
            Name = team.Name,
            CreatedAtUtc = team.CreatedAtUtc,
        };
    }

    public static Team ToEntity(this CreateTeamDto createTeamDto)
    {
        return new Team
        {
            Name = createTeamDto.Name,
            CreatedAtUtc = DateTime.UtcNow,
        };
    }

    public static void UpdateFromDto(this Team team, UpdateTeamDto updateTeamDto)
    {
        team.Name = updateTeamDto.Name;
        team.UpdatedAtUtc = DateTime.UtcNow;
    }
}
