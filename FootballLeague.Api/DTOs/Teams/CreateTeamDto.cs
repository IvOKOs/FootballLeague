using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Api.DTOs.Teams;

public class CreateTeamDto
{
    [MinLength(3)]
    public required string Name { get; init; } = string.Empty;
}
