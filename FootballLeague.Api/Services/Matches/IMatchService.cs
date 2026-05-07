using FootballLeague.Api.DTOs.Matches;

namespace FootballLeague.Api.Services.Matches;

public interface IMatchService
{
    Task<MatchDto> CreateMatchAsync(CreateMatchDto createMatchDto, CancellationToken token);
    Task DeleteMatchAsync(int id, CancellationToken token);
    Task<MatchDto> GetMatchByIdAsync(int id, CancellationToken token);
    Task<MatchesCollectionDto> GetMatchesAsync(CancellationToken token);
    Task UpdateMatchAsync(int id, UpdateMatchDto updateMatchDto, CancellationToken token);
}