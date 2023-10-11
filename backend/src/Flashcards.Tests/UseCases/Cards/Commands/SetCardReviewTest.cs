using Flashcards.Application.UseCases.Cards.Commands.SetReview;
using Flashcards.Domain.Entities;
using Flashcards.Domain.Enums;
using Flashcards.Domain.Exceptions;
using System.Data;

namespace Flashcards.Tests.UseCases.Cards.Commands
{
    public class SetCardReviewTest : BaseTest
    {
        public static readonly object[][] CorrectData =
        {
          new object[] { CardCorrectnessEnum.InCorrect, 0, 0, DateTime.UtcNow.AddDays(1)},
          new object[] { CardCorrectnessEnum.Correct, 1, 2, DateTime.UtcNow.AddDays(3) },
          new object[] { CardCorrectnessEnum.InCorrect, 2, 0, DateTime.UtcNow.AddDays(1) }
        };

        [Theory, MemberData(nameof(CorrectData))]
        public async void When_CardExists_Should_UpdateCardCorrectness(CardCorrectnessEnum newCorrectness, int stage, int newStage, DateTime nextRepeatDate)
        {
            //Arrange
            var set = new Card()
            {
                Id = 1,
                Definition = "Test",
                Correctness = CardCorrectnessEnum.Correct,
                NextRepeatDate = DateTime.UtcNow,
                Stage = stage,
                SetId = 1,
                Term = "Term",
                UserId = Guid.Empty,
                DateAdd = DateTime.UtcNow,
            };
            DbContext.Cards.Add(set);
            DbContext.SaveChanges();
            var command = new SetCardReviewCommand()
            {
                Correctness = newCorrectness,
                CardId = 1,
            };
            //Act
            await Mediator.Send(command);
            var card = DbContext.Cards.Where(x => x.Id == 1).First();
            //Assert
            Assert.Equal(newStage, card.Stage);
            Assert.Equal(nextRepeatDate.Date, card.NextRepeatDate!.Value.Date);
        }

        [Fact]
        public async void When_CardNotExists_Should_ThrowNotFoundException()
        {
            //Arrange
            var command = new SetCardReviewCommand()
            {
                Correctness = CardCorrectnessEnum.Correct,
                CardId = 1,
            };
            //Act
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => Mediator.Send(command));
        }
    }
}
