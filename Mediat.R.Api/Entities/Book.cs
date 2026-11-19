namespace Mediat.R.Api.Entities;

public class Book
{
    public Book() { }
    public Book(string title, string description)
    {
        Title = title;
        Description = description;
    }
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
