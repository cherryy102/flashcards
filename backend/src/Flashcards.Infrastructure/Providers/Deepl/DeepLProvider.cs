using DeepL;
using Flashcards.Application.Common.Interfaces.Deepl;
using Flashcards.Domain.Exceptions;
using Microsoft.Extensions.Options;

namespace Flashcards.Infrastructure.Providers.Deepl
{
    internal class DeeplProvider : IDeeplProvider
    {
        private readonly DeeplSettings options;

        public DeeplProvider(IOptions<DeeplSettings> options)
        {
            this.options = options.Value;
        }

        public async Task<string> Translate(string text, string? sourceLanguageCode, string targetLanguageCode)
        {
            try
            {
                var translator = new Translator(options.ApiKey);
                var translatedText = await translator.TranslateTextAsync(text, sourceLanguageCode, targetLanguageCode);
                return translatedText.Text;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
            
        }
    }
}
