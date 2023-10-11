using MediatR;

namespace Flashcards.Application.UseCases.Sets.Commands.Add
{
    public class AddSetCommand : IRequest
    {
        public string Name { get; set; } = string.Empty;
    }
}
