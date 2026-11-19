using Mediat.R.Interfaces;

namespace Mediat.R.Api.Core.Queries;

public record GetAllBooksQuery(): IQuery<IEnumerable<Models.Book>>;
