using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Web_omlate.Models
{
    public class OfferedCourse
    {
        public OfferedCourse()
        {
            CoursesEnrolled = new HashSet<LearnerEnroll>();
            LectureSchedules = new List<LectureSchedule>();
        }
        [Key]
        public int OfferedCourseID { get; set; }

        [Required(ErrorMessage = "Please Select Start Date")]
        [Display(Name="Course Offered Date")]
        [DataType(DataType.Date)]
        public DateTime StartingDate { get; set; }

        [Required(ErrorMessage="Please Select Finish Date")]
        [Display(Name = "Course Finish Date")]
        [DataType(DataType.Date)]
        public DateTime FinishDate { get; set; }

        [Required(ErrorMessage = "Please Enter Credit Hours")]
        [Display(Name = "Credit Hours")]
        [Range(1.00,6.00,ErrorMessage="Credit Hours range from 1.00 to 6.00")]
        public decimal CreditHours { get; set; }

        [Display(Name="Enrolled Learners")]
        [ScaffoldColumn(false)]
        public int LearnerCount { get; set; }

        [Required]
        public string OfferedByID { get; set; }
        public virtual User OfferdBy { get; set; }
        public virtual Course Course { get; set; }

        public virtual ICollection<LearnerEnroll> CoursesEnrolled { get; set; }
        public virtual ICollection<LectureSchedule> LectureSchedules { get; set; }
        public virtual ICollection<Assessment> Assessments { get; set; }
    }
}
