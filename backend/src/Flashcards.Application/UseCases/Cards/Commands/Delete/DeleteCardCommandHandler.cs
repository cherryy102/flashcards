using Flashcards.Application.Common.Interfaces;
using Flashcards.Application.Common.Interfaces.UserResolver;
using Flashcards.Domain.Exceptions;
using MediatR;

namespace Flashcards.Application.UseCases.Cards.Commands.Delete
{
    internal class DeleteCardCommandHandler : IRequestHandler<DeleteCardCommand>
    {
        private readonly IUserResolver userResolver;
        private readonly IFlashcardsDbContext dbContext;

        public DeleteCardCommandHandler(IUserResolver userResolver, IFlashcardsDbContext dbContext)
        {
            this.userResolver = userResolver;
            this.dbContext = dbContext;
        }

        public Task Handle(DeleteCardCommand request, CancellationToken cancellationToken)
        {
            var card = dbContext.Cards.FirstOrDefault(x => x.Id == request.Id && x.UserId == userResolver.Id);
            if (card is null)
            {
                throw new NotFoundException("Card does not exist");
            }
            dbContext.Cards.Remove(card);
            dbContext.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
