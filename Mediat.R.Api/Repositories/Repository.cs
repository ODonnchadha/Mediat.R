using Mediat.R.Api.Entities;
using Mediat.R.Api.Interfaces.Repositories;
using Mediat.R.Context;
using Microsoft.EntityFrameworkCore;

namespace Mediat.R.Api.Repositories
{
    public class Repository : IRepository
    {
        private readonly AppDbContext context;
        public Repository(AppDbContext context) => this.context = context;
        public async Task<Entities.Book?> CreateAsync(Entities.Book entity, CancellationToken cancellationToken)
        {
            Context.Entities.Book b = new Context.Entities.Book(entity.Title, entity.Description, DateTime.UtcNow);
            context.Books.Add(b);
            await context.SaveChangesAsync();
            
            return await GetAsync(b.Id, cancellationToken);
        }

        public async Task<IEnumerable<Entities.Book>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await context.Books
                .AsNoTracking()
                .Select(b => new Entities.Book
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    CreatedAt = b.CreatedAt,
                    IsCompleted = b.IsCompleted,
                    CompletedAt = b.CompletedAt
                }).ToListAsync(cancellationToken);
        }

        public async Task<Book?> GetAsync(long id, CancellationToken cancellationToken = default)
        {
            return await context.Books
                .AsNoTracking()
                .Where(b => b.Id == id)
                .Select(b => new Entities.Book
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    CreatedAt = b.CreatedAt,
                    IsCompleted = b.IsCompleted,
                    CompletedAt = b.CompletedAt
                }).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
