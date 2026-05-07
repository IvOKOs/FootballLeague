namespace FootballLeague.Api.DTOs.Teams;

public class TeamsCollectionDto : ICollectionResponse<TeamDto>
{
    public required List<TeamDto> Items { get; init; }
}
