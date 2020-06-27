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
    public class ProductsToOrderController : ApiController
    {
        private ApplicationDbContext db;
        public ProductsToOrderController()
        {
            db = new ApplicationDbContext();
        }

        public List<Product> GetProducts()
        {
            var current = User.Identity.GetUserId();
            var customer = db.Customers.Single(c => c.ApplicationUserId == current);
            var query = (from p in db.ProductsToOrder
                         join c in db.Customers on p.CustomerID equals c.Id
                         join pr in db.Products on p.ProductID equals pr.Id
                         where c.Id == customer.Id
                         select pr).ToList();
            return query;
        }
    }
}
