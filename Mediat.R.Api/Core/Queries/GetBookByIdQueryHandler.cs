using Mediat.R.Api.Core.Mappings;
using Mediat.R.Api.Interfaces.Repositories;
using Mediat.R.Interfaces;

namespace Mediat.R.Api.Core.Queries
{
    public class GetBookByIdQueryHandler(IRepository repository): IQueryHandler<GetBookByIdQuery, Models.Book?>
    {
        public async Task<Models.Book?> HandleAsync(GetBookByIdQuery query, CancellationToken cancellationToken)
        {
            var entity = await repository.GetAsync(query.Id, cancellationToken);
            return entity?.ToBook();
        }
    }
}
