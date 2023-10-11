using FluentValidation;

namespace Flashcards.Application.UseCases.Sets.Commands.Add
{
    public class AddSetCommandValidator : AbstractValidator<AddSetCommand>
    {
        public AddSetCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty();
        }
    }
}
