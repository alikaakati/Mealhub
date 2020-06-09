using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MealHubProject.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public int Phone { get; set; }

        public virtual ApplicationUser restaurant { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
    }
}