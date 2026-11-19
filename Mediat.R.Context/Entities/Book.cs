namespace Mediat.R.Context.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public bool IsCompleted { get; private set; } = false;
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public Book(string title, string description, DateTime createdAt)
    {
        Title = title;
        Description = description;
        CreatedAt = createdAt;
    }
}