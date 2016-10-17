using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_omlate.Models
{
    public class Course
    {

        public Course()
        {
            OfferedCourses = new HashSet<OfferedCourse>();
        }

        [Key]
        [Required(ErrorMessage="Please Enter Course Code")]
        [ScaffoldColumn(true)]
        [Display(Name = "Course Code")]
        [StringLength(6,MinimumLength=5,ErrorMessage="Course code should be 5 to 6 letters")]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        //[Remote("CheckCourseCode","Instructor",ErrorMessage="You have Already Offered Course with same Code")]
        public string CourseCode { get; set; }

        [Required(ErrorMessage = "Please Enter Course Title")]
        [Display(Name = "Course Title")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Course code should be 4 to 30 letters")]
        public string CourseTitle { get; set; }

        [Display(Name="Course Thumbnail")]
        [DataType(DataType.ImageUrl)]
        public string CourseImage { get; set; }

        [Required]
        [Display(Name="Course Category Name")]
        public int CourseCategoryID { get; set; }
        public virtual CourseCategory CourseCategory { get; set; }

        public virtual ICollection<OfferedCourse> OfferedCourses { get; set; }
    }
}