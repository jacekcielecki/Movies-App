using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoviesApi.Models
{
    public class Rate
    {
        [Key]
        public int Id { get; set; }
        public int Stars { get; set; }

        public int MovieId { get; set; }
        [JsonIgnore]
        public virtual Movie Movie { get; set; }
    }
}
