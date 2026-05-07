namespace FootballLeague.Api.DTOs.Matches;

public class CreateMatchDto
{
    public required int HomeTeamId { get; init; }
    public required int AwayTeamId { get; init; }
    public required int HomeScore { get; init; }
    public required int AwayScore { get; init; }
    public required DateTime PlayedAtUtc { get; init; }
}
