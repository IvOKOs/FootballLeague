using FootballLeague.Api.DTOs.Teams;

namespace FootballLeague.Api.DTOs.Matches;

public class MatchDto
{
    public required int Id { get; init; }
    public required int HomeTeamId { get; init; }
    public required int AwayTeamId { get; init; }
    public required int HomeScore { get; init; }
    public required int AwayScore { get; init; }
    public required DateTime PlayedAtUtc { get; init; }

    public TeamDto HomeTeam { get; init; } = null!;
    public TeamDto AwayTeam { get; init; } = null!;
}
