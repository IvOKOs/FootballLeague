namespace FootballLeague.Api.DTOs.Matches;

public class MatchesCollectionDto : ICollectionResponse<MatchDto>
{
    public required List<MatchDto> Items { get; init; }
}
