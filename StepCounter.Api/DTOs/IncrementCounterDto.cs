using System.ComponentModel.DataAnnotations;

namespace StepCounter.Api.DTOs;

public class IncrementCounterDto
{
    [Required(ErrorMessage = "Steps count is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Steps count must be greater than 0")]
    public int Steps { get; set; }
}
