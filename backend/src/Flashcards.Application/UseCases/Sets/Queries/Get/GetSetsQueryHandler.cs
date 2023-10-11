using Flashcards.Application.Common.Interfaces;
using Flashcards.Application.Common.Interfaces.UserResolver;
using Flashcards.Application.UseCases.Sets.Queries.Get.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Application.UseCases.Sets.Queries.Get
{
    internal class GetSetsQueryHandler : IRequestHandler<GetSetsQuery, IEnumerable<SetDto>>
    {
        private readonly IUserResolver userResolver;
        private readonly IFlashcardsDbContext dbContext;

        public GetSetsQueryHandler(IUserResolver userResolver, IFlashcardsDbContext dbContext)
        {
            this.userResolver = userResolver;
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<SetDto>> Handle(GetSetsQuery request, CancellationToken cancellationToken)
        {
            var sets = await dbContext.Sets
                .AsNoTracking()
                .Where(x => x.UserId == userResolver.Id)
                .Select(x => new SetDto
                {
                    Id = x.Id,
                    Name = x.Name,
                })
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);
            return sets;
        }
    }
}
