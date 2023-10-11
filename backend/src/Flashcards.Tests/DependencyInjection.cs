using Flashcards.Application;
using Flashcards.Application.Common.Interfaces;
using Flashcards.Application.Common.Interfaces.UserResolver;
using Flashcards.Infrastructure;
using Flashcards.Infrastructure.Persistence.Flashcards;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Flashcards.Tests
{
    internal static class DependencyInjection
    {
        public static IServiceCollection GetServices()
        {
            var services = new ServiceCollection();

            services.RegisterOnlyInfrastructureServices();
            services.AddApplication();
            services.AddScoped<IUserResolver, TestUserService>();

            services.RegisterMemoryDbContext();

            return services;
        }

        private static IServiceCollection RegisterMemoryDbContext(this IServiceCollection services)
        {
            var dbContextOptions = new DbContextOptionsBuilder<FlashcardsDbContext>();
            dbContextOptions.UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new FlashcardsDbContext(dbContextOptions.Options);
            dbContext.Database.EnsureDeleted();

            services.AddSingleton<IFlashcardsDbContext>(dbContext);
            return services;
        }

    }
}
