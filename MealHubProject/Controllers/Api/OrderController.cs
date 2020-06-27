using MealHubProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MealHubProject.Controllers.Api
{
    public class OrderController : ApiController
    {

        private ApplicationDbContext db;
        public OrderController()
        {
            db = new ApplicationDbContext();
        }
        public IEnumerable<Order> GetOrders()
        {
            return db.Orders.ToList();
        }
        public Order GetOrder(int id)
        {
            var Order = db.Orders.SingleOrDefault(c => c.Id == id);
            if (Order == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else
            {
                return Order;
            }
        }

        public void DeleteOrder(int Id)
        {
            var OrdertInDb = db.Orders.SingleOrDefault(c => c.Id == Id);
            if (OrdertInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else
            {
                db.Orders.Remove(OrdertInDb);
                db.SaveChanges();
            }

        }
    }
}
