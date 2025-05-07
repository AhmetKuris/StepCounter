namespace StepCounter.Api.DTOs;

public class TeamResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TotalSteps { get; set; }
} 