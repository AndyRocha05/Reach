using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace Reach.Models
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%%First Name%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [Display(Name = " AdminName")]
        [Required(ErrorMessage = "Enter your Username")]
        [MinLength(2, ErrorMessage = "First name must be at least 2 characters")]
        public string Name { get; set; }

        // %%%%%%%%%%%%%%%%%%%%%%%Password%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [Required]
        [Display(Name = " Admin Password")]
        [MinLength(4, ErrorMessage = "Password Must be at least 4 characters")]
        [DataType(DataType.Password)]

        public string Password { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        // %%%%%%%%%%%%%%%%%%%%%%%%%Will not be Mapped but will Confirm password%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [NotMapped]
        [Compare("Password", ErrorMessage = "Please ensure that the password confirmation matches the password")]
        [DataType(DataType.Password)]
        public string Confirm { get; set; }
    }
}