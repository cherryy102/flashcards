using Flashcards.Application.UseCases.Cards.Commands.Add;
using Flashcards.Domain.Entities;
using Flashcards.Domain.Enums;
using Flashcards.Domain.Exceptions;

namespace Flashcards.Tests.UseCases.Cards.Commands
{
    public class AddCardCommandTest : BaseTest
    {

        [Fact]
        public async void When_SetExists_Should_AddCard()
        {
            //Arrange
            var set = new Set()
            {
                Id = 1,
                Name = "Test",
                UserId = Guid.Empty,
                DateAdd = DateTime.UtcNow,
            };
            DbContext.Sets.Add(set);    
            DbContext.SaveChanges();
            var command = new AddCardCommand()
            {
                Definition = "Def",
                Term = "Term",
                SetId = 1,
            };
            //Act
            await Mediator.Send(command);
            var card = DbContext.Cards.First();
            //Assert
            Assert.Equal("Term", card.Term);
            Assert.Equal("Def", card.Definition);
            Assert.Equal(0, card.Stage);
            Assert.Equal(CardCorrectnessEnum.Correct, card.Correctness);
            Assert.Equal(DateTime.UtcNow.Date, card.NextRepeatDate!.Value.Date);
        }

        [Fact]
        public async void When_SetNotExists_Should_ThrowNotFoundException()
        {
            //Arrange
            var command = new AddCardCommand()
            {
                Definition = "Def",
                Term = "Term",
                SetId = 1,
            };
            //Act
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => Mediator.Send(command));
        }
    }
}
