using FluentValidation;

namespace Flashcards.Application.UseCases.Sets.Commands.Update
{
    public class UpdateSetCommandValidator : AbstractValidator<UpdateSetCommand>
    {
        public UpdateSetCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .NotEmpty();
            RuleFor(x => x.Name)
                .NotEmpty();
        }
    }
}
