using Flashcards.Domain.Enums;

namespace Flashcards.Application.UseCases.Cards.Queries.Get.Dtos
{
    public class CardDto
    {
        public int Id { get; set; }
        public string Definition { get; set; } = string.Empty;
        public string Term { get; set; } = string.Empty;
        public int SetId { get; set; }
        public CardCorrectnessEnum Correctness { get; set; }
    }
}
