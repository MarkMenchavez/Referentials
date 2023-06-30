namespace Referentials.Validators;

using FluentValidation;
using Referentials.ViewModels;

public class PageOptionsValidator : AbstractValidator<PageOptions>
{
    public PageOptionsValidator()
    {
        this.RuleFor(x => x.First).InclusiveBetween(1, 20);
        this.RuleFor(x => x.Last).InclusiveBetween(1, 20);
    }
}
