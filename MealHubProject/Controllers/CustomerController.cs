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

    [Authorize]
    public class CustomerController : Controller
    {
        private ApplicationDbContext db;
        public CustomerController()
        {
            db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }
        [Route("Customer/Index")]
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        [Route("Customer/Profile")]
        // GET: Customer
        public ActionResult Profile()
        {

            return View();
        }


        [Route("Customer/FollowedRestaurants")]
        // GET: Customer
        public ActionResult FollowedRestaurants()
        {
            var query = (from c in db.Customers
                           join f in db.Followed on c.ApplicationUserId equals f.customer
                           join r in db.Restaurants on f.restaurant equals r.ApplicationUserId
                           select r)
              .ToList();

            return View(query);
        }

        [Route("Customer/FollowedRestaurants/Details/{rUsername}")]
        public ActionResult Details(string rUsername)
        {
            var restaurant = db.Restaurants.Single(c => c.Username == rUsername);
            return Content("Phone" + restaurant.Phone);
        }

        [Route("Customer/OrderADelivery/{rUsername}")]
        public ActionResult OrderADelivery(string rUsername)
        {

            var UserId = User.Identity.GetUserId();
            var restaurant = db.Restaurants.SingleOrDefault(c => c.Username == rUsername);
            var user = db.Customers.SingleOrDefault(c => c.ApplicationUserId == UserId);
            Session["restaurant"] = restaurant.Id;
            OrderViewModel x = new OrderViewModel();
            x.restaurant = restaurant;
            x.customer = user;
            return View(x);
        }

            
        [Route("Customer/SendOrder")]
        [HttpPost]
        public ActionResult SendOrder(OrderViewModel o) 
        {
            var RestaurantId = (int)Session["restaurant"];
            var UserId = User.Identity.GetUserId();
            var user = db.Customers.SingleOrDefault(c => c.ApplicationUserId == UserId);
            var Restaurant = db.Restaurants.SingleOrDefault(m => m.Id == RestaurantId);
            Order dbOrder = new Order();
            
            
            dbOrder.RestaurantId = RestaurantId;
            dbOrder.CustomerId = user.Id;
            dbOrder.Details = o.order.Details;
            dbOrder.Address = o.order.Address;
            dbOrder.Status = "Pending";
            dbOrder.Total = 100;

            db.Orders.Add(dbOrder);
            db.SaveChanges();
            OrderViewModel newOrder = new OrderViewModel();
            newOrder.order = o.order;
            newOrder.restaurant = Restaurant;
            newOrder.customer = user;
            return View(newOrder);
        }
        [Route("Customer/ViewMyOrders")]
        public ActionResult ViewMyOrders()
        {
            var userAppUserId = User.Identity.GetUserId();
            var user = db.Customers.Single(c => c.ApplicationUserId == userAppUserId);
            var UserId = user.Id;
            var orders = (from o in db.Orders
                          join c in db.Customers on o.CustomerId equals c.Id 
                          where o.CustomerId == UserId
                          select o).ToList();
            var restaurants = (from r in db.Restaurants
                             join o in db.Orders on r.Id equals o.RestaurantId
                             select r).ToList();
            OrderCustomersViewModel MyOrders = new OrderCustomersViewModel();
            MyOrders.customer = user;
            MyOrders.orders = orders;
            MyOrders.restaurants = restaurants;
            return View(MyOrders);
        }
        

    }
    }