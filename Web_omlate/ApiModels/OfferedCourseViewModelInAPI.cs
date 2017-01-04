using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_omlate.Models;

namespace Web_omlate.ApiModels
{
    public class OfferedCourseViewModelInAPI
    {
        public int OfferedCourseID { get; set; }

        public string OfferedByID { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }

        public String IsSuggested { get; set; }
        public Course Course { get; set; }

        public Decimal CreditHours { get; set; }
    }
}