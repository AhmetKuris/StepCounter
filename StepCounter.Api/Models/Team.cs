namespace StepCounter.Api.Models;

public class Team
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public List<Counter> Counters { get; set; } = new();
}
