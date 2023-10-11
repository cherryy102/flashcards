namespace Flashcards.Application.Common.Interfaces.Deepl
{
    public interface IDeeplProvider
    {
        Task<string> Translate(string text, string? sourceLanguageCode, string targetLanguageCode);
    }
}
