namespace FootballLeague.Api.Services.TeamRankings.PointsCalculation;

public interface IPointsStrategy
{
    int CalculatePoints(int wins, int draws);
}