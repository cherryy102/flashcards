using Flashcards.Application.Common.Interfaces;
using Flashcards.Application.Common.Interfaces.Deepl;
using Flashcards.Application.Common.Interfaces.UserResolver;
using Flashcards.Infrastructure.Authentication;
using Flashcards.Infrastructure.Persistence.Flashcards;
using Flashcards.Infrastructure.Providers.Deepl;
using Flashcards.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Flashcards.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var flashcardsConnectionString = configuration.GetConnectionString("FlashcardsConn");

            services.Configure<DeeplSettings>(configuration.GetSection(DeeplSettings.SectionName));

            services.AddDbContext<IFlashcardsDbContext, FlashcardsDbContext>(options =>
            {
                options.UseNpgsql(flashcardsConnectionString, builder =>
                {
                    builder.MigrationsAssembly(typeof(FlashcardsDbContext).Assembly.FullName);
                    builder.MigrationsHistoryTable("history_migrations", FlashcardsDbContext.DefaultSchema);
                });
            });

            services.AddAuth(configuration);
            services.RegisterOnlyInfrastructureServices();
            services.AddHealthChecks()
                .AddDbContextCheck<FlashcardsDbContext>();

            return services;
        }

        internal static IServiceCollection RegisterOnlyInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IUserResolver, UserResolverService>();
            services.AddScoped<IDeeplProvider, DeeplProvider>();
            return services;
        }

        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var keycloakConf = configuration.GetSection(KeycloakSettings.SectionName).Get<KeycloakSettings>();

            services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = keycloakConf!.Authority;
                    options.MetadataAddress = keycloakConf.MetadataAddress;
                    options.Audience = "account";
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = keycloakConf.ValidateIssuer,
                        IssuerSigningKey = BuildRSAKey(keycloakConf.PublicKey)
                    };
#if DEBUG
                    options.RequireHttpsMetadata = false;
#endif
                    options.SaveToken = true;
                });

            return services;
        }

        private static RsaSecurityKey BuildRSAKey(string publicKeyJWT)
        {
            RSA rsa = RSA.Create();

            rsa.ImportSubjectPublicKeyInfo(

                source: Convert.FromBase64String(publicKeyJWT),
                bytesRead: out _
            );

            var IssuerSigningKey = new RsaSecurityKey(rsa);

            return IssuerSigningKey;
        }
    }
}
