using MediatR;

namespace Flashcards.Application.UseCases.Cards.Commands.Delete
{
    public class DeleteCardCommand : IRequest
    {
        public int Id { get; set; }
    }
}
