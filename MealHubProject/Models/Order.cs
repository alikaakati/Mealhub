using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MealHubProject.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int RestaurantId { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public string Details { get; set; }
        public int Total { get; set; }
    }
}