using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MealHubProject.Models;

namespace MealHubProject.ViewModels
{
    public class OrderViewModel
    {
        public Customer customer { get; set; }
        public Restaurant restaurant { get; set; }
        public Order order { get; set; }
    }
}