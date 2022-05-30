using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoviesApp.Models.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public int MovieId { get; set; }
    }
}

