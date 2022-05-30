using FluentValidation;
using MoviesApi.Models.Dtos;

namespace MoviesApi.Models.Validators
{
    public class CreateMovieDtoValidator : AbstractValidator<CreateMovieDto>
    {
        private readonly MoviesDbContext _context;
        public CreateMovieDtoValidator(MoviesDbContext moviesDbContext)
        {
            _context = moviesDbContext;

                RuleFor(x => x.Title_pl).NotEmpty().Length(3, 40);
                RuleFor(x => x.Title_en).NotEmpty().Length(3, 40);
                RuleFor(x => x.Description).NotEmpty().MaximumLength(500).WithMessage("Description is required");
                //RuleFor(x => x.Genre);//.NotEmpty().WithMessage("Genre must be specified");
                RuleFor(x => x.Director).NotEmpty().Length(3, 20);
                RuleFor(x => x.Screenwriter).NotEmpty().Length(3, 20);
                RuleFor(x => x.Country).Length(0, 20);
                //RuleFor(x => x.Premiere);
                RuleFor(x => x.Lenght).NotEmpty().LessThanOrEqualTo(300);
                //RuleFor(x => x.Image).Custom((value, context) =>
                //{
                //    if (string.IsNullOrEmpty(value))
                //    {
                //        value = "https://upload.wikimedia.org/wikipedia/commons/f/fc/No_picture_available.png";
                //    }
                //});
                //RuleFor(x => x.Rates);
                //RuleFor(x => x.Comments);
        }
    }
}
