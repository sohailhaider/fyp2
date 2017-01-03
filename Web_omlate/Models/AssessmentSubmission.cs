using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_omlate.Models
{
    public class AssessmentSubmission
    {
        [Key]
        public int Id { get; set; }

        [ScaffoldColumn(false)]
        public int AssessmentId { get; set; }
        [ScaffoldColumn(false)]
        public int LearnerId { get; set; }
        [DataType(DataType.Upload)]
        public string FilePath { get; set; }
        [DataType(DataType.DateTime)]
        public String  FileType { get; set; }
        
        public DateTime SubmissionTime { get; set; }
        public Assessment Assessment { get; set; }

        public virtual User Learner { get; set; }
    }
}