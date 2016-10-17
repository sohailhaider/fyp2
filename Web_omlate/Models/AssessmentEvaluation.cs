using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web_omlate.Models
{
    public class AssessmentEvaluation
    {
        public AssessmentEvaluation()
        {
            EvaluationDate = DateTime.Now;
        }
        [Column(Order=0)]
        [Key]
        public int AssessmentID { get; set; }

        [Key]
        [Column(Order=1)]
        public int LearnerEnrollID { get; set; }

        public int Points { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name="Instructor Feedback")]
        public String Feedback { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime EvaluationDate { get; set; }
        
        [DataType(DataType.Upload)]
        public byte[] File { get; set; }

        public virtual Assessment Assessment { get; set; }
        public virtual LearnerEnroll LearnerEnroll { get; set; }

    }
}