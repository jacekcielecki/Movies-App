using FluentValidation;
using MoviesApp.Models.Dtos;

namespace MoviesApp.Models.Validators
{
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            RuleFor(x => x.OldPassword).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6);
            RuleFor(x => x.RepeatNewPassword).Equal(x => x.NewPassword);
        }
    }
}
