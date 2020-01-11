using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MVC.Models
{
    public class User
    {

        [Key]
        public int Id { get; set; }

        [Display(Name = "Student ID Number")]
        [Range(10000000, 999999999, ErrorMessage = "You need to enter a valid ID !")]
        [Required]
        public int StudentID { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Password need to be between 5 to 50 chars")]
        public String Password { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public String Email { get; set; }

        [Display(Name = "Full Name")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Full Name need to be between 5 to 50 chars")]
        public String FullName { get; set; }

        public int Rank { get; set; }
    }
}