namespace Web_omlate.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class User
    {
        public User()
        {
            OfferedCourses = new HashSet<OfferedCourse>();
            EnrolledCourses = new HashSet<LearnerEnroll>();
            Status = true;
        }
    
        [Required]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "First Name should be 5 to 30 letters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Last Name should be 5 to 30 letters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Educational Field")]
        public string Field { get; set; }

        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        [Display(Name = "Username")]
        [Required(ErrorMessage="Username is Required")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Username is too short")]
        [Remote("CheckUsername","User",ErrorMessage="Username is Already Taken")]
        public string Username { get; set; }

        [Required(ErrorMessage="Please Enter a Valid Password")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password is too short")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Email is too short")]
        [DataType(DataType.EmailAddress,ErrorMessage = "Invalid Email")]
        [Display(Name = "Email")]
        [Remote("CheckEmail", "User", ErrorMessage = "Email already Exists")]
        public string Email { get; set; }

        [StringLength(30, MinimumLength = 10, ErrorMessage = "Invalid Phone Number")]
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone No is Required")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please Enter Valid Phone Number")]
        public string PhoneNo { get; set; }
        
        [Display(Name = "User Type")]
        [ScaffoldColumn(false)]
        public string Type { get; set; }

        [Display(Name = "User Status")]
        [ScaffoldColumn(false)]
        public bool Status { get; set; }

        public virtual ICollection<OfferedCourse> OfferedCourses { get; set; }

        public virtual ICollection<LearnerEnroll> EnrolledCourses { get; set; }
    }
}
