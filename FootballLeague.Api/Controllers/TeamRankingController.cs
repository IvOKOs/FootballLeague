using FootballLeague.Api.Database;
using FootballLeague.Api.DTOs.TeamRankings;
using FootballLeague.Api.Services.TeamRankings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FootballLeague.Api.Controllers;

[ApiController]
[Route("teamrankings")]
public class TeamRankingController : ControllerBase
{
    private readonly ITeamRankingService _teamRankingService;

    public TeamRankingController(ITeamRankingService teamRankingService)
    {
        _teamRankingService = teamRankingService;
    }

    [HttpGet]
    public async Task<ActionResult<TeamRankingsCollectionDto>> GetTeamRankings(CancellationToken token)
    {
        var teamRankings = await _teamRankingService.GetTeamRankingsAsync(token);
        var teamRankingsResponse = new TeamRankingsCollectionDto
        {
            Items = teamRankings
        };
        return Ok(teamRankingsResponse);
    }
}
