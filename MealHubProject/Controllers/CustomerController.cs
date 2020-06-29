using MealHubProject.Models;
using MealHubProject.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MealHubProject.Controllers
{

    [Authorize(Roles = "Customer")]
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
            var UserId = User.Identity.GetUserId();
            var customer = db.Customers.SingleOrDefault(c => c.ApplicationUserId == UserId);
            return RedirectToAction("MakeAnOrder", "Customer");
        }
        [Route("Customer/AvailableRestaurants")]
        // GET: Customer
        public ActionResult AvailableRestaurants()
        {
            var UserId = User.Identity.GetUserId();
            var customer = db.Customers.SingleOrDefault(c => c.ApplicationUserId == UserId);
            return View(customer);
        }

        [Route("Customer/Profile")]
        // GET: Customer
        public ActionResult Profile()
        {
            var current = User.Identity.GetUserId();
            var customer = db.Customers.Single(c=>c.ApplicationUserId == current);
            return View(customer);
        }

        [Route("Customer/UpdateProfile")]
        public ActionResult UpdateProfile(Customer c)
        {
            var current = User.Identity.GetUserId();
            var customer = db.Customers.Single(x =>x.ApplicationUserId == current);
            if(c.Phone != null && c.Phone != 0)
            {
                customer.Phone = c.Phone;

            }
            if(c.Address != null && c.Address != "")
            {
                customer.Address = c.Address;
            }
            

            db.SaveChanges();
            return RedirectToAction("Profile", "Customer");

        }
        [Route("Customer/MakeAnOrder")]
        public ActionResult MakeAnOrder()
        {
            var query = (from r in db.Restaurants
                         select r)
                        .ToList();

            return View(query);
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
        [Route("Customer/ViewMenu/{rId}")]
        public ActionResult ViewMenu(int rId) {
            var restaurant = db.Restaurants.Single(c=>c.Id==rId);
            var products = (from p in db.Products
                            join r in db.Restaurants on p.restaurantID equals r.Id
                            where r.Id == rId
                            select p).ToList();
            ProductsListViewModel result = new ProductsListViewModel();
            result.restaurant = restaurant;
            result.Products = products;
            return View(result);
        }
        [Route("Customer/MyOrders")]
        public ActionResult MyOrders()
        {
            var current = User.Identity.GetUserId();
            var user = db.Customers.Single(c => c.ApplicationUserId == current);
            var orders = (from o in db.Orders
                          join c in db.Customers on o.CustomerId equals c.Id
                          where c.Id == user.Id && o.CustomerId == user.Id
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
                j.customer = user;
                j.order = orders[i];
                var restaurant = db.Restaurants.Single(c=>c.Id == thisOrder.RestaurantId);
                j.restaurant = restaurant;
                j.products = productsforthisorder;
                result.Add(j);
            }
            return View(result);
        }
        [Route("Customer/CancelOrder/{oId}")]
        public ActionResult CancelOrder(int oId)
        {
            var order = db.Orders.Single(c => c.Id == oId);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("MyOrders","Customer");
        }
        [Route("Customer/FollowedRestaurants/Details/{rUsername}")]
        public ActionResult Details(string rUsername)
        {
            var res = db.Restaurants.Single(c => c.Username == rUsername);
            var query = (from p in db.Products
                         join r in db.Restaurants on p.restaurantID equals r.Id
                         where r.Username == rUsername
                         select p).ToList();
            ProductsListViewModel list = new ProductsListViewModel();
            list.Products = query;
            list.restaurant = res;
            return View(list);
        }
        

        [Route("Customer/ContinueToAddress")]
        public ActionResult ContinueToAddress()
        {
            return View();
        }
        [Route("Customer/SubmitOrder")]
        public ActionResult SubmitOrder(Order order)
        {
            var current = User.Identity.GetUserId();
            var customer = db.Customers.Single(c => c.ApplicationUserId == current);
            var query = (from p in db.ProductsToOrder
                         join c in db.Customers on p.CustomerID equals c.Id
                         join pr in db.Products on p.ProductID equals pr.Id
                         where c.Id == customer.Id
                         select pr).ToList();
            var total = 0;
            for (int i = 0; i < query.Count; i++)
            {
                total = total + query[i].Price;
            }
            Debug.WriteLine(query[0].Price);
            Order newOrder = new Order();
            newOrder.CustomerId = customer.Id;
            newOrder.RestaurantId = query[0].restaurantID;
            newOrder.Status = "Pending";
            newOrder.Total = total;
            newOrder.Details = order.Details;
            if (order.Address == null)
            {
                newOrder.Address = customer.Address;
            }
            else
            {
                newOrder.Address = order.Address;
            }
            db.Orders.Add(newOrder);
            db.SaveChanges();
            return RedirectToAction("SubmitItems", "Customer");
        }
        [Route("Customer/SubmitItems")]
        public ActionResult SubmitItems()
        {
            var current = User.Identity.GetUserId();
            var customer = db.Customers.Single(c => c.ApplicationUserId == current);
            var query = (from p in db.ProductsToOrder
                         join c in db.Customers on p.CustomerID equals c.Id
                         join pr in db.Products on p.ProductID equals pr.Id
                         where c.Id == customer.Id
                         select pr).ToList();
            var Order = db.Orders.OrderByDescending(c => c.Id).First(c => c.CustomerId == customer.Id);
            var OrderID = Order.Id;


            var anotherquery = (from p in db.ProductsToOrder
                                join c in db.Customers on p.CustomerID equals c.Id
                                join pr in db.Products on p.ProductID equals pr.Id
                                where c.Id == customer.Id
                                select p).ToList();
            var total = 0;
            for (int i = 0; i < query.Count; i++)
            {

                var prod = new ProductsInOrder();
                prod.OrderID = OrderID;
                prod.ProductID = query[i].Id;
                db.ProductsInOrder.Add(prod);
                db.SaveChanges();

            }

            for (int i = 0; i < anotherquery.Count; i++)
            {
                db.ProductsToOrder.Remove(anotherquery[i]);
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Customer");
        }

        [Route("Customer/Follow/{rId}")]
        public ActionResult Follow(int rID)
        {
            var current = User.Identity.GetUserId();
            var restaurant = db.Restaurants.Single(c => c.Id == rID);
            Following f = new Following();
            f.restaurant = restaurant.ApplicationUserId;
            f.customer = current;
            db.Followed.Add(f);
            db.SaveChanges();
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }
        [Route("Customer/Unfollow/{rId}")]
        public ActionResult Unfollow(int rId)
        {
            var current = User.Identity.GetUserId();
            var restaurant = db.Restaurants.Single(c => c.Id == rId);
            var x = restaurant.ApplicationUserId;
            var following = db.Followed.Single(c=>c.restaurant == x && c.customer == current);
            db.Followed.Remove(following);
            db.SaveChanges();
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }
        [Route("Customer/OrderADelivery/{rUsername}")]
        public ActionResult OrderADelivery(string rUsername)
        {

            var UserId = User.Identity.GetUserId();
            var restaurant = db.Restaurants.SingleOrDefault(c => c.Username == rUsername);
            var user = db.Customers.SingleOrDefault(c => c.ApplicationUserId == UserId);
            Session["restaurant"] = restaurant.Id;
            var query = (from p in db.Products
                         join r in db.Restaurants on p.restaurantID equals r.Id
                         where r.Username == rUsername
                         select p).ToList();
            OrderViewModel x = new OrderViewModel();
            x.restaurant = restaurant;
            x.customer = user;
            x.products = query;
            return View(x);
        }
        [Route("Customer/RemoveFromCart/{pId}")]
        public ActionResult RemoveFromCart(int pId)
        {
            var remove = db.ProductsToOrder.First(c => c.ProductID == pId);
            db.ProductsToOrder.Remove(remove);
            db.SaveChanges();
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }
        [Route("Customer/AddToCart/{rId}/{pId}")]
        public ActionResult AddToCart(int rId , int pId)
        {
            ProductsToOrder p = new ProductsToOrder();
            var current = User.Identity.GetUserId();
            var customer = db.Customers.Single(c => c.ApplicationUserId == current);
            var restaurant = db.Restaurants.Single(c=> c.Id == rId);
            var product = db.Products.Single(c=>c.Id == pId);
            var restaurantID = restaurant.Id;
            var customerID = customer.Id;
            var productID = product.Id;
            p.CustomerID = customerID;
            p.RestaurantID = restaurantID;
            p.ProductID = productID;
            db.ProductsToOrder.Add(p);
            db.SaveChanges();
  
        return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        [Route("Customer/PrintProducts")]
        public ActionResult PrintProducts()
        {
            return View();
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