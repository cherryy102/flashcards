using Flashcards.Application.UseCases.Sets.Queries.GetWithCards;
using Flashcards.Domain.Entities;
using Flashcards.Domain.Exceptions;

namespace Flashcards.Tests.UseCases.Sets.Queries
{
    public class GetSetWithCardsQueryTest : BaseTest
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
                Cards =  new List<Card>() { 
                    new Card()
                    {
                        Id = 1,
                        Correctness = Domain.Enums.CardCorrectnessEnum.Correct,
                        DateAdd = DateTime.UtcNow,
                        Definition = "Def",
                        SetId = 1,
                        NextRepeatDate = DateTime.UtcNow,
                        Stage = 0,
                        Term = "Term",
                        UserId = Guid.Empty,
                    }
                }
            };
            DbContext.Sets.Add(newSet);
            DbContext.SaveChanges();
            var query = new GetSetWithCardsQuery()
            {
                SetId = 1
            };
            //Act
            var set = await Mediator.Send(query);
            var card = set.Cards.First();
            //Assert
            Assert.Single(set.Cards);
            Assert.Equal(newSet.Id, set.Id);
            Assert.Equal(newSet.Name, set.Name);
            Assert.Equal("Term", card.Term);
            Assert.Equal("Def", card.Definition);
            Assert.Equal(1, card.Id);
        }

        [Fact]
        public async void When_SetsNotExists_Should_ThrowNotFoundException()
        {
            //Arrange
            var query = new GetSetWithCardsQuery()
            {
                SetId = 1
            };
            //Act
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => Mediator.Send(query));
        }
    }
}
