using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JwtApp.Models
{
    public class Likes
    {
        [ForeignKey("Reel")]
        [Key]
        [Column(Order = 1)]
        public int ReelId { get; set; }
        [ForeignKey("UserModel")]
        [Key]
        [Column(Order = 2)]
        public string username { get; set; }
        public bool IsLike { get; set; }

    }
}
