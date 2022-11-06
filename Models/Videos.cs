using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JwtApp.Models
{
    public class Videos
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VideoId { get; set; }
        [Url]
        [Required]
        public string Url { get; set; }
    }
}
