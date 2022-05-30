using AutoMapper;
using MoviesApi.Models;
using MoviesApi.Models.Dtos;
using MoviesApp.Models;
using MoviesApp.Models.Dtos;

namespace MoviesApi
{
    public class MoviesMappingProfile : Profile
    {
        public MoviesMappingProfile()
        {
            CreateMap<Movie, MovieDto>();

            CreateMap<CreateMovieDto, Movie>();

            CreateMap<CreateCommentDto, Comment>();

            CreateMap<Comment, CommentDto>();

            CreateMap<CreateUserDto, User>();

            CreateMap<User, UserDto>();

        }
    }
}
