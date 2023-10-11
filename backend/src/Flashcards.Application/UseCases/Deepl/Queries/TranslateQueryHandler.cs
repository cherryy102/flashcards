using Flashcards.Application.Common.Interfaces.Deepl;
using Flashcards.Application.UseCases.Deepl.Queries.Dtos;
using MediatR;

namespace Flashcards.Application.UseCases.Deepl.Queries
{
    internal class TranslateQueryHandler : IRequestHandler<TranslateQuery, TranslateResultDto>
    {
        private readonly IDeeplProvider deeplProvider;

        public TranslateQueryHandler(IDeeplProvider deeplProvider)
        {
            this.deeplProvider = deeplProvider;
        }

        public async Task<TranslateResultDto> Handle(TranslateQuery request, CancellationToken cancellationToken)
        {
            var text = await deeplProvider.Translate(request.Text, request.SourceLanguageCode, request.TargetLanguageCode);
            var translateDto = new TranslateResultDto(text);
            return translateDto;
        }
    }
}
