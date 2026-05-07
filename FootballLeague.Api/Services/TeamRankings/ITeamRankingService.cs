using FootballLeague.Api.DTOs.TeamRankings;

namespace FootballLeague.Api.Services.TeamRankings
{
    public interface ITeamRankingService
    {
        Task<List<TeamRanking>> GetTeamRankingsAsync(CancellationToken token);
    }
}