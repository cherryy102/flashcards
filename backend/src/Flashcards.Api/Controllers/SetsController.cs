using Flashcards.Application.UseCases.Sets.Commands.Add;
using Flashcards.Application.UseCases.Sets.Commands.Delete;
using Flashcards.Application.UseCases.Sets.Commands.Update;
using Flashcards.Application.UseCases.Sets.Queries.Get;
using Flashcards.Application.UseCases.Sets.Queries.GetWithCards;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flashcards.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route($"{Configurations.ApiUrl}/v{{version:apiVersion}}/sets")]
    [Authorize]
    public class SetsController : ControllerBase
    {
        private readonly IMediator mediator;

        public SetsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Add([FromBody] AddSetCommand addSet)
        {
            await mediator.Send(addSet);
            return Ok();
        }

        [HttpPut]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Update([FromBody] UpdateSetCommand update)
        {
            await mediator.Send(update);
            return Ok();
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<IEnumerable<Application.UseCases.Sets.Queries.Get.Dtos.SetDto>>> Get()
        {
            var query = new GetSetsQuery();
            var setsDto = await mediator.Send(query);
            return Ok(setsDto);
        }

        [HttpGet("with-cards/{setId}")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<Application.UseCases.Sets.Queries.GetWithCards.Dtos.SetDto>> GetWithCards(int setId)
        {
            var query = new GetSetWithCardsQuery()
            {
                SetId = setId,
            };
            var setDto = await mediator.Send(query);
            return Ok(setDto);
        }

        [HttpDelete("{id}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteSetCommand() 
            {
                Id = id,
            };
            await mediator.Send(command);
            return Ok();
        }
    }
}
