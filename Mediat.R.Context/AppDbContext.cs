using Mediat.R.Context.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mediat.R.Context
{
    public class AppDbContext : DbContext
    {
        protected AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Book> Books => Set<Book>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>().HasData(
                new Book(
                    "At Swim-Two-Birds",
                    "The story follows an unnamed, lazy Dublin college student living with his uncle who writes a metafictional novel about a bad author whose characters rebel against him.",
                    new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                )
                {
                    Id = 1
                },
                new Book(
                    "A Portrait of the Artist as a Young Man",
                    "A 1916 modernist semi-autobiographical novel by Irish writer James Joyce that follows Stephen Dedalus from childhood to young adulthood as he rebels against Irish, Catholic, and family constraints to pursue art in Europe.",
                    new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                )
                {
                    Id = 2
                },
                new Book(
                    "Pylon",
                    "Set outside his usual Yoknapatawpha County in a thinly disguised New Orleans during Mardi Gras, it follows an unnamed newspaper reporter drawn into the lives of a destitute, rootless barnstorming air-race team.",
                    new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                )
                {
                    Id = 3
                }
            );
        }
    }
}
