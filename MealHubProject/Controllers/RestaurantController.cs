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
    [Authorize(Roles = "Restaurant")]
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
            var current = User.Identity.GetUserId();
            var restaurant = db.Restaurants.Single(c=>c.ApplicationUserId == current);
            var url = "https://localhost:44370/Restaurant/ViewMenu/" + restaurant.Id;
            return Redirect(url);
        }
        [Route("Restaurant/ViewMenu/{id}")]
        public ActionResult ViewMenu(int id)
        {
            var restaurant = db.Restaurants.SingleOrDefault(c => c.Id == id);
            if(restaurant.Id != id)
            {
                return Content("You cannot access this page");
            }
            var products = (from p in db.Products
                            join r in db.Restaurants on p.restaurantID equals r.Id
                            where r.Id == id
                            select p).ToList();
            ProductsListViewModel list = new ProductsListViewModel();
            list.Products = products;
            list.restaurant = restaurant;
            return View(list);
        }

        [Route("Restaurant/AddItem")]
        public ActionResult AddItem()
        {
            return View();
        }
        [Route("Restaurant/EditItem/{itemId}")]
        public ActionResult EditItem(int itemId)
        {
            var product = db.Products.Single(c=>c.Id == itemId);
            return View(product);
        }

        [Route("Restaurant/UpdateItem")]
        public ActionResult UpdateItem(Product newp)
        {
            var p = db.Products.Single(c=>c.Id == newp.Id);
            p.Name = newp.Name;
            p.Price = newp.Price;
            p.Category = newp.Category;
            p.Description = newp.Description;
            p.Discount = newp.Discount;
            db.SaveChanges();
            var current = User.Identity.GetUserId();
            var restaurant = db.Restaurants.Single(c => c.ApplicationUserId == current);
            var url = "https://localhost:44370/Restaurant/ViewMenu/" + restaurant.Id;
            return Redirect(url);
        }
        [Route("Restaurant/Profile")]
        public ActionResult Profile()
        {
            var current = User.Identity.GetUserId();
            var restaurant = db.Restaurants.Single(c=>c.ApplicationUserId == current);
            return View(restaurant);
        }
        [Route("Restaurant/UpdateProfile")]
        public ActionResult UpdateProfile(Restaurant c)
        {
            var current = User.Identity.GetUserId();
            var customer = db.Restaurants.Single(x => x.ApplicationUserId == current);
            if (c.Phone != null && c.Phone != 0)
            {
                customer.Phone = c.Phone;

            }
            if (c.Address != null && c.Address != "")
            {
                customer.Address = c.Address;
            }


            db.SaveChanges();
            return RedirectToAction("Profile", "Restaurant");

        }
        [Route("Restaurant/ViewOrders")]
        public ActionResult ViewOrders()
        {

            var current = User.Identity.GetUserId();
            var restaurant = db.Restaurants.Single(c => c.ApplicationUserId == current);
            var orders = (from o in db.Orders
                          join r in db.Restaurants on o.RestaurantId equals r.Id
                          where r.Id == restaurant.Id && o.RestaurantId == restaurant.Id
                          select o).ToList();
            var products = new List<Product>();
            List<OrderViewModel> result = new List<OrderViewModel>();

            for (int i = 0; i < orders.Count; i++)
            {
                var temp = orders[i].Id;
                var thisOrder = orders[i];
                var productsforthisorder =
                    (from pio in db.ProductsInOrder
                     join p in db.Products on pio.ProductID equals p.Id
                     join o in db.Orders on pio.OrderID equals o.Id
                     where o.Id == temp
                     select p
                    ).ToList();
                var j = new OrderViewModel();
                var customer = db.Customers.Single(c => c.Id == thisOrder.CustomerId);

                j.customer = customer;
                j.order = orders[i];
                j.restaurant = restaurant;
                j.products = productsforthisorder;
                result.Add(j);
            }
            return View(result);
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
        [Route("Restaurant/DeleteItem/{itemId}")]
        public ActionResult DeleteItem(int itemId)
        {
            var item = db.Products.Remove(db.Products.Single(c=>c.Id == itemId));
            db.SaveChanges();
            var current = User.Identity.GetUserId();
            var restaurant = db.Restaurants.Single(c => c.ApplicationUserId == current);
            var url = "https://localhost:44370/Restaurant/ViewMenu/" + restaurant.Id;
            return Redirect(url);
        }
        [Route("Restaurant/DoAddItem")]
        public ActionResult DoAddItem(Product p)
        {

            var current = User.Identity.GetUserId();
            var restaurant = db.Restaurants.Single(c=>c.ApplicationUserId==current);
            p.restaurantID = restaurant.Id;
            p.Discount = 0;
            db.Products.Add(p);
            db.SaveChanges();
            var url = "https://localhost:44370/Restaurant/ViewMenu/"+restaurant.Id;
            return Redirect(url);
        }

    }


}