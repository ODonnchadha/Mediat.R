using Mediat.R.Interfaces;

namespace Mediat.R.Api.Core.Commands;

public record CreateBookCommand(string Title, string? Description): ICommand<Models.Book?>;
