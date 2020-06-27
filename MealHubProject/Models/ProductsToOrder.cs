using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MealHubProject.Models
{
    public class ProductsToOrder
    {
        public int Id { get; set; }
        public int ProductID { get; set; }
        public int CustomerID { get; set; }
        public int RestaurantID { get; set; }
    }
}