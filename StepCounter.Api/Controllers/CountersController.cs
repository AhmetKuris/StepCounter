using Microsoft.AspNetCore.Mvc;
using StepCounter.Api.DTOs;
using StepCounter.Api.Services;

namespace StepCounter.Api.Controllers;

[ApiController]
[Route("api/teams/{teamId:guid}/[controller]")]
[Tags("Counters")]
public class CountersController : ControllerBase
{
    private readonly TeamService _service;

    public CountersController(TeamService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult Add(Guid teamId, [FromBody] CreateCounterDto dto)
    {
        _service.AddCounter(teamId, dto.Name);
        return Created("", null);
    }

    [HttpDelete("{counterId:guid}")]
    public IActionResult Delete(Guid teamId, Guid counterId)
    {
        _service.RemoveCounter(teamId, counterId);
        return NoContent();
    }

    [HttpPost("{counterId:guid}/increment")]
    public IActionResult Increment(Guid teamId, Guid counterId, [FromBody] IncrementCounterDto dto)
    {
        _service.IncrementCounter(teamId, counterId, dto.Steps);
        return Ok();
    }
}
