using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Models.Dtos
{
    public class MovieDto
    {
        [Key]
        public int Id { get; set; }
        public string Title_pl { get; set; }
        public string Title_en { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Screenwriter { get; set; }
        public string Country { get; set; }
        public DateTime Premiere { get; set; }
        public int Lenght { get; set; }
        public string Image { get; set; }

        public virtual List<Rate> Rates { get; set; }
        public virtual List<Comment> Comments { get; set; }
    }

}
