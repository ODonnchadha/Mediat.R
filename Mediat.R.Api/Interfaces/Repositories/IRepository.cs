namespace Mediat.R.Api.Interfaces.Repositories;

public interface IRepository
{
    Task<Entities.Book?> GetAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Entities.Book>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Entities.Book?> CreateAsync(Entities.Book entity, CancellationToken cancellationToken);
}
