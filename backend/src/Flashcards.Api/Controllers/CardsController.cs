using Flashcards.Application.UseCases.Cards.Commands.Add;
using Flashcards.Application.UseCases.Cards.Commands.Delete;
using Flashcards.Application.UseCases.Cards.Commands.SetReview;
using Flashcards.Application.UseCases.Cards.Commands.Update;
using Flashcards.Application.UseCases.Cards.Queries.Get;
using Flashcards.Application.UseCases.Cards.Queries.Get.Dtos;
using Flashcards.Application.UseCases.Cards.Queries.GetReviews;
using Flashcards.Application.UseCases.Cards.Queries.GetReviews.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flashcards.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route($"{Configurations.ApiUrl}/v{{version:apiVersion}}/cards")]
    [Authorize]
    public class CardsController : ControllerBase
    {
        private readonly IMediator mediator;

        public CardsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Add([FromBody] AddCardCommand addCard)
        {
            await mediator.Send(addCard);
            return Ok();
        }

        [HttpPut]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Update([FromBody] UpdateCardCommand updateCard)
        {
            await mediator.Send(updateCard);
            return Ok();
        }

        [HttpPut("review")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SetReview([FromBody] SetCardReviewCommand setRepeat)
        {
            await mediator.Send(setRepeat);
            return Ok();
        }

        [HttpGet("{setId}")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<IEnumerable<CardDto>>> Get(int setId)
        {
            var query = new GetCardsQuery()
            {
                SetId = setId,
            };
            var cardsDto = await mediator.Send(query); 
           
            return Ok(cardsDto);
        }

        [HttpGet("reviews")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<IEnumerable<ReviewCardDto>>> GetReviews()
        {
            var query = new GetCardsReviewsQuery();
            var cardsDto = await mediator.Send(query);
            return Ok(cardsDto);
        }

        [HttpDelete("{id}")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteCardCommand()
            {
                Id = id,
            };
            await mediator.Send(command);
            return Ok();
        }
    }
}
