using System.ComponentModel.DataAnnotations;

namespace StepCounter.Api.DTOs;

public class CreateTeamDto
{
    [Required(ErrorMessage = "Team name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Team name must be between 2 and 100 characters")]
    public string Name { get; set; } = string.Empty;
}
