using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class Grades
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Select Course")]
        public int courseID { get; set; }

        [Required]
        [Display(Name = "Select Student")]
        public int studentID { get; set; }

        [Display(Name = "Exam A Grade")]
        [Range(0, 100, ErrorMessage = "You need to enter a valid grade !")]
        public int? aGrade { get; set; }

        [Display(Name = "Exam B Grade")]
        [Range(0, 100, ErrorMessage = "You need to enter a valid grade !")]
        public int? bGrade { get; set; }
    }
}