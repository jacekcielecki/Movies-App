using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Models;
using MoviesApi.Models.Dtos;
using MoviesApi.Exceptions;
using System.Linq;
using MoviesApp.Models;
using MoviesApp.Exceptions;

namespace MoviesApi.Services
{
    public interface IMovieService
    {
        int AddMovie(CreateMovieDto newMovie);
        IEnumerable<MovieDto> Get();
        PagedResult<MovieDto> GetPage(MovieQuery query);
        public MovieDto GetById(int id);
        void Update(int movieId, CreateMovieDto newMovie);
        void Delete(int movieId);
    }

    public class MovieService : IMovieService
    {
        private readonly MoviesDbContext _context;
        private readonly IMapper _mapper;
        public MovieService(MoviesDbContext moviesDbContext, IMapper autoMapper)
        {
            _context = moviesDbContext;
            _mapper = autoMapper;
        }

        public int AddMovie(CreateMovieDto createMovieDto)
        {
            var movie = _mapper.Map<Movie>(createMovieDto); 
            _context.Movies.Add(movie);
            _context.SaveChanges();
            return createMovieDto.Id;
        }

        public IEnumerable<MovieDto> Get()
        {
            var movies = _context.Movies
                .Include(r => r.Rates)
                .Include(r => r.Comments)
                .OrderBy(d => d.Id);

            var movieDtos = _mapper.Map<IEnumerable<MovieDto>>(movies);
            return movieDtos;
        }

        public PagedResult<MovieDto> GetPage(MovieQuery query)
        {
            var movies = _context.Movies
                .Include(r => r.Rates)
                .Include(r => r.Comments)
                .OrderBy(d => d.Id);
            var selectedMovies = movies
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize);

            if (query.PageNumber < 1 || query.PageNumber > 10)
                throw new PageOutOfRangeException("Page Number out of range");

            var movieDtos = _mapper.Map<IEnumerable<MovieDto>>(selectedMovies);
            var result = new PagedResult<MovieDto>(movieDtos, movies.Count(), query.PageSize, query.PageNumber);

            return result;
        }

        public MovieDto GetById(int movieId)
        {
            var movie = GetMovie(movieId);

            var movieDto = _mapper.Map<MovieDto>(movie);
            return movieDto;
        }

        public void Delete(int movieId)
        {
            var movie = GetMovie(movieId);

            _context.Movies.Remove(movie);
            _context.SaveChanges();
            return;
        }

        public void Update(int movieId, CreateMovieDto createMovieDto)
        {
            var movie = GetMovie(movieId);

            movie.Id = createMovieDto.Id;
            movie.Title_en = createMovieDto.Title_en;
            movie.Director = createMovieDto.Director;
            movie.Rates = createMovieDto.Rates;
            movie.Comments = createMovieDto.Comments;
            movie.Image = createMovieDto.Image;
            movie.Country = createMovieDto.Country;
            movie.Description = createMovieDto.Description;
            movie.Screenwriter = createMovieDto.Screenwriter;
            movie.Title_pl = createMovieDto.Title_pl;

            _context.SaveChanges();
            return;
        }

        private Movie GetMovie(int movieId)
        {
            var movie = _context.Movies
                .Include(r => r.Rates)
                .Include(r => r.Comments)
                .FirstOrDefault(m => m.Id == movieId);

            if (movie is null)
                throw new NotFoundException("The movie is not found");

            return movie;
        }
    }
}
