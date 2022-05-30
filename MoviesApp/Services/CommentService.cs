using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Models;
using MoviesApi.Models.Dtos;
using MoviesApi.Exceptions;
using MoviesApp.Models.Dtos;

namespace MoviesApi.Services
{
    public interface ICommentService
    {
        int CreateComment(int movieId, CreateCommentDto newComment);
        IEnumerable<CommentDto> GetAllComments(int movieId);
        CommentDto GetCommentById(int movieId, int commentId);
        void DeleteComment(int movieId, int commentId);

    }

    public class CommentService : ICommentService
    {
        private readonly MoviesDbContext _context;
        private readonly IMapper _mapper;

        public CommentService(MoviesDbContext moviesDbContext, IMapper autoMapper)
        {
            _context = moviesDbContext;
            _mapper = autoMapper;
        }

        public int CreateComment(int movieId, CreateCommentDto newComment)
        {
            var movie = GetMovie(movieId);

            var comment = _mapper.Map<Comment>(newComment);
            comment.MovieId = movieId;

            _context.Comments.Add(comment);
            _context.SaveChanges();


            return (comment.Id);
        }

        public IEnumerable<CommentDto> GetAllComments(int movieId)
        {
            var movie = GetMovie(movieId);

            IEnumerable<Comment> comments = movie.Comments;
            if (comments is null)
                throw new NotFoundException("Comments not found");

            var response = _mapper.Map<IEnumerable<CommentDto>>(comments);
            return (response);
        }

        public CommentDto GetCommentById(int movieId, int commentId)
        {
            var movie = GetMovie(movieId);

            Comment comment = movie.Comments.FirstOrDefault(i => i.Id == commentId);
            if (comment is null)
                throw new NotFoundException("Comment not found");

            var response = _mapper.Map<CommentDto>(comment);
            return (response);
        }

        public void DeleteComment(int movieId, int commentId)
        {
            var movie = GetMovie(movieId);

            Comment comment = movie.Comments.FirstOrDefault(i => i.Id == commentId);
            if (comment is null)
                throw new NotFoundException("Comment not found");

            _context.Comments.Remove(comment);
            _context.SaveChanges();
        }

        private Movie GetMovie(int movieId)
        {
            var movie = _context
                .Movies
                .Include(r => r.Comments)
                .FirstOrDefault(i => i.Id == movieId);

            if (movie is null)
                throw new NotFoundException("Movie not found");

            return movie;
        }
    }
}
