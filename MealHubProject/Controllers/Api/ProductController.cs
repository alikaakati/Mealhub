using MealHubProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MealHubProject.Controllers.Api
{
    public class ProductController : ApiController
    {
        private ApplicationDbContext db;
        public ProductController()
        {
            db = new ApplicationDbContext();
        }
        public IEnumerable<Product> GetProducts()
        {
            return db.Products.ToList();
        }

        public Product GetProduct(int id)
        {
            var Product = db.Products.SingleOrDefault(c => c.restaurantID == id);
            if (Product == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else
            {
                return Product;
            }
        }
        [HttpPost]
        public Product CreateProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            else
            {
                db.Products.Add(product);
                db.SaveChanges();
                return product;
            }
        }
        [HttpPut]
        public void UpdateProduct(int Id, Product product)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var productInDb = db.Products.SingleOrDefault(c => c.Id == Id);
            if (productInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else
            {
                productInDb.Name = product.Name;
                productInDb.Category = product.Category;
                productInDb.Description = product.Description;
                productInDb.Price = product.Price;
                productInDb.Discount = product.Discount;
                db.SaveChanges();
            }

        }

        public void DeleteProduct(int Id)
        {
            var productInDb = db.Products.SingleOrDefault(c => c.Id == Id);
            if (productInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else
            {
                db.Products.Remove(productInDb);
                db.SaveChanges();
            }

        }

    }
}
