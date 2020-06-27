using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MealHubProject.Models;

namespace MealHubProject.ViewModels
{
    public class ProductsListViewModel
    {
        public List<Product> Products { get; set; }
        public Restaurant restaurant { get; set; }
    }
}