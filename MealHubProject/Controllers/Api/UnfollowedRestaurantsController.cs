using MealHubProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
namespace MealHubProject.Controllers.Api
{
    public class UnfollowedRestaurantsController : ApiController
    {
        private ApplicationDbContext db;
        public UnfollowedRestaurantsController()
        {
            db = new ApplicationDbContext();
        }
        // GET API/RESTAURANTS
        public IEnumerable<Restaurant> GetRestaurants()
        {
            var current = User.Identity.GetUserId();
            var allrestaurants = db.Restaurants.ToList();
            var followedrestaurants = (from r in db.Restaurants
                         join f in db.Followed on r.ApplicationUserId equals f.restaurant
                         join c in db.Customers on f.customer equals c.ApplicationUserId
                         where c.ApplicationUserId == f.customer
                         && c.ApplicationUserId == current
                         && f.restaurant == r.ApplicationUserId               
                         select r).ToList();
            
            var notfollowed = allrestaurants.Except(followedrestaurants);
            return notfollowed;
        }
    }
}
