using FluentValidation;
using Mediat.R.Api.Core.Commands;
using Mediat.R.Api.Core.Queries;
using Mediat.R.Api.Interfaces.Repositories;
using Mediat.R.Api.Repositories;
using Mediat.R.Context;
using Mediat.R.Extensions;
using Mediat.R.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Mediat.R.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.ConfigureMediatR();
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();

            var connectionString = builder.Configuration.GetConnectionString("Library");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddScoped<IRepository, Repository>();
            builder.Services.AddScoped<IMediator, Mediator>();
            builder.Services.AddScoped<ICommandHandler<CreateBookCommand, Models.Book?>, CreateBookCommandHandler>();
            builder.Services.AddScoped<IQueryHandler<GetBookByIdQuery, Models.Book?>, GetBookByIdQueryHandler>();
            builder.Services.AddScoped<IQueryHandler<GetAllBooksQuery, IEnumerable<Models.Book>>, GetAllBooksQueryHandler>();
            builder.Services.AddScoped<IValidator<CreateBookCommand>, CreateBookCommandValidator>();

            var app = builder.Build();
            app.MapOpenApi();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "v1");
            });
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider
                    .GetRequiredService<AppDbContext>();

                context.Database.Migrate();
            }

            app.Run();
        }
    }
}
