using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoviesApi.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }

        public int MovieId { get; set; }

        [JsonIgnore]
        public virtual Movie Movie { get; set; }
    }

}
