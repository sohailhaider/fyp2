using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web_omlate.Models
{
    public class QuizAttempt
    {
        public QuizAttempt()
        {

        }

        [Key]
        [Display(Name = "Attempt ID")]
        public int AttemptID { get; set; }
        [Required]
        [Display(Name = "Answers")]
        public String Answers { get; set; }

        [Display(Name = "Marks")]
        public double Marks { get; set; }
        
        [Display(Name = "Attempt Time")]
        public DateTime AttemptTime { get; set; }

        [Display(Name = "Learner")]
        public String LearnerID { get; set; }

        [Required]
        [Display(Name = "Offered Course")]
        public int OfferedCourseID { get; set; }

        [Required]
        [Display(Name = "QuizID")]
        public int QuizID { get; set; }


        public virtual User Learner { get; set; }
        public virtual OfferedCourse OfferedCourse { get; set; }
        public virtual Quiz Quiz { get; set; }
    }
}