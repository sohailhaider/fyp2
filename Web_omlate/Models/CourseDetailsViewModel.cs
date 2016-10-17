using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_omlate.Models
{
    public class CourseDetailsViewModel
    {
        [Key]
        public int Id { get; set; }
        public Course Course { get; set; }
        public OfferedCourse OfferedCourse { get; set; }
        public List<LectureResource> LectureResources { get; set; }
        public ICollection<Assessment> Assessments { get; set; }
    }
}