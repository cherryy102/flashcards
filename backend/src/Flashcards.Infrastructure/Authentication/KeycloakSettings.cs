namespace Flashcards.Infrastructure.Authentication
{
    public class KeycloakSettings
    {
        public const string SectionName = "Keycloak";

        public string Authority { get; set; } = string.Empty;
        public string MetadataAddress { get; set; } = string.Empty;
        public string ValidateIssuer { get; set; } = string.Empty;
        public string PublicKey { get; set; } = string.Empty;
    }
}
