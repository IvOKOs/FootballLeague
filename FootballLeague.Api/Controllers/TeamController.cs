using FootballLeague.Api.DTOs.Teams;
using FootballLeague.Api.Services.Teams;
using Microsoft.AspNetCore.Mvc;

namespace FootballLeague.Api.Controllers;

[ApiController]
[Route("teams")]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpGet]
    public async Task<ActionResult<TeamsCollectionDto>> GetTeams(CancellationToken token)
    {
        var teams = await _teamService.GetTeamsAsync(token);
        return Ok(teams);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TeamDto>> GetTeamById(int id, CancellationToken token)
    {
        var teamDto = await _teamService.GetTeamByIdAsync(id, token);
        return Ok(teamDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTeam(CreateTeamDto createTeamDto, CancellationToken token)
    {
        var teamDto = await _teamService.CreateTeamAsync(createTeamDto, token);
        return CreatedAtAction(nameof(GetTeamById), new { id = teamDto.Id }, teamDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTeamById(int id, UpdateTeamDto updateTeamDto, CancellationToken token)
    {
        await _teamService.UpdateTeamAsync(id, updateTeamDto, token);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTeamById(int id, CancellationToken token)
    {
        await _teamService.DeleteTeamAsync(id, token);
        return NoContent();
    }
}
