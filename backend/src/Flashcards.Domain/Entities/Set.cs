using Flashcards.Domain.Interfaces;

namespace Flashcards.Domain.Entities
{
    public class Set : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public DateTime DateAdd { get; set; }

        public IEnumerable<Card>? Cards { get; set; }
    }
}
