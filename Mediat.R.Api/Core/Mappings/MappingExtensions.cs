namespace Mediat.R.Api.Core.Mappings;

public static class MappingExtensions
{
    /// <summary>
    /// Map internalized entity to that of the client-facing DTO.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static Models.Book ToBook(this Entities.Book item)
    {
        return new Models.Book(
            item.Id,
            item.Title,
            item.Description,
            item.IsCompleted,
            item.CreatedAt,
            item.CompletedAt);
    }
}
