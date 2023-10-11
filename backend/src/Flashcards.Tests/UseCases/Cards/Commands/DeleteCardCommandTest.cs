using Flashcards.Application.UseCases.Cards.Commands.Delete;
using Flashcards.Domain.Entities;
using Flashcards.Domain.Enums;
using Flashcards.Domain.Exceptions;

namespace Flashcards.Tests.UseCases.Cards.Commands
{
    public class DeleteCardCommandTest : BaseTest
    {
        [Fact]
        public async void When_CardExists_Should_DeleteCard()
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
            var command = new DeleteCardCommand()
            {
                Id = 1
            };
            //Act
            await Mediator.Send(command);
            var card = DbContext.Cards.Where(x => x.Id == 1).FirstOrDefault();
            //Assert
            Assert.Null(card);
        }

        [Fact]
        public async void When_CardNotExists_Should_ThrowNotFoundException()
        {
            //Arrange
            var command = new DeleteCardCommand()
            {
               Id = 1
            };
            //Act
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => Mediator.Send(command));
        }
    }
}
