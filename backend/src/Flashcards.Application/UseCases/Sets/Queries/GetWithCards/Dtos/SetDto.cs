namespace Flashcards.Application.UseCases.Sets.Queries.GetWithCards.Dtos
{
    public class SetDto
    {
        public SetDto(int id, string name, IEnumerable<CardDto> cards)
        {
            Id = id;
            Name = name;
            Cards = cards;
        }

        public int Id { get; }
        public string Name { get; }
        public IEnumerable<CardDto> Cards { get; }
    }
}
