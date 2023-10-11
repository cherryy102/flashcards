using MediatR;

namespace Flashcards.Application.UseCases.Cards.Commands.Add
{
    public class AddCardCommand : IRequest
    {
        public string Definition { get; set; } = string.Empty;
        public string Term { get; set; } = string.Empty;
        public int SetId { get; set; }
    }
}
