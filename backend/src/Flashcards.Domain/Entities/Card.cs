using Flashcards.Domain.Enums;
using Flashcards.Domain.Interfaces;

namespace Flashcards.Domain.Entities
{
    public class Card : IEntity
    {
        public int Id { get; set; }
        public string Definition { get; set; } = string.Empty;
        public string Term { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public int SetId { get; set; }
        public DateTime DateAdd { get; set; }
        public CardCorrectnessEnum Correctness { get; set; }
        public int Stage { get; set; }
        public DateTime? NextRepeatDate { get; set; }
        public Set? Set { get; set; }
    }
}
