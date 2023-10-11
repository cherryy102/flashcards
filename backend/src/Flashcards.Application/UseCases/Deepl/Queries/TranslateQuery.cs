using Flashcards.Application.UseCases.Deepl.Queries.Dtos;
using MediatR;

namespace Flashcards.Application.UseCases.Deepl.Queries
{
    public class TranslateQuery : IRequest<TranslateResultDto>
    {
        public string Text { get; set; } = string.Empty;
        public string? SourceLanguageCode { get; set; }
        public string TargetLanguageCode { get; set; } = string.Empty;
    }
}
