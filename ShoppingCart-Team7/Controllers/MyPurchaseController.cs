using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCart_Team7.Models;

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
            var data =
                from p in purchases
                from c in carts
                from pd in products
                where p.ProductId == pd.Id && pd.Id == c.ProductId
                select new
                {
                    q = c.Quantity
                };

            ViewData["purchases"] = purchases;
            ViewData["carts"] = carts;
            ViewData["products"] = products;
            ViewData["data"] = data;

            foreach (Purchase p in purchases)
            {

                Cart c = carts.FirstOrDefault(x => x.ProductId == p.ProductId);
            }

            return View();
        }
    }
}
