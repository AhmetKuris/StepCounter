namespace StepCounter.Api.Models;

public class Counter
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public int Steps { get; set; } = 0;
}
