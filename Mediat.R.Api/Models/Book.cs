namespace Mediat.R.Api.Models;

public record Book(
    int Id,
    string Title,
    string? Description,
    bool IsCompleted,
    DateTime CreatedAt,
    DateTime? CompletedAt);
