using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class scLink
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Select Course")]
        public int courseID { get; set; }

        [Required]
        [Display(Name = "Select Student")]
        public int studentID { get; set; }
    }
}