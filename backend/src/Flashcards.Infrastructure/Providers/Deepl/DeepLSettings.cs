namespace Flashcards.Infrastructure.Providers.Deepl
{
    public class DeeplSettings
    {
        public const string SectionName = "DeepL";

        public string ApiKey { get; set; } = string.Empty;
    }
}
