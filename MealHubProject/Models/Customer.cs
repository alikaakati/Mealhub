using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MealHubProject.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }


        [Required]
        [Display(Name = "Phone")]
        public int Phone { get; set; }

        
        public virtual ApplicationUser customer { get; set; }
        
        [Required]
        public string ApplicationUserId { get; set; }
    }
}