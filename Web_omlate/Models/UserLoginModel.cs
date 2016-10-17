using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_omlate.Models
{
    public class UserLoginModel
    {
        [Key]
        public int ID { get; set; }
        [Display(Name="Email")]
        [Required]
        public String Email { get; set; }

        [Display(Name = "Password")]
        [Required]
        public String Password { get; set; }
    }
}