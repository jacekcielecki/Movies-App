using FluentValidation;
using MoviesApi.Models;
using MoviesApp.Models.Dtos;

namespace MoviesApp.Models.Validators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        private readonly MoviesDbContext _context;
        public CreateUserDtoValidator(MoviesDbContext moviesDbContext)
        {
            _context = moviesDbContext;

            RuleFor(x => x.Username).NotEmpty().MinimumLength(6).Custom((value, context) =>
            {
                var usernameTaken = _context.Users.Any(r => r.Username == value);
                if (usernameTaken)
                    context.AddFailure("Username", "This username is taken");
            });
            RuleFor(x => x.Email).NotEmpty().EmailAddress().Custom((value, context) =>
            {
                var emailTaken = _context.Users.Any(r => r.Username == value);
                if (emailTaken)
                    context.AddFailure("Email", "This email is taken");
            }); ;
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
            RuleFor(x => x.ConfirmPassword).Equal(e => e.Password);

        }
    }
}
