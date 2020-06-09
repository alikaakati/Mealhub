using MealHubProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MealHubProject.ViewModels
{
    public class OrderListViewModel
    {
        public List<Customer> customers { get; set; }
        public Restaurant restaurant { get; set; }
        public List<Order> orders { get; set; }
    }
}