using Flashcards.Application.UseCases.Deepl.Queries;
using Flashcards.Application.UseCases.Deepl.Queries.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flashcards.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route($"{Configurations.ApiUrl}/v{{version:apiVersion}}/translate")]
    [Authorize]
    public class TranslateController : ControllerBase
    {
        private readonly IMediator mediator;

        public TranslateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<TranslateResultDto>> TranslateV1([FromBody] TranslateQuery query)
        {
            var translateDto = await mediator.Send(query);
            return Ok(translateDto);
        }
    }
}
