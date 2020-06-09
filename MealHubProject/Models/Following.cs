using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MealHubProject.Models
{
    public class Following
    {
        public int Id { get; set; }
        public string restaurant { get; set; }
        public string customer { get; set; }
    }
}