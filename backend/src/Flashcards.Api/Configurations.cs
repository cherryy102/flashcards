namespace Flashcards.Api
{
    public static class Configurations
    {
        public const string ApiUrl = "/api/flashcards";
        public const string HealthUrl = $"{ApiUrl}/health";

        public static IHostBuilder ConfigureAppConfiguration(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureAppConfiguration((context, config) => {
                var env = context.HostingEnvironment;
                config
                    .AddJsonFile("appsettings.json", optional: false)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                    .AddEnvironmentVariables();
            });
        }
    }
}
