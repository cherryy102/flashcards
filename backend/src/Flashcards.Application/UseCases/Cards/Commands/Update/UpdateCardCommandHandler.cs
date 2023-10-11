using Flashcards.Application.Common.Interfaces;
using Flashcards.Application.Common.Interfaces.UserResolver;
using Flashcards.Domain.Exceptions;
using MediatR;

namespace Flashcards.Application.UseCases.Cards.Commands.Update
{
    internal class UpdateCardCommandHandler : IRequestHandler<UpdateCardCommand>
    {
        private readonly IUserResolver userResolver;
        private readonly IFlashcardsDbContext dbContext;

        public UpdateCardCommandHandler(IUserResolver userResolver, IFlashcardsDbContext dbContext)
        {
            this.userResolver = userResolver;
            this.dbContext = dbContext;
        }

        public Task Handle(UpdateCardCommand request, CancellationToken cancellationToken)
        {
            var card = dbContext.Cards.FirstOrDefault(x => x.Id == request.Id && x.UserId == userResolver.Id);
            if (card is null)
            {
                throw new NotFoundException("Card does not exist");
            }
            if (request.Term is not null)
            {
                card.Term = request.Term;
            }
            if (request.Definition is not null)
            {
                card.Definition = request.Definition;
            }
            if (request.Correctness is not null)
            {
                card.Correctness = (Domain.Enums.CardCorrectnessEnum)request.Correctness;
            }
            dbContext.Cards.Update(card);
            dbContext.SaveChanges();

            return Task.CompletedTask;
        }
    }
}
