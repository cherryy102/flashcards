using MediatR;

namespace Flashcards.Application.UseCases.Sets.Commands.Delete
{
    public class DeleteSetCommand : IRequest
    {
        public int Id { get; set; }
    }
}
