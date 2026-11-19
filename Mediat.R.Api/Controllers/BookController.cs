using FluentValidation;
using Mediat.R.Api.Core.Commands;
using Mediat.R.Api.Core.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Mediat.R.Api.Controllers
{
    [ApiController(), ApiVersion("1.0")]
    [Route("api/books")]
    public class BookController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;

        // GET: api/v1/books
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Models.Book>>> GetBooks(CancellationToken cancellationToken)
        {
            var query = new GetAllBooksQuery();
            var result = await mediator.SendQueryAsync<GetAllBooksQuery, IEnumerable<Models.Book>>(query, cancellationToken);

            return Ok(result);
        }

        // GET: api/v1/books/{id}
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Models.Book>> GetBook([FromRoute] int id, CancellationToken cancellationToken)
        {
            var query = new GetBookByIdQuery(id);
            var result = await mediator.SendQueryAsync<GetBookByIdQuery, Models.Book?>(query, cancellationToken);

            if (result == null) return NotFound();
            return Ok(result);
        }

        // POST: api/v1/books
        [HttpPost(), ProducesResponseType(StatusCodes.Status201Created), ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Models.Book>> CreateBook([FromBody] CreateBookCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await mediator.SendCommandAsync<CreateBookCommand, Models.Book?>(command, cancellationToken);
                if (result == null) return NotFound();
                return CreatedAtAction(nameof(GetBook), new { id = result.Id, version = "1.0" }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
        }
    }
}
