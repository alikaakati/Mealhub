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
            return View();
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
        [Route("Admin/AddRestaurant")]
        public ActionResult AddRestaurant() {
            return View();
        }


    }
}