using Flashcards.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Application.Common.Interfaces
{
    public interface IFlashcardsDbContext
    {
        public DbSet<Card> Cards { get; }
        public DbSet<Set> Sets { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
    }
}
