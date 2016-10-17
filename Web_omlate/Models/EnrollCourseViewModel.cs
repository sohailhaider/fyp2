using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Web_omlate.Models;

namespace Web_omlate.Models
{
    [NotMapped]
    public class EnrollCourseViewModel
    {
        public int OfferedCourseID { get; set; }
        [Display(Name = "Course Offered Date")]
        public DateTime StartingDate { get; set; }
        [Display(Name = "Course Finish Date")]
        public DateTime FinishDate { get; set; }
        [Display(Name = "Offered By")]
        public string InstructorName { get; set; }
        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }
        [Display(Name = "Course Title")]
        public string CourseTitle { get; set; }
        [Display(Name = "Course Category")]
        public string CategoryName { get; set; }
        [Display(Name = "Credit Hours")]
        public decimal CreditHours { get; set; }
    }
}