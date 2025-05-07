namespace StepCounter.Api.DTOs;

public class ErrorResponseDto
{
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; }
    public string? ErrorCode { get; set; }
} 