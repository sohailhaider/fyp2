using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web_omlate.Models
{
    public class IsAttempted
    {

        [Key]
        [Display(Name = "Attempt ID")]
        public int AttemptedID { get; set; }

        public String Username { get; set; }

        public int QuizID { get; set; }

        public DateTime AttemptedTime { get; set; }

        public virtual User Learner { get; set; }

        public virtual Quiz Quiz { get; set; }
    }
}