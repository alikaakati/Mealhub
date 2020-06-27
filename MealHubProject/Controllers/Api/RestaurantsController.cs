using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using MealHubProject.Models;
namespace MealHubProject.Controllers.Api
{
    public class RestaurantsController : ApiController
    {
        private ApplicationDbContext db;
        public RestaurantsController()
        {
            db = new ApplicationDbContext();
        }
        // GET API/RESTAURANTS
        public IEnumerable<Restaurant> GetRestaurants() 
        {
            return db.Restaurants.ToList();   
        }
        // GET API/RESTAURANT/{ID}
        public Restaurant GetRestaurant(int id)
        {
            var Restaurant = db.Restaurants.SingleOrDefault(c => c.Id == id);
            if (Restaurant == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else
            {
                return Restaurant;
            }
        }
        // Creating a customer
        // POST API/RESTAURANTS
        [HttpPost]
        public Restaurant CreateRestaurant(Restaurant restaurant)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            else
            {
                db.Restaurants.Add(restaurant);
                db.SaveChanges();
                return restaurant;
            }
        }
        // PUT API/CUSTOMER/{ID}
        [HttpPut]
        public void UpdateRestaurant(int Id , Restaurant restaurant)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var RestaurantInDb = db.Restaurants.SingleOrDefault(c => c.Id == Id);
            if(RestaurantInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else
            {
                RestaurantInDb.Username = restaurant.Username;
                RestaurantInDb.Phone = restaurant.Phone;

                db.SaveChanges();
            }

        }
        // DELETE API/RESTAURANT/{ID}
        [HttpDelete]
        public void DeleteRestaurant(int Id)
        {
            var RestaurantInDb = db.Restaurants.SingleOrDefault(c => c.Id == Id);
            if (RestaurantInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else
            {
                db.Restaurants.Remove(RestaurantInDb);
                db.SaveChanges();
            }

        }
    }


}
