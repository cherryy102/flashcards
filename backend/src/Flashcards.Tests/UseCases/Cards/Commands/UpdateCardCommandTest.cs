using Flashcards.Application.UseCases.Cards.Commands.Update;
using Flashcards.Domain.Entities;
using Flashcards.Domain.Enums;
using Flashcards.Domain.Exceptions;

namespace Flashcards.Tests.UseCases.Cards.Commands
{
    public class UpdateCardCommandTest : BaseTest
    {
        [Fact]
        public async void When_CardExists_Should_UpdateCard()
        {
            //Arrange
            var set = new Card()
            {
                Id = 1,
                Definition = "Test",
                Correctness = CardCorrectnessEnum.Correct,
                NextRepeatDate = DateTime.UtcNow,
                Stage = 0,
                SetId = 1,
                Term = "Term",
                UserId = Guid.Empty,
                DateAdd = DateTime.UtcNow,
            };
            DbContext.Cards.Add(set);
            DbContext.SaveChanges();
            var command = new UpdateCardCommand()
            {
                Id = 1,
                Definition = "Test2",
                Term = "Term2",
                Correctness = CardCorrectnessEnum.InCorrect
            };
            //Act
            await Mediator.Send(command);
            var card = DbContext.Cards.Where(x => x.Id == 1).First();
            //Assert
            Assert.Equal("Test2", card.Definition);
            Assert.Equal("Term2", card.Term);
            Assert.Equal(CardCorrectnessEnum.InCorrect, card.Correctness);
        }

        [Fact]
        public async void When_CardNotExists_Should_ThrowNotFoundException()
        {
            //Arrange
            var command = new UpdateCardCommand()
            {
                Id = 1
            };
            //Act
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => Mediator.Send(command));
        }
    }
}
