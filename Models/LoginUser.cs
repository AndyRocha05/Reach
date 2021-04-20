using System;
using System.ComponentModel.DataAnnotations;

namespace Reach.Models{
public class LoginUser
{
      // %%%%%%%%%%%%%%%%%%%%%%%Email%%%%%%%%%%%%%
    [Display(Name = "Log in Name")]
    [Required(ErrorMessage = "Enter your email")]
    [EmailAddress(ErrorMessage = "Enter a valid email address")]
    public string LoginEmail {get; set;}
      // %%%%%%%%%%%%%%%%%%%%%%%Password %%%%%%%%%%%%%
    [Display(Name = "Log in Password")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Enter a password")]
    public string LoginPassword { get; set; }
}
}