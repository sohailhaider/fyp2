using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_omlate.Models
{
    public class LectureResource
    {
        [Key]
        public int LectureResourceID { get; set; }

        [Required]
        [Display(Name = "File Name")]
        public string FileName { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Resource File")]
        public string FilePath { get; set; }

        [Display(Name="File Type")]
        public String ResourceType { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name="Upload Date")]
        public DateTime DateTime { get; set; }
        
        [Required]
        public int LectureScheduleID { get; set; }
        public virtual LectureSchedule LectureSchedule { get; set; }
    }
}