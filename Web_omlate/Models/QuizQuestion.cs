using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web_omlate.Models
{
    public class QuizQuestion
    {
        [Key]
        [Display(Name = "QuizQuestionID")]
        public int ID { get; set; }
        

        [Display(Name ="Question")]
        [Required]
        public String QuestionStatement { get; set; }
        
        [Display(Name ="Options")]
        public String Options { get; set; }

        [Display(Name = "Answer")]
        public String Answer { get; set; }
        public int QuizID { get; set; }

        public virtual Quiz Quiz { get; set; }
    }
}