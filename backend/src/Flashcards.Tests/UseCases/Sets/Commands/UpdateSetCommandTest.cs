using Flashcards.Application.UseCases.Sets.Commands.Update;
using Flashcards.Domain.Entities;
using Flashcards.Domain.Exceptions;

namespace Flashcards.Tests.UseCases.Sets.Commands
{
    public class UpdateSetCommandTest : BaseTest
    {
        [Fact]
        public async void When_SetExists_Should_UpdateSet()
        {
            //Arrange
            var newSet = new Set()
            {
                Id = 1,
                Name = "Name",
                UserId = Guid.Empty,
                DateAdd = DateTime.UtcNow,
            };
            DbContext.Sets.Add(newSet);
            DbContext.SaveChanges();
            var command = new UpdateSetCommand()
            {
                Id = 1,
                Name = "New name"
            };
            //Act
            await Mediator.Send(command);
            var set = DbContext.Sets.Where(x => x.Id == 1).First();
            //Assert
            Assert.Equal(newSet.Name, set.Name);
        }

        [Fact]
        public async void When_SetNotExists_Should_ThrowNotFoundException()
        {
            //Arrange
            var command = new UpdateSetCommand()
            {
                Id = 1
            };
            //Act
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => Mediator.Send(command));
        }
    }
}
