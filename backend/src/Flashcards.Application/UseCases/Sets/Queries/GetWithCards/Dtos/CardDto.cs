namespace Flashcards.Application.UseCases.Sets.Queries.GetWithCards.Dtos
{
    public class CardDto
    {
        public CardDto(int id, string definition, string term)
        {
            Id = id;
            Definition = definition;
            Term = term;
        }

        public int Id { get; }
        public string Definition { get; }
        public string Term { get; }
    }
}
