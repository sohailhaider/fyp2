using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_omlate.Models
{
    public class CourseCategory
    {
        public CourseCategory()
        {
            Courses = new HashSet<Course>();
        }
        public int CourseCategoryID { get; set; }

        [StringLength(30, MinimumLength = 4, ErrorMessage = "Invalid Course Category Name")]
        [Display(Name = "Course Category Name")]
        [Required]
        public string CategoryName { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}