using FootballLeague.Api.DTOs.Matches;
using FootballLeague.Api.Services.Matches;
using Microsoft.AspNetCore.Mvc;

namespace FootballLeague.Api.Controllers;

[ApiController]
[Route("matches")]
public class MatchController : ControllerBase
{
    private readonly IMatchService _matchService;

    public MatchController(IMatchService matchService)
    {
        _matchService = matchService;
    }

    [HttpGet]
    public async Task<ActionResult<MatchesCollectionDto>> GetMatches(CancellationToken token)
    {
        var matches = await _matchService.GetMatchesAsync(token);
        return Ok(matches);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MatchDto>> GetMatchById(int id, CancellationToken token)
    {
        var matchDto = await _matchService.GetMatchByIdAsync(id, token);
        return Ok(matchDto);
    }

    [HttpPost]
    public async Task<ActionResult> CreateMatch(CreateMatchDto createMatchDto, CancellationToken token)
    {
        var matchDto = await _matchService.CreateMatchAsync(createMatchDto, token);
        return CreatedAtAction(nameof(GetMatchById), new { id = matchDto.Id }, matchDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateMatchById(int id, UpdateMatchDto updateMatchDto, CancellationToken token)
    {
        await _matchService.UpdateMatchAsync(id, updateMatchDto, token);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMatchById(int id, CancellationToken token)
    {
        await _matchService.DeleteMatchAsync(id, token);
        return NoContent();
    }
}
