using Flashcards.Domain.Enums;
using MediatR;

namespace Flashcards.Application.UseCases.Cards.Commands.Update
{
    public class UpdateCardCommand : IRequest
    {
        public int Id { get; set; }
        public string? Definition { get; set; }
        public string? Term { get; set; }
        public CardCorrectnessEnum? Correctness { get; set; }
    }
}
