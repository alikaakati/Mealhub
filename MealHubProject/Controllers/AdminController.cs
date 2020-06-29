using MealHubProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MealHubProject.ViewModels;

namespace MealHubProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db;
        public AdminController()
        {
            db = new ApplicationDbContext();
        }
        // GET: Admin
        public ActionResult Index()
        {
            return RedirectToAction("GetRestaurants", "Admin");
        }
        [Route("Admin/GetRestaurants")]
        public ActionResult GetRestaurants()
        {
            return View();
        }
        [Route("Admin/ViewMenu/{id}")]
        public ActionResult ViewMenu(int id)
        {
            var res = db.Restaurants.Single(c => c.Id == id);
            var query = (from p in db.Products
                          join r in db.Restaurants on p.restaurantID equals r.Id
                          where r.Id == id
                          select p).ToList();
            ProductsListViewModel list = new ProductsListViewModel();
            list.Products = query;
            list.restaurant = res;
            return View(list);
        }
        [Route("Admin/AddUser")]
        public ActionResult AddUser()
        {
            return View();
        }

        [Route("Admin/DeleteUser/{rId}")]
        public ActionResult DeleteUser(int rId)
        {
            var r = db.Customers.Single(c => c.Id == rId);
            db.Customers.Remove(r);
            db.SaveChanges();
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }


        [Route("Admin/UpdateRestaurant")]
        public ActionResult UpdateRestaurant(Customer c)
        {
            var customer = db.Restaurants.Single(m => m.Username == c.Username);
            if (c.Phone != null && c.Phone != 0)
            {
                customer.Phone = c.Phone;

            }
            if (c.Address != null && c.Address != "")
            {
                customer.Address = c.Address;
            }
            if (c.Username != null && c.Username != "")
            {
                customer.Username = c.Username;
            }


            db.SaveChanges();

            return RedirectToAction("GetRestaurants", "Admin");
        }






        [Route("Admin/Orders")]
        public ActionResult Orders()
        {
            var orders = (from o in db.Orders
                          join r in db.Restaurants on o.RestaurantId equals r.Id
                          where o.RestaurantId == r.Id
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
                var restaurant = db.Restaurants.Single(c => c.Id == thisOrder.RestaurantId);
                j.customer = customer;
                j.order = orders[i];
                j.restaurant = restaurant;
                j.products = productsforthisorder;
                result.Add(j);
            }
            return View(result);
        }











        [Route("Admin/UpdateUser")]
        public ActionResult UpdateUser(Customer c)
        {
            var customer = db.Customers.Single(m => m.Username == c.Username);
            if (c.Phone != null && c.Phone != 0)
            {
                customer.Phone = c.Phone;

            }
            if (c.Address != null && c.Address != "")
            {
                customer.Address = c.Address;
            }
            if (c.Username != null && c.Username != "")
            {
                customer.Username = c.Username;
            }


            db.SaveChanges();

            return RedirectToAction("Customers","Admin");
        }

        [Route("Admin/EditUser/{rId}")]
        public ActionResult EditUser(int rId)
        {
            var customer = db.Customers.Single(c => c.Id == rId);

            return View(customer);
        }

        [Route("Admin/EditRestaurant/{rId}")]
        public ActionResult EditRestaurant(int rId)
        {
            var res = db.Restaurants.Single(c => c.Id == rId);

            return View(res);
        }




        [Route("Admin/DeleteRestaurant/{rId}")]
        public ActionResult DeleteRestaurant(int rId)
        {
            var r = db.Restaurants.Single(c => c.Id == rId);
            db.Restaurants.Remove(r);
            db.SaveChanges();
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }
        [Route("Admin/AddRestaurant")]
        public ActionResult AddRestaurant() {
            return View();
        }
        [Route("Admin/Customers")]
        public ActionResult Customers()
        {
            return View();
        }

    }
}