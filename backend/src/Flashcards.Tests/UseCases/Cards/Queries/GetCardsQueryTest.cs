using Flashcards.Application.UseCases.Cards.Queries.Get;
using Flashcards.Domain.Entities;

namespace Flashcards.Tests.UseCases.Cards.Queries
{
    public class GetCardsQueryTest : BaseTest
    {
        [Fact]
        public async void When_SetExists_Should_Return_Cards()
        {
            //Arrange
            var sets = new List<Set> {
                new Set()
                {
                    Id = 1,
                    Name = "Name",
                    DateAdd = DateTime.Now,
                    Cards = new List<Card>
                    {
                        new Card() { 
                            Id = 1,
                            Definition = "Def",
                            Term = "Term",
                            Correctness = Domain.Enums.CardCorrectnessEnum.Correct,
                            NextRepeatDate = DateTime.UtcNow,
                            UserId = Guid.Empty,
                            DateAdd = DateTime.UtcNow,
                            SetId = 1,
                            Stage = 0,
                        }
                    },
                    UserId = Guid.Empty,
                },
                new Set()
                {
                    Id = 2,
                    Name = "Name2",
                    DateAdd = DateTime.Now,
                    UserId = Guid.Empty,
                },
            };
            DbContext.Sets.AddRange(sets);
            DbContext.SaveChanges();
            var query = new GetCardsQuery()
            {
                SetId = 1,
            };
            //Act
            var cards = await Mediator.Send(query);
            var card = cards.First();
            //Assert
            Assert.Single(cards);
            Assert.Equal(1, card.SetId);
            Assert.Equal(1, card.Id);
            Assert.Equal("Term", card.Term);
            Assert.Equal("Def", card.Definition);
            Assert.Equal(Domain.Enums.CardCorrectnessEnum.Correct, card.Correctness);
        }

        [Fact]
        public async void When_SetExists_Should_Return_EmptyList()
        {
            //Arrange
            var query = new GetCardsQuery()
            {
                SetId = 1,
            };
            //Act
            var cards = await Mediator.Send(query);
            //Assert
            Assert.Empty(cards);
        }
    }
}
