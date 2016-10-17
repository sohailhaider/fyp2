using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_omlate.Models
{
    public class LearnerEnroll
    {
        public LearnerEnroll()
        {
            Status = 1;
        }
        public int LearnerEnrollID { get; set; }

        [Required(ErrorMessage = "Please Select Enroll Date")]
        [Display(Name = "Course Enroll Date")]
        [DataType(DataType.Date)]
        public DateTime EnrollDate { get; set; }

        [Required(ErrorMessage = "Please Select Completion Date")]
        [Display(Name = "Course Completion Date")]
        [DataType(DataType.Date)]
        public DateTime CompletionDate { get; set; }

        [Display(Name = "Course Status")]
        public short Status { get; set; }

        [Required(ErrorMessage="No Learner Selected for Enrollment")]
        public string EnrolledLearnerID { get; set; }

        public virtual User EnrolledLearner { get; set; }

        [Required(ErrorMessage = "No Course Selected for Enrollment")]
        public int EnrolledCourseID { get; set; }

        public virtual OfferedCourse EnrolledCourse { get; set; }

    }
}