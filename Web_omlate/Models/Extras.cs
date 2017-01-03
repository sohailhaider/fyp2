using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_omlate.Models
{
    public class Extras
    {

        [Key]
        [Display(Name = "Attempt ID")]
        public int ExtraID { get; set; }
    }
}