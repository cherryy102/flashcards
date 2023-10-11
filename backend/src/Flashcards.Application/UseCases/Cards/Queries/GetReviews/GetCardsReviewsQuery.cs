using Flashcards.Application.UseCases.Cards.Queries.GetReviews.Dtos;
using MediatR;

namespace Flashcards.Application.UseCases.Cards.Queries.GetReviews
{
    public class GetCardsReviewsQuery : IRequest<IEnumerable<ReviewCardDto>>
    {
    }
}
