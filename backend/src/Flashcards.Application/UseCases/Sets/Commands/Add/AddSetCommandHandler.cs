using Flashcards.Application.Common.Interfaces;
using Flashcards.Application.Common.Interfaces.UserResolver;
using MediatR;

namespace Flashcards.Application.UseCases.Sets.Commands.Add
{
    internal class AddSetCommandHandler : IRequestHandler<AddSetCommand>
    {
        private readonly IUserResolver userResolver;
        private readonly IFlashcardsDbContext dbContext;

        public AddSetCommandHandler(IUserResolver userResolver, IFlashcardsDbContext dbContext)
        {
            this.userResolver = userResolver;
            this.dbContext = dbContext;
        }

        public Task Handle(AddSetCommand request, CancellationToken cancellationToken)
        {
            var set = new Domain.Entities.Set()
            {
                Name = request.Name,
                DateAdd = DateTime.UtcNow,
                UserId = userResolver.Id,
            };
            dbContext.Sets.Add(set);
            dbContext.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
