using Flashcards.Application.UseCases.Sets.Commands.Add;

namespace Flashcards.Tests.UseCases.Sets.Commands
{
    public class AddSetCommandTest : BaseTest
    {
        [Fact]
        public async void When_PassName_Should_AddSet()
        {
            //Arrange
            var command = new AddSetCommand()
            {
                Name = "Set",
            };
            //Act
            await Mediator.Send(command);
            var set = DbContext.Sets.First();
            //Assert
            Assert.Equal(command.Name, set.Name);
            Assert.Equal(Guid.Empty, set.UserId);
        }
    }
}
