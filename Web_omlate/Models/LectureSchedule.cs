using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_omlate.Models
{
    public class LectureSchedule
    {
        public LectureSchedule()
        {
            LectureResources = new List<LectureResource>();
        }
        public int LectureScheduleID { get; set; }

        [Display(Name = "Lecture Date")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime LectureDate { get; set; }

        [Display(Name = "Lecture Time")]
        [DataType(DataType.Time)]
        [Required]
        public TimeSpan LectureTime { get; set; }
        [Required]
        public string StreamName { get; set; }
        public int OfferedCourseID { get; set; }
        public virtual OfferedCourse OfferedCourse { get; set; }
        public List<LectureResource> LectureResources { get; set; }
    }
}