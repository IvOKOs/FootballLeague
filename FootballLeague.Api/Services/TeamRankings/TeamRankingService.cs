using FootballLeague.Api.Database;
using FootballLeague.Api.DTOs.TeamRankings;
using FootballLeague.Api.Services.TeamRankings.PointsCalculation;
using Microsoft.EntityFrameworkCore;

namespace FootballLeague.Api.Services.TeamRankings;

public class TeamRankingService : ITeamRankingService
{
    private readonly IFootballDbContext _dbContext;
    private readonly IPointsStrategy _pointsStrategy;

    public TeamRankingService(IFootballDbContext dbContext, IPointsStrategy pointsStrategy)
    {
        _dbContext = dbContext;
        _pointsStrategy = pointsStrategy;
    }

    public async Task<List<TeamRanking>> GetTeamRankingsAsync(CancellationToken token)
    {
        var homeMatchesQuery = _dbContext.Matches.Select(m => new
        {
            TeamId = m.HomeTeamId,
            TeamName = m.HomeTeam.Name,
            Goals = m.HomeScore,
            GoalsAgainst = m.AwayScore,
            Win = m.HomeScore > m.AwayScore,
            Draw = m.HomeScore == m.AwayScore,
            Loss = m.HomeScore < m.AwayScore
        });

        var awayMatchesQuery = _dbContext.Matches.Select(m => new
        {
            TeamId = m.AwayTeamId,
            TeamName = m.AwayTeam.Name,
            Goals = m.AwayScore,
            GoalsAgainst = m.HomeScore,
            Win = m.AwayScore > m.HomeScore,
            Draw = m.HomeScore == m.AwayScore,
            Loss = m.AwayScore < m.HomeScore
        });

        var rankings = await homeMatchesQuery
            .Concat(awayMatchesQuery)
            .GroupBy(t => new { t.TeamId, t.TeamName })
            .Select(g => new TeamRanking
            {
                TeamId = g.Key.TeamId,
                TeamName = g.Key.TeamName,
                MatchesPlayed = g.Count(),
                Wins = g.Count(t => t.Win),
                Draws = g.Count(t => t.Draw),
                Losses = g.Count(t => t.Loss),
                GoalsFor = g.Sum(t => t.Goals),
                GoalsAgainst = g.Sum(t => t.GoalsAgainst)
            })
            .ToListAsync(token);

        CalculatePoints(rankings);

        var orderedRankings = OrderTeamRankings(rankings);

        return orderedRankings;
    }

    private void CalculatePoints(List<TeamRanking> rankings)
    {
        foreach (var team in rankings)
        {
            team.Points = _pointsStrategy.CalculatePoints(team.Wins, team.Draws);
        }
    }

    private List<TeamRanking> OrderTeamRankings(List<TeamRanking> rankings)
    {
        var orderedRankings = rankings
            .OrderByDescending(r => r.Points)
            .ThenByDescending(r => r.GoalsFor - r.GoalsAgainst)
            .ToList();

        return orderedRankings;
    }
}
