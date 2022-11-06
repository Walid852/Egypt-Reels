using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JwtApp.Models
{
    public class PropertiesOfReels
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PropertyId { get; set; }
        [Required]
        public string Government { get; set; }

    }
}
