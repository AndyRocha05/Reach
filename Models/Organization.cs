using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace Reach.Models
{
    public class Organization
    {
        [Key]
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

         // %%%%%%%%%%%%%%%%%%%%%%%Update time and date%%%%%%%%%%%%%
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public List<Church> Churches { get; set; }
    }
}