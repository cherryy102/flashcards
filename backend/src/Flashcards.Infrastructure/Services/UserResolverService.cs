using Flashcards.Application.Common.Interfaces.UserResolver;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Flashcards.Infrastructure.Services
{
    internal class UserResolverService : IUserResolver
    {
        private readonly ClaimsPrincipal principal;

        public UserResolverService(IHttpContextAccessor httpContextAccessor)
        {
            this.principal = httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal();
        }

        private Guid? id;
        private string? email;
        private IEnumerable<string>? roles;

        public Guid Id
        {
            get
            {
                if (!this.id.HasValue)
                {
                    if (!Guid.TryParse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid id))
                    {
                        throw new ArgumentNullException(nameof(this.principal));
                    }
                    this.id = id;
                }
                return id.Value;
            }
        }

        public string Email
        {
            get
            {
                email ??= principal.FindFirst(ClaimTypes.Email)?.Value ?? "";
                return email;
            }
        }

        public IEnumerable<string> Roles
        {
            get
            {
                if (roles == null)
                {
                    var identity = principal.Identity ?? throw new ArgumentNullException(nameof(principal.Identity));
                    roles = ((ClaimsIdentity)identity).Claims
                        .Where(x => x.Type == ClaimTypes.Role)
                        .Select(x => x.Value)
                        .ToList();
                }
                return roles;
            }
        }
    }
}
