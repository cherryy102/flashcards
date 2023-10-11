using Flashcards.Application.Common.Interfaces;
using Flashcards.Application.Common.Interfaces.UserResolver;
using Flashcards.Domain.Exceptions;
using MediatR;

namespace Flashcards.Application.UseCases.Sets.Commands.Update
{
    internal class UpdateSetCommandHandler : IRequestHandler<UpdateSetCommand>
    {
        private readonly IUserResolver userResolver;
        private readonly IFlashcardsDbContext dbContext;

        public UpdateSetCommandHandler(IUserResolver userResolver, IFlashcardsDbContext dbContext)
        {
            this.userResolver = userResolver;
            this.dbContext = dbContext;
        }

        public Task Handle(UpdateSetCommand request, CancellationToken cancellationToken)
        {
            var set = dbContext.Sets.FirstOrDefault(x => x.Id == request.Id && x.UserId == userResolver.Id);
            if (set is null)
            {
                throw new NotFoundException("Set does not exists");
            }
            set.Name = request.Name;
            dbContext.Sets.Update(set);
            dbContext.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
