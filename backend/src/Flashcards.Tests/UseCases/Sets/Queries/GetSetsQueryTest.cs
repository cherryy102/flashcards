using Flashcards.Application.UseCases.Sets.Queries.Get;
using Flashcards.Domain.Entities;

namespace Flashcards.Tests.UseCases.Sets.Queries
{
    public class GetSetsQueryTest : BaseTest
    {
        [Fact]
        public async void When_SetsExists_Should_ReturnSets()
        {
            //Arrange
            var newSet = new Set()
            {
                Id = 1,
                Name = "Name",
                DateAdd = DateTime.UtcNow,
                UserId = Guid.Empty,
            };
            DbContext.Sets.Add(newSet);
            DbContext.SaveChanges();
            var query = new GetSetsQuery();
            //Act
            var sets = await Mediator.Send(query);
            var set = sets.First();
            //Assert
            Assert.Single(sets);
            Assert.Equal(newSet.Id, set.Id);
            Assert.Equal(newSet.Name, set.Name);
        }

        [Fact]
        public async void When_SetsNotExists_Should_ReturnEmptyList()
        {
            //Arrange
            var query = new GetSetsQuery();
            //Act
            var sets = await Mediator.Send(query);
            //Assert
            Assert.Empty(sets);
        }
    }
}
