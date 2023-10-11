using Flashcards.Application.Common.Interfaces.UserResolver;

namespace Flashcards.Tests
{
    public class TestUserService : IUserResolver
    {
        public Guid Id => Guid.Empty;

        public string Email => "test@test.com";

        public IEnumerable<string> Roles => Enumerable.Empty<string>();
    }
}
