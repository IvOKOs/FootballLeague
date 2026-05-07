namespace FootballLeague.Api.DTOs.TeamRankings;

public class TeamRanking
{
    public int TeamId { get; init; }
    public string TeamName { get; init; } = string.Empty;
    public int MatchesPlayed { get; init; }
    public int Wins { get; init; }
    public int Draws { get; init; }
    public int Losses { get; init; }
    public int Points { get; set; }
    public int GoalsFor { get; init; }
    public int GoalsAgainst { get; init; }
    public int GoalDifference => GoalsFor - GoalsAgainst;
}
