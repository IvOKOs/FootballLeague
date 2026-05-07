namespace FootballLeague.Api.Services.TeamRankings.PointsCalculation;

public class StandardPointsStrategy : IPointsStrategy
{
    public int CalculatePoints(int wins, int draws)
    {
        return wins * (int)MatchOutcome.Win + draws;
    }
}
