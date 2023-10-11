using Flashcards.Application.UseCases.Cards.Queries.Get;
using Flashcards.Application.UseCases.Cards.Queries.GetReviews;
using Flashcards.Domain.Entities;

namespace Flashcards.Tests.UseCases.Cards.Queries
{
    public class GetCardsReviewsQueryTest : BaseTest
    {
        [Fact]
        public async void When_NextRepeatDateAreLessOrEqualCurrentDate_Should_Return_Cards()
        {
            //Arrange
            var set = new Set()
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
                            NextRepeatDate = DateTime.UtcNow.AddDays(2),
                            UserId = Guid.Empty,
                            DateAdd = DateTime.UtcNow,
                            SetId = 1,
                            Stage = 0,
                        },
                        new Card() {
                            Id = 2,
                            Definition = "Def2",
                            Term = "Term2",
                            Correctness = Domain.Enums.CardCorrectnessEnum.Correct,
                            NextRepeatDate = DateTime.UtcNow.AddDays(-1),
                            UserId = Guid.Empty,
                            DateAdd = DateTime.UtcNow,
                            SetId = 1,
                            Stage = 0,
                        },
                         new Card() {
                            Id = 3,
                            Definition = "Def3",
                            Term = "Term3",
                            Correctness = Domain.Enums.CardCorrectnessEnum.Correct,
                            NextRepeatDate = DateTime.UtcNow,
                            UserId = Guid.Empty,
                            DateAdd = DateTime.UtcNow,
                            SetId = 1,
                            Stage = 0,
                        }
                    },
                UserId = Guid.Empty,
            };
            DbContext.Sets.Add(set);
            DbContext.SaveChanges();
            var query = new GetCardsReviewsQuery();
            //Act
            var cards = await Mediator.Send(query);
            var card = cards.First(x => x.Id == 2);
            //Assert
            Assert.Equal(2, cards.Count());
            Assert.Equal("Term2", card.Term);
            Assert.Equal("Def2", card.Definition);
        }

        [Fact]
        public async void When_NextRepeatDateAreGreaterThanCurrentDate_Should_Return_EmptyCards()
        {
            //Arrange
            var query = new GetCardsReviewsQuery();
            //Act
            var cards = await Mediator.Send(query);
            //Assert
            Assert.Empty(cards);
        }
    }
}
