using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_omlate.Models
{
    public class ViewSubmissionViewModel
    {
        [Key]
        public int Id { get; set; }

        public int SubmissionId { get; set; }
        [Display(Name="Submission Time")]
        public DateTime SubmissionTime { get; set; }
        [Display(Name = "Submited By")]
        public String LearnerName { get; set; }
    }
}