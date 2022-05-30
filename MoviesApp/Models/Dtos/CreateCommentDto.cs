using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Models.Dtos
{
    public class CreateCommentDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public int MovieId { get; set; }

    }

}
