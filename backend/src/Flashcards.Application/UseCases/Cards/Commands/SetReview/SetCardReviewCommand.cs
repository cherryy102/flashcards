using Flashcards.Domain.Enums;
using MediatR;

namespace Flashcards.Application.UseCases.Cards.Commands.SetReview
{
    public class SetCardReviewCommand : IRequest
    {
        public int CardId { get; set; }
        public CardCorrectnessEnum Correctness { get; set; }
    }
}
