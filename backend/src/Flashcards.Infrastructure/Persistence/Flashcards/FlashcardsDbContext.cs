using Flashcards.Application.Common.Interfaces;
using Flashcards.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Infrastructure.Persistence.Flashcards
{
    internal class FlashcardsDbContext : DbContext, IFlashcardsDbContext
    {
        public const string DefaultSchema = "flashcards";
        public const string AccountSchema = "account";
        public FlashcardsDbContext(DbContextOptions<FlashcardsDbContext> options) : base(options)
        {
        }

        public DbSet<Card> Cards => Set<Card>();
        public DbSet<Set> Sets => Set<Set>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FlashcardsDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
