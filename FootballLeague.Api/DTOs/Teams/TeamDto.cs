using FootballLeague.Api.DTOs.Matches;

namespace FootballLeague.Api.DTOs.Teams;

public class TeamDto
{
    public required int Id { get; init; }
    public required string Name { get; init; } = string.Empty;
    public required DateTime CreatedAtUtc { get; init; }

    public IEnumerable<MatchDto> HomeMatches { get; init; } = new List<MatchDto>();
    public IEnumerable<MatchDto> AwayMatches { get; init; } = new List<MatchDto>();
}
