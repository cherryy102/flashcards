using Flashcards.Application.Common.Interfaces;
using Flashcards.Application.Common.Interfaces.UserResolver;
using Flashcards.Domain.Enums;
using Flashcards.Domain.Exceptions;
using MediatR;

namespace Flashcards.Application.UseCases.Cards.Commands.SetReview
{
    internal class SetCardReviewCommandHandler : IRequestHandler<SetCardReviewCommand>
    {
        private readonly IFlashcardsDbContext dbContext;
        private readonly IUserResolver userResolver;

        public SetCardReviewCommandHandler(IFlashcardsDbContext dbContext, IUserResolver userResolver)
        {
            this.dbContext = dbContext;
            this.userResolver = userResolver;
        }

        public Task Handle(SetCardReviewCommand request, CancellationToken cancellationToken)
        {
            var card = dbContext.Cards.FirstOrDefault(x => x.Id == request.CardId && x.UserId == userResolver.Id);
            if (card is null)
            {
                throw new NotFoundException("Card does not exist");
            }
            if (request.Correctness == CardCorrectnessEnum.InCorrect)
            {
                card.Stage = 0;
                card.NextRepeatDate = GetNextRepeatDate(card.Stage);
            }
            else
            {
                card.Stage += 1;
                card.NextRepeatDate = GetNextRepeatDate(card.Stage);
            }
            dbContext.Cards.Update(card);
            dbContext.SaveChanges();

            return Task.CompletedTask;
        }

        private static DateTime GetNextRepeatDate(int stage)
        {
            var days = Math.Pow(stage, 2) - stage + 1;
            return DateTime.UtcNow.AddDays(days);
        }
    }
}
