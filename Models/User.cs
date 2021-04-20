using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace Reach.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%%First Name%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [Display(Name = " Name")]
        [Required(ErrorMessage = "Enter your First name")]
        [MinLength(2, ErrorMessage = "First name must be at least 2 characters")]
        public string Name { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%%Email%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [Display(Name = "Email Address")]
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%%Password%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [Required]
        [MinLength(8, ErrorMessage = "Password Must be at least 8 characters")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must be between 6 and 20 characters and contain one uppercase letter, one lowercase letter, one digit and one special character.")]

        public string Password { get; set; }

        // %%%%%%%%%%%%%%%%%%%%%%%List of Weddings %%%%%%%%%%%%%
        // public List<Participant> Guests { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%%List of wedding you user posted  %%%%%%%%%%%%%
        // public List<Event> MyPost { get; set; }

        // %%%%%%%%%%%%%%%%%%%%%%%Update time and date%%%%%%%%%%%%%
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        // %%%%%%%%%%%%%%%%%%%%%%%%%Will not be Mapped but will Confirm password%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [NotMapped]
        [Compare("Password", ErrorMessage = "Please ensure that the password confirmation matches the password")]
        [DataType(DataType.Password)]
        public string Confirm { get; set; }
    }
}