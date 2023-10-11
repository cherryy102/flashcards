using Flashcards.Application.UseCases.Sets.Commands.Delete;
using Flashcards.Domain.Entities;
using Flashcards.Domain.Exceptions;

namespace Flashcards.Tests.UseCases.Sets.Commands
{
    public class DeleteSetCommandTest : BaseTest
    {
        [Fact]
        public async void When_SetExists_Should_DeleteCard()
        {
            //Arrange
            var set = new Set()
            {
                Id = 1,
                UserId = Guid.Empty,
                DateAdd = DateTime.UtcNow,
                Name= "Test",
            };
            DbContext.Sets.Add(set);
            DbContext.SaveChanges();
            var command = new DeleteSetCommand()
            {
                Id = 1
            };
            //Act
            await Mediator.Send(command);
            var card = DbContext.Sets.Where(x => x.Id == 1).FirstOrDefault();
            //Assert
            Assert.Null(card);
        }

        [Fact]
        public async void WhenSetNotExists_Should_ThrowNotFoundException()
        {
            //Arrange
            var command = new DeleteSetCommand()
            {
                Id = 1
            };
            //Act
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => Mediator.Send(command));
        }
    }
}
