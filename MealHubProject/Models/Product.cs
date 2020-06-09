using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MealHubProject.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public int restaurantID { get; set; }
    }
}