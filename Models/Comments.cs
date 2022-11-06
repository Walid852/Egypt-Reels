using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JwtApp.Models
{
    public class Comments
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }
        [ForeignKey("Reel")]
        [Required(ErrorMessage = "ReelId is required.")]
        public int ReelId { get; set; }
        [ForeignKey("UserModel")]
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(70, MinimumLength = 10)]
        public string username { get; set; }
        [MaxLength(500)]
        public string Comment { get; set; }

    }
}
