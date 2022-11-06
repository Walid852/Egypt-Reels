using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JwtApp.Models
{
    public class Reel
    {
        [Key]
        public int ReelId { get; set; }
        [ForeignKey("UserModel")]
        public string Username { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }

        [ForeignKey("Videos")]
        [Required]
        public int VideoId { get; set; }
        
        [ForeignKey("PropertiesOfReels")]
        [Required]
        public int propertyId { get; set; }

        public int NumberOfLikes { get; set; }
        public int NumberOfComments { get; set; }
        public int NumberOfShare { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss tt}")]
        [Required]
        public System.DateTime? CreatedDate { get; set; }
    }
}
