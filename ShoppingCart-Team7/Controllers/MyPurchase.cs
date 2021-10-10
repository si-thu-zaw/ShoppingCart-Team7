using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ShoppingCart_Team7.Models;
using System.Dynamic;

namespace ShoppingCart_Team7.Controllers
{
    public class MyPurchaseController : Controller
    {
        private DBContext dbContext; // dont forget to create ref

        public MyPurchaseController(DBContext dbContext) // dont forget to instantiate
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            List<Purchase> purchases = dbContext.Purchases.ToList();
            List<Cart> carts = dbContext.Carts.ToList();
            List<Product> products = dbContext.Products.ToList();

            ViewData["purchases"] = purchases;
            ViewData["carts"] = carts;
            ViewData["products"] = products;

            foreach (Purchase p in purchases)
            {

                Cart c = carts.FirstOrDefault(x => x.ProductId == p.ProductId);
            }

            return View();
        }

        
    }
}
