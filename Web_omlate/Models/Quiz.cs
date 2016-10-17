using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web_omlate.Models
{
    public class Quiz
    {
        public Quiz()
        {
            Questions = new HashSet<QuizQuestion>();
            QuizAttempts = new HashSet<QuizAttempt>();
            QuizAttemptsCount = new HashSet<IsAttempted>();
        }

        [Key]
        [Display(Name ="QuizID")]
        public int QuizID { get; set; }
        [Required]
        [Display(Name ="Instructor")]
        public String InstructorID { get; set; }
        
        public virtual ICollection<QuizQuestion> Questions { get; set; }
        

        [Required]
        [ScaffoldColumn(false)]
        [Display(Name ="Offered Course ID")]
        public int offeredCourseID { get; set; }

        [Required]
        [Display(Name = "Quiz Title")]
        public String QuizTitle { get; set; }

        [Required]
        [Display(Name = "Quiz Deadline")]
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }

        [Required, Display(Name ="Duration"), Range(0, 1440)]
        public int Duration { get; set; }


        public virtual ICollection<QuizAttempt> QuizAttempts { get; set; }
        public virtual ICollection<IsAttempted> QuizAttemptsCount { get; set; }
    }
}