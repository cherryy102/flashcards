namespace Flashcards.Application.UseCases.Cards.Queries.GetReviews.Dtos
{
    public class ReviewCardDto
    {
        public int Id { get; set; }
        public string Definition { get; set; } = string.Empty;
        public string Term { get; set; } = string.Empty;
    }
}
