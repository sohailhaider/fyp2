using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web_omlate.Models
{
    [NotMapped]
    public class OfferedCoursesViewModel
    {
        public int OfferedCourseID { get; set; }
        
        public string OfferedByID { get; set; }

        public Course Course { get; set; }
    }
}