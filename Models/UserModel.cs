using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JwtApp.Models
{
    public class UserModel
    {
        
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(70, MinimumLength = 10)]
        [Key]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{8,32}$", ErrorMessage = "Require that at least one digit appear anywhere in the string" +
            "and at least one lowercase letter appear anywhere in the string and at least one uppercase letter appear anywhere in the string and" +
            "at least one special character appear anywhere in the string and The password must be at least 8 characters long, but no more than 32")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email ID is Required")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(50)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Incorrect Email Format")]
        public string EmailAddress { get; set; }

        public string Role { get; set; }
        [Required(ErrorMessage = "Government is required.")]
        public string Government { get; set; }
        [Range(1, 120, ErrorMessage = "Age must be between 1-120 in years.")]
        public int Age { get; set; }
        /*[Url][Required]
         */
    }
}
