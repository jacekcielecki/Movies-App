
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Models;
using MoviesApi.Models.Dtos;
using MoviesApi.Services;
using MoviesApp.Models;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("api/movies")]
    [Authorize]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<MovieDto> GetById([FromRoute] int id)
        {
            var Movie = _movieService.GetById(id);
            return Ok(Movie);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<MovieDto>> Get()
        {
            var Movies = _movieService.Get();
            return Ok(Movies);
        }

        [AllowAnonymous]
        [HttpGet("page")]
        public ActionResult<IEnumerable<MovieDto>> GetPageOfMovies([FromQuery]MovieQuery query)
        {
            var Movies = _movieService.GetPage(query);
            return Ok(Movies);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([FromBody] CreateMovieDto newMovie)
        {
            int id = _movieService.AddMovie(newMovie);
            return Created($"/api/Movies/{id}", null);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete([FromRoute] int id)
        {
            _movieService.Delete(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Update([FromRoute] int movieId, [FromBody] CreateMovieDto newMovie)
        {
            _movieService.Update(movieId, newMovie);
            return Ok();
        }
    }
}
