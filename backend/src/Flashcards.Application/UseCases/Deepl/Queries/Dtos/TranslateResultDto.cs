namespace Flashcards.Application.UseCases.Deepl.Queries.Dtos
{
    public class TranslateResultDto
    {
        public TranslateResultDto(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }
}
