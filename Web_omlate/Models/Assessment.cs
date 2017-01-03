using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_omlate.Models
{
    public class Assessment
    {
        public Assessment()
        {
            Points = 0;
            DateTime = DateTime.Now;
        }
        public int AssessmentID { get; set; }
        [Display(Name="Assignment Title")]
        [Required]
        public String AssessmentTitle { get; set; }
        
        [DataType(DataType.DateTime)]
        [Display(Name="Upload Time")]
        public DateTime DateTime { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Due Date")]
        [Required]
        public DateTime DueDate { get; set; }

        public int Points { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Assessment File")]
        public string FilePath { get; set; }

        [ScaffoldColumn(false)]
        public String FileType { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Note or Description")]
        public String Note { get; set; }

        public virtual OfferedCourse OfferedCourse { get; set; }
        public virtual ICollection<AssessmentSubmission> AssessmentSubmissions { get; set; }

    }
}