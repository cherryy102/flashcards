using Flashcards.Application.Common.Interfaces;
using Flashcards.Application.Common.Interfaces.UserResolver;
using Flashcards.Domain.Entities;
using Flashcards.Domain.Exceptions;
using MediatR;

namespace Flashcards.Application.UseCases.Cards.Commands.Add
{
    internal class AddCardCommandHandler : IRequestHandler<AddCardCommand>
    {
        private readonly IUserResolver userResolver;
        private readonly IFlashcardsDbContext dbContext;

        public AddCardCommandHandler(IUserResolver userResolver, IFlashcardsDbContext dbContext)
        {
            this.userResolver = userResolver;
            this.dbContext = dbContext;
        }

        public Task Handle(AddCardCommand request, CancellationToken cancellationToken)
        {
            var set = dbContext.Sets.FirstOrDefault(x => x.Id == request.SetId && x.UserId == userResolver.Id);
            if (set is null)
            {
                throw new NotFoundException("Set does not exists");
            }
            var card = new Card()
            {
                Definition = request.Definition,
                SetId = request.SetId,
                Term = request.Term,
                Correctness = Domain.Enums.CardCorrectnessEnum.Correct,
                UserId = userResolver.Id,
                DateAdd = DateTime.UtcNow,
                Stage = 0,
                NextRepeatDate = DateTime.UtcNow,
            };
            dbContext.Cards.Add(card);
            dbContext.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
