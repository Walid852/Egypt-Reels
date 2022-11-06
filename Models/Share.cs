using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JwtApp.Models
{
    public class Share
    {
        [ForeignKey("Reel")]
        [Key]
        [Column(Order = 1)]
        public int ReelId { get; set; }
        [ForeignKey("UserModel")]
        [Key]
        [Column(Order = 2)]
        public string username { get; set; }

        public bool IsShare { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss tt}")]
        [Required]
        public System.DateTime? SharedDate { get; set; }
        [MaxLength(100)]
        public string MyOwnTitle { get; set; }


    }
}
