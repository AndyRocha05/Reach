using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace Reach.Models
{
    public class Church
    {
        [Key]
        public int ChurchId { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%%Church Name %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [Display(Name = "Church Name")]
        [Required(ErrorMessage = "Enter Title of event")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
        public string ChurchName { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%%Time%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [Display(Name = "Pastors Name")]
        [Required(ErrorMessage = "Enter Title of event")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
        public string PastorsName { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%% Url%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        public string ImageUrl { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%% Address%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        public string Address { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%%City%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        public string City { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%%Zip%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        public int ZipCode { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%%State%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        public string State { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%%Description%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [Required(ErrorMessage="Please add a Description")]

        public string Description{ get; set;}
        // %%%%%%%%%%%%%%%%%%%%%%%Description%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        public bool Accept { get; set;} = false;

        // %%%%%%%%%%%%%%%%%%%%%%%Update time and date%%%%%%%%%%%%%
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        // %%%%%%%%%%%%%%%%%%%%%%%Gets the UserId %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        public int UserId { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%%Who posted the Church  %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        public User PostedBy { get; set;}
        // %%%%%%%%%%%%%%%%%%%%%%%Connect to OrganizationId %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        public int OrganizationId { get; set; }
        public Organization Owner { get; set; }
    }
}