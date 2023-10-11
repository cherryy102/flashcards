using Flashcards.Application.UseCases.Sets.Queries.Get.Dtos;
using MediatR;

namespace Flashcards.Application.UseCases.Sets.Queries.Get
{
    public class GetSetsQuery : IRequest<IEnumerable<SetDto>>
    {
    }
}
