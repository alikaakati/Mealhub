using MealHubProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MealHubProject.ViewModels
{
    public class OrderCustomersViewModel
    {
        public Customer customer { get; set; }
        public List<Restaurant> restaurants { get; set; }
        public List<Order> orders { get; set; }
    }
}