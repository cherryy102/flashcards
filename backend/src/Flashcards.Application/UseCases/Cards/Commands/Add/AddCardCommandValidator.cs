using FluentValidation;

namespace Flashcards.Application.UseCases.Cards.Commands.Add
{
    public class AddCardCommandValidator : AbstractValidator<AddCardCommand>
    {
        public AddCardCommandValidator()
        {
            RuleFor(x => x.SetId)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.Definition)
                .NotEmpty();
            RuleFor(x => x.Term)
               .NotEmpty();
        }
    }
}
