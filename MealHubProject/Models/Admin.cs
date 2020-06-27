using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MealHubProject.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public virtual ApplicationUser customer { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
    }
}