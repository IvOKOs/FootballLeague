using FootballLeague.Api.DTOs.Teams;

namespace FootballLeague.Api.Services.Teams;

public interface ITeamService
{
    Task<TeamsCollectionDto> GetTeamsAsync(CancellationToken token);
    Task<TeamDto> GetTeamByIdAsync(int id, CancellationToken token);
    Task<TeamDto> CreateTeamAsync(CreateTeamDto dto, CancellationToken token);
    Task UpdateTeamAsync(int id, UpdateTeamDto dto, CancellationToken token);
    Task DeleteTeamAsync(int id, CancellationToken token);
}