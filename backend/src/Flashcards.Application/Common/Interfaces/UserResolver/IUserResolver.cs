namespace Flashcards.Application.Common.Interfaces.UserResolver
{
    public interface IUserResolver
    {
        public Guid Id { get; }
        public string Email { get; }
        public IEnumerable<string> Roles { get; }
    }
}
