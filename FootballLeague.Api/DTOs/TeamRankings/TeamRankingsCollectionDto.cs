
namespace FootballLeague.Api.DTOs.TeamRankings;

public class TeamRankingsCollectionDto : ICollectionResponse<TeamRanking>
{
    public required List<TeamRanking> Items { get; init; }
}
