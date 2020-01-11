using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(0, 1439, ErrorMessage = "You need to enter a valid time !")]
        public int startTime { get; set; }

        [Required]
        [Display(Name = "Course Duration Time")]
        public int duration { get; set; }

        [Required]
        [Display(Name = "Teacher")]
        public int teacher { get; set; }

        [Display(Name = "Name")]
        [Required]
        public String name { get; set; }

        [Display(Name = "Classroom")]
        [Required]
        public String classroom { get; set; }

        [Display(Name = "Classroom Exam A")]
        [Required]
        public String classrooma { get; set; }

        [Display(Name = "Classroom Exam B")]
        [Required]
        public String classroomb { get; set; }

        [Display(Name = "Day of the week - (ex 1 = sunday)")]
        [Required]
        [Range(1, 7, ErrorMessage = "You need to enter a valid day of the week !")]
        public int day { get; set; }

        [Required]
        public DateTime aDate { get; set; }

        [Required]
        public DateTime bDate { get; set; }
    }
}