using MealHubProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MealHubProject.Controllers.Api
{
    public class CustomersController : ApiController
    {
        private ApplicationDbContext db;
        public CustomersController()
        {
            db = new ApplicationDbContext();
        }
        public IEnumerable<Customer> GetCustomers()
        {
            return db.Customers.ToList();
        }
        public Customer GetCustomer(int id)
        {
            var Customer = db.Customers.SingleOrDefault(c => c.Id == id);
            if (Customer == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else
            {
                return Customer;
            }
        }
        [HttpPost]
        public Customer CreateCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            else
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return customer;
            }
        }
        [HttpPut]
        public void UpdateCustomer(int Id, Customer customer)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var customerInDb = db.Customers.SingleOrDefault(c => c.Id == Id);
            if (customerInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else
            {
                customerInDb.Username = customer.Username;
                customerInDb.Phone = customer.Phone;

                db.SaveChanges();
            }

        }
        // DELETE API/RESTAURANT/{ID}
        [HttpDelete]
        public void DeleteCustomer(int Id)
        {
            var customerInDb = db.Customers.SingleOrDefault(c => c.Id == Id);
            if (customerInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else
            {
                db.Customers.Remove(customerInDb);
                db.SaveChanges();
            }

        }
    }
}
