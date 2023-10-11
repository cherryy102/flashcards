using Flashcards.Application.Common.Interfaces;
using Flashcards.Application.Common.Interfaces.UserResolver;
using Flashcards.Application.UseCases.Sets.Queries.GetWithCards.Dtos;
using Flashcards.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Application.UseCases.Sets.Queries.GetWithCards
{
    internal class GetSetWithCardsQueryHandler : IRequestHandler<GetSetWithCardsQuery, SetDto>
    {
        private readonly IFlashcardsDbContext dbContext;
        private readonly IUserResolver userResolver;

        public GetSetWithCardsQueryHandler(IFlashcardsDbContext dbContext, IUserResolver userResolver)
        {
            this.dbContext = dbContext;
            this.userResolver = userResolver;
        }
        public async Task<SetDto> Handle(GetSetWithCardsQuery request, CancellationToken cancellationToken)
        {
            var set = await dbContext.Sets
                .AsNoTracking()
                .Where(x => x.Id == request.SetId)
                .Where(x => x.UserId == userResolver.Id)
                .Select(x => new SetDto(
                        x.Id,
                        x.Name,
                        x.Cards!
                            .OrderByDescending(y => y.DateAdd)
                            .Select(y => new CardDto(y.Id, y.Definition, y.Term))
                            .ToList()
                    )
                )
                .FirstOrDefaultAsync(cancellationToken);
            if (set == null)
            {
                throw new NotFoundException("Set does not exists");
            }
            return set;
        }
    }
}
