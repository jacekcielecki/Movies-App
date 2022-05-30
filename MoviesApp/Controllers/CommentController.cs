using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Models;
using MoviesApi.Models.Dtos;
using MoviesApi.Services;
using MoviesApp.Models.Dtos;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("api/movies/{movieId}/comment")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public ActionResult Create([FromRoute]int movieId, [FromBody]CreateCommentDto comment)
        {
            var newCommentId = _commentService.CreateComment(movieId, comment);
            return Created($"/api/Movies/{movieId}/comment/{comment.Id}", null);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<CommentDto>> GetAll([FromRoute]int movieId)
        {
            var comments = _commentService.GetAllComments(movieId);
            return Ok(comments);
        }

        [AllowAnonymous]
        [HttpGet("{commentId}")]
        public ActionResult<CommentDto> Get([FromRoute]int movieId, [FromRoute]int commentId)
        {
            var comment = _commentService.GetCommentById(movieId, commentId);
            return Ok(comment);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpDelete("{commentId}")]
        public ActionResult Delete([FromRoute]int movieId, [FromRoute]int commentId)
        {
            _commentService.DeleteComment(movieId, commentId);
            return NoContent();
        }
    }
}
