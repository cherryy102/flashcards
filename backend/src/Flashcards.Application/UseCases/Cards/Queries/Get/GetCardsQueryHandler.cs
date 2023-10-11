using Flashcards.Application.Common.Interfaces;
using Flashcards.Application.Common.Interfaces.UserResolver;
using Flashcards.Application.UseCases.Cards.Queries.Get.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Application.UseCases.Cards.Queries.Get
{
    internal class GetCardsQueryHandler : IRequestHandler<GetCardsQuery, IEnumerable<CardDto>>
    {
        private readonly IUserResolver userResolver;
        private readonly IFlashcardsDbContext dbContext;

        public GetCardsQueryHandler(IUserResolver userResolver, IFlashcardsDbContext dbContext)
        {
            this.userResolver = userResolver;
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<CardDto>> Handle(GetCardsQuery request, CancellationToken cancellationToken)
        {
            var cards = await dbContext.Cards
                .AsNoTracking()
                .Where(x => x.UserId == userResolver.Id)
                .Where(x => x.SetId == request.SetId)
                .Select(x => new CardDto
                {
                    Id = x.Id,
                    Definition = x.Definition,
                    Term = x.Term,
                    SetId = x.SetId,
                    Correctness = x.Correctness,
                })
                .ToListAsync(cancellationToken);
            return cards;
        }
    }
}
