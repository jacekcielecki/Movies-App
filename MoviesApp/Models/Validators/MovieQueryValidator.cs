using FluentValidation;

namespace MoviesApp.Models.Validators
{
    public class MovieQueryValidator : AbstractValidator<MovieQuery>
    {
        private int[] allowedPageSizes = new int[] { 6, 9, 12 };
        public MovieQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).Custom((value, context) =>
            {
                if (!allowedPageSizes.Contains(value))
                    context.AddFailure("PageSize", $"PageSize must be in [{string.Join(",", allowedPageSizes)}]");
            });
        }
    }
}
