using Microsoft.AspNetCore.Mvc;
using StepCounter.Api.DTOs;
using StepCounter.Api.Services;

namespace StepCounter.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Teams")]
public class TeamsController : ControllerBase
{
    private readonly TeamService _service;

    public TeamsController(TeamService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var teams = _service.GetAllTeams()
            .Select(t => new
            {
                t.Id,
                t.Name,
                TotalSteps = t.Counters.Sum(c => c.Steps)
            });
        return Ok(teams);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateTeamDto dto)
    {
        _service.CreateTeam(dto.Name);
        return Created("", null);
    }

    [HttpDelete("{teamId:guid}")]
    public IActionResult Delete(Guid teamId)
    {
        _service.DeleteTeam(teamId);
        return NoContent();
    }

    [HttpGet("{teamId:guid}/steps")]
    public IActionResult GetTotalSteps(Guid teamId)
    {
        var total = _service.GetTeamTotalSteps(teamId);
        return Ok(new { TeamId = teamId, TotalSteps = total });
    }

    [HttpGet("{teamId:guid}/counters")]
    public IActionResult GetCounters(Guid teamId)
    {
        var counters = _service.GetCounters(teamId)
            .Select(c => new { c.Id, c.Name, c.Steps });
        return Ok(counters);
    }
}