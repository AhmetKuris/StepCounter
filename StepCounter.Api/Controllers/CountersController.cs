using Microsoft.AspNetCore.Mvc;
using StepCounter.Api.DTOs;
using StepCounter.Api.Services;

namespace StepCounter.Api.Controllers;

/// <summary>
/// Controller for managing step counters within teams
/// </summary>
[ApiController]
[Route("api/teams/{teamId:guid}/[controller]")]
[Tags("Counters")]
[Produces("application/json")]
public class CountersController : ControllerBase
{
    private readonly TeamService _service;

    public CountersController(TeamService service)
    {
        _service = service;
    }

    /// <summary>
    /// Creates a new counter for a team
    /// </summary>
    /// <param name="teamId">The ID of the team</param>
    /// <param name="dto">Counter creation data</param>
    /// <returns>The created counter</returns>
    /// <response code="201">Returns the newly created counter</response>
    /// <response code="400">If the dto is invalid</response>
    /// <response code="404">If the team was not found</response>
    [HttpPost]
    [ProducesResponseType(typeof(CounterResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddAsync(Guid teamId, [FromBody] CreateCounterDto dto)
    {
        try
        {
            var counter = await _service.AddCounterAsync(teamId, dto.Name);
            var response = new CounterResponseDto
            {
                Id = counter.Id,
                Name = counter.Name,
                Steps = counter.Steps
            };
            return Created($"/api/teams/{teamId}/counters/{counter.Id}", response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ErrorResponseDto { Message = ex.Message });
        }
    }

    /// <summary>
    /// Deletes a counter from a team
    /// </summary>
    /// <param name="teamId">The ID of the team</param>
    /// <param name="counterId">The ID of the counter to delete</param>
    /// <returns>No content</returns>
    /// <response code="204">If the counter was successfully deleted</response>
    /// <response code="404">If the team or counter was not found</response>
    [HttpDelete("{counterId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(Guid teamId, Guid counterId)
    {
        try
        {
            await _service.RemoveCounterAsync(teamId, counterId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ErrorResponseDto { Message = ex.Message });
        }
    }

    /// <summary>
    /// Increments the step count for a counter
    /// </summary>
    /// <param name="teamId">The ID of the team</param>
    /// <param name="counterId">The ID of the counter</param>
    /// <param name="dto">The increment data</param>
    /// <returns>The updated counter</returns>
    /// <response code="200">Returns the updated counter</response>
    /// <response code="400">If the dto is invalid</response>
    /// <response code="404">If the team or counter was not found</response>
    [HttpPost("{counterId:guid}/increment")]
    [ProducesResponseType(typeof(CounterResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> IncrementAsync(Guid teamId, Guid counterId, [FromBody] IncrementCounterDto dto)
    {
        try
        {
            var counter = await _service.IncrementCounterAsync(teamId, counterId, dto.Steps);
            var response = new CounterResponseDto
            {
                Id = counter.Id,
                Name = counter.Name,
                Steps = counter.Steps
            };
            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ErrorResponseDto { Message = ex.Message });
        }
    }
}
