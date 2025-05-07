using Microsoft.AspNetCore.Mvc;
using StepCounter.Api.DTOs;
using StepCounter.Api.Services;

namespace StepCounter.Api.Controllers;

/// <summary>
/// Controller for managing teams and their step counters
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Tags("Teams")]
[Produces("application/json")]
public class TeamsController : ControllerBase
{
    private readonly TeamService _service;

    public TeamsController(TeamService service)
    {
        _service = service;
    }

    /// <summary>
    /// Gets all teams with their total step counts
    /// </summary>
    /// <returns>List of teams with their total steps</returns>
    /// <response code="200">Returns the list of teams</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TeamResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync()
    {
        var teams = await _service.GetAllTeamsAsync();
        var result = teams.Select(t => new TeamResponseDto
        {
            Id = t.Id,
            Name = t.Name,
            TotalSteps = t.Counters.Sum(c => c.Steps)
        });
        return Ok(result);
    }

    /// <summary>
    /// Creates a new team
    /// </summary>
    /// <param name="dto">Team creation data</param>
    /// <returns>The created team</returns>
    /// <response code="201">Returns the newly created team</response>
    /// <response code="400">If the dto is invalid</response>
    [HttpPost]
    [ProducesResponseType(typeof(TeamResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateTeamDto dto)
    {
        var team = await _service.CreateTeamAsync(dto.Name);
        var response = new TeamResponseDto
        {
            Id = team.Id,
            Name = team.Name,
            TotalSteps = 0
        };
        return Created($"/api/teams/{team.Id}", response);
    }

    /// <summary>
    /// Deletes a team
    /// </summary>
    /// <param name="teamId">The ID of the team to delete</param>
    /// <returns>No content</returns>
    /// <response code="204">If the team was successfully deleted</response>
    /// <response code="404">If the team was not found</response>
    [HttpDelete("{teamId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(Guid teamId)
    {
        await _service.DeleteTeamAsync(teamId);
        return NoContent();
    }

    /// <summary>
    /// Gets the total steps for a team
    /// </summary>
    /// <param name="teamId">The ID of the team</param>
    /// <returns>The total steps for the team</returns>
    /// <response code="200">Returns the total steps</response>
    /// <response code="404">If the team was not found</response>
    [HttpGet("{teamId:guid}/steps")]
    [ProducesResponseType(typeof(TeamResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTotalStepsAsync(Guid teamId)
    {
        var total = await _service.GetTeamTotalStepsAsync(teamId);
        var team = await _service.GetTeamAsync(teamId);
        var response = new TeamResponseDto
        {
            Id = teamId,
            Name = team?.Name ?? string.Empty,
            TotalSteps = total
        };
        return Ok(response);
    }

    /// <summary>
    /// Gets all counters for a team
    /// </summary>
    /// <param name="teamId">The ID of the team</param>
    /// <returns>List of counters in the team</returns>
    /// <response code="200">Returns the list of counters</response>
    /// <response code="404">If the team was not found</response>
    [HttpGet("{teamId:guid}/counters")]
    [ProducesResponseType(typeof(IEnumerable<CounterResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCountersAsync(Guid teamId)
    {
        var counters = await _service.GetCountersAsync(teamId);
        var result = counters.Select(c => new CounterResponseDto
        {
            Id = c.Id,
            Name = c.Name,
            Steps = c.Steps
        });
        return Ok(result);
    }
}