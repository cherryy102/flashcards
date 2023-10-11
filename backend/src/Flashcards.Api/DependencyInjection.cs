using Flashcards.Api.Middleware;
using Flashcards.Application.UseCases.Cards.Commands.Add;
using Flashcards.Application.UseCases.Cards.Commands.Update;
using Flashcards.Application.UseCases.Sets.Commands.Add;
using Flashcards.Application.UseCases.Sets.Commands.Update;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace Flashcards.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiVersioningConfiguration(this IServiceCollection services)
        {
            services.AddApiVersioning(options => {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("Accept-Version"),
                    new MediaTypeApiVersionReader("api-version")
                );
            });
            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<AddSetCommand>, AddSetCommandValidator>(); ;
            services.AddScoped<IValidator<UpdateSetCommand>, UpdateSetCommandValidator>();
            services.AddScoped<IValidator<AddCardCommand>, AddCardCommandValidator>();
            services.AddScoped<IValidator<UpdateCardCommand>, UpdateCardCommandValidator>();
            services.AddScoped<ErrorHandlingMiddleware>();

            return services;
        }
    }

}
