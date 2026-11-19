using Mediat.R.Api.Core.Mappings;
using Mediat.R.Api.Interfaces.Repositories;
using Mediat.R.Interfaces;

namespace Mediat.R.Api.Core.Commands
{
    public class CreateBookCommandHandler(IRepository repository): ICommandHandler<CreateBookCommand, Models.Book?>
    {
        public async Task<Models.Book?> HandleAsync(CreateBookCommand command, CancellationToken cancellationToken)
        {
            var entity = await repository.CreateAsync(
                new Entities.Book(command.Title, command.Description ?? string.Empty), cancellationToken);

            return entity?.ToBook();
        }
    }
}
