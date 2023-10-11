using Flashcards.Application.Common.Interfaces;
using Flashcards.Application.Common.Interfaces.UserResolver;
using Flashcards.Domain.Exceptions;
using MediatR;

namespace Flashcards.Application.UseCases.Sets.Commands.Delete
{
    internal class DeleteSetCommandHandler : IRequestHandler<DeleteSetCommand>
    {
        private readonly IUserResolver userResolver;
        private readonly IFlashcardsDbContext dbContext;

        public DeleteSetCommandHandler(IUserResolver userResolver, IFlashcardsDbContext dbContext)
        {
            this.userResolver = userResolver;
            this.dbContext = dbContext;
        }

        public Task Handle(DeleteSetCommand request, CancellationToken cancellationToken)
        {
            var set = dbContext.Sets.FirstOrDefault(x => x.Id == request.Id && x.UserId == userResolver.Id);
            if (set is null)
            {
                throw new NotFoundException("Set does not exists");
            }
            dbContext.Sets.Remove(set);
            dbContext.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
