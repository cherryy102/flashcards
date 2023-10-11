using Flashcards.Application.Common.Interfaces;
using Flashcards.Application.Common.Interfaces.UserResolver;
using Flashcards.Application.UseCases.Cards.Queries.GetReviews.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Application.UseCases.Cards.Queries.GetReviews
{
    internal class GetCardsReviewsHandler : IRequestHandler<GetCardsReviewsQuery, IEnumerable<ReviewCardDto>>
    {
        private readonly IUserResolver userResolver;
        private readonly IFlashcardsDbContext dbContext;

        public GetCardsReviewsHandler(IFlashcardsDbContext dbContext, IUserResolver userResolver)
        {
            this.dbContext = dbContext;
            this.userResolver = userResolver;
        }

        public async Task<IEnumerable<ReviewCardDto>> Handle(GetCardsReviewsQuery request, CancellationToken cancellationToken)
        {
            var currentDate = DateTime.UtcNow.Date;
            var cards = await dbContext.Cards
                .AsNoTracking()
                .Where(x => x.UserId == userResolver.Id)
                .Where(x => x.NextRepeatDate!.Value.Date <= currentDate)
                .Select(x => new ReviewCardDto()
                {
                    Id = x.Id,
                    Definition = x.Definition,
                    Term = x.Term,
                })
                .ToListAsync(cancellationToken);
            return cards;
        }
    }
}
