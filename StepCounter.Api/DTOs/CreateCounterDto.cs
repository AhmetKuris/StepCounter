using System.ComponentModel.DataAnnotations;

namespace StepCounter.Api.DTOs;

public class CreateCounterDto
{
    [Required(ErrorMessage = "Counter name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Counter name must be between 2 and 100 characters")]
    public string Name { get; set; } = string.Empty;
}
