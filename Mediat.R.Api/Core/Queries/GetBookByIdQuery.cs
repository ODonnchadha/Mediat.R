using Mediat.R.Interfaces;

namespace Mediat.R.Api.Core.Queries;

public record GetBookByIdQuery(long Id): IQuery<Models.Book?>;
