using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MealHubProject.Models
{
    public class ProductsInOrder
    {
        public int Id { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }

    }
}