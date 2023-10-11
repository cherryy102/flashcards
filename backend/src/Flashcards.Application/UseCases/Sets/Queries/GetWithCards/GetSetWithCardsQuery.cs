using Flashcards.Application.UseCases.Sets.Queries.GetWithCards.Dtos;
using MediatR;

namespace Flashcards.Application.UseCases.Sets.Queries.GetWithCards
{
    public class GetSetWithCardsQuery : IRequest<SetDto>
    {
        public int SetId { get; set; }
    }
}
