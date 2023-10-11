using Flashcards.Application.UseCases.Cards.Queries.Get.Dtos;
using MediatR;

namespace Flashcards.Application.UseCases.Cards.Queries.Get
{
    public class GetCardsQuery : IRequest<IEnumerable<CardDto>>
    {
        public int SetId { get; set; }
    }
}
