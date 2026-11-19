using Mediat.R.Api.Interfaces.Repositories;
using Mediat.R.Interfaces;
using Mediat.R.Api.Core.Mappings;

namespace Mediat.R.Api.Core.Queries
{
    public class GetAllBooksQueryHandler(IRepository repository): IQueryHandler<GetAllBooksQuery, IEnumerable<Models.Book>>
    {
        public async Task<IEnumerable<Models.Book>> HandleAsync(GetAllBooksQuery query, CancellationToken cancellationToken)
        {
            var entities = await repository.GetAllAsync(cancellationToken);
            return entities.Select(b => b.ToBook());
        }
    }
}
