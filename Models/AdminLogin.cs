using System;
using System.ComponentModel.DataAnnotations;

namespace Reach.Models{
public class AdminLogin
{
      // %%%%%%%%%%%%%%%%%%%%%%%Email%%%%%%%%%%%%%
    [Display(Name = "Admin Login Name")]
    [Required(ErrorMessage = "Enter your User Name")]
    public string LoginName {get; set;}
      // %%%%%%%%%%%%%%%%%%%%%%%Password %%%%%%%%%%%%%
    [Display(Name = "Admin Login Password")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Enter a password")]
    public string LoginPassword { get; set; }
}
}