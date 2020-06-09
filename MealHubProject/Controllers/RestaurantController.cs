using MealHubProject.Models;
using MealHubProject.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MealHubProject.Controllers
{
    public class RestaurantController : Controller
    {
        private ApplicationDbContext db;
        public RestaurantController()
        {
            db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }
        // GET: Restaurant
        [Route("Restaurant/Index")]
        public ActionResult Index()
        {
            return View();
        }
        [Route("Restaurant/ViewMenu")]
        public ActionResult ViewMenu()
        {
            return View();
        }


        [Route("Restaurant/UpdateMenu")]
        public ActionResult UpdateMenu()
        {
            return View();
        }


        [Route("Restaurant/ViewOrders")]
        public ActionResult ViewOrders()
        {

            var rAppUserId = User.Identity.GetUserId();
            var restaurant = db.Restaurants.Single(c => c.ApplicationUserId == rAppUserId);
            var rId = restaurant.Id;
            var orders = (from o in db.Orders
                         join r in db.Restaurants on o.RestaurantId equals r.Id
                         where r.Id == rId
                         select o).ToList();
            var customers = (from c in db.Customers
                          join o in db.Orders on c.Id equals o.CustomerId
                          select c).ToList();

            OrderListViewModel ordersList = new OrderListViewModel();
            ordersList.orders = orders;
            ordersList.customers = customers;
            ordersList.restaurant = restaurant;
            return View(ordersList);
        }
        [Route("Restaurant/AcceptOrder/{orderId}")]
        public ActionResult AcceptOrder(int orderId)
        {
            var o = db.Orders.Single(c => c.Id == orderId);
            o.Status = "Accepted";
            db.SaveChanges();
            return RedirectToAction("ViewOrders","Restaurant"); 
        }
        [Route("Restaurant/RefuseOrder/{orderId}")]
        public ActionResult RefuseOrder(int orderId)
        {
            var o = db.Orders.Single(c => c.Id == orderId);
            o.Status = "Refused";
            db.SaveChanges();
            return RedirectToAction("ViewOrders","Restaurant");
        }
    }


}