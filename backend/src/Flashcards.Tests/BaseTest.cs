using Flashcards.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Flashcards.Tests
{
    public abstract class BaseTest
    {
        private readonly IServiceCollection services = DependencyInjection.GetServices();
        private IServiceProvider? serviceProvider;

        protected IMediator Mediator => ServiceProvider.GetRequiredService<IMediator>();
        protected IFlashcardsDbContext DbContext => ServiceProvider.GetRequiredService<IFlashcardsDbContext>();

        protected IServiceProvider ServiceProvider
        {
            get
            {
                serviceProvider ??= services.BuildServiceProvider();
                return serviceProvider;
            }
            set => serviceProvider = value;
        }

        protected IServiceCollection Services { get => services; }
    }

}
