namespace StepCounter.Api.DTOs;

public class CounterResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Steps { get; set; }
} 