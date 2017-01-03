using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web_omlate.Models
{
    public class Rule
    {
        [Key]
        [Display(Name = "Attempt ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RuleID { get; set; }
        public double Confidence { get; set; }
        public virtual String Drivers { get; set; }
        public virtual String Indicates { get; set; }
    }
}