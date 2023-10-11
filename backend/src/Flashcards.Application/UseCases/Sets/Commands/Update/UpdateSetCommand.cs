using MediatR;

namespace Flashcards.Application.UseCases.Sets.Commands.Update
{
    public class UpdateSetCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
