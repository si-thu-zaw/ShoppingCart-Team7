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

        public IActionResult Index(int id)
        {
            string username = "charles";
            User Buyer = dbContext.Users.FirstOrDefault(u => u.UserName == username);
            List<Purchase> purchases = dbContext.Purchases.Where(x => x.UserId == Buyer.Id).ToList();
            List<Product> products = dbContext.Products.ToList();

            ViewData["purchases"] = Sort(purchases, id);
            ViewData["products"] = products;
            ViewData["searchStr"] = "";

            return View();
        }

        public List<Purchase> Sort(List<Purchase> p, int id)
        {
            DateTime d = DateTime.Today;

            if (id == 2)
            {
                var purchases =
                from pur in p
                where pur.PurchaseDate.Month <= d.Month && pur.PurchaseDate.Month >= d.Month - 1
                select pur;

                List<Purchase> purchaseList = new List<Purchase>();

                foreach (Purchase pc in purchases)
                {
                    purchaseList.Add(pc);
                }
                return purchaseList;
            }
            if (id == 3)
            {
                var purchases =
                from pur in p
                where pur.PurchaseDate.Year <= d.Year && pur.PurchaseDate.Year >= d.Year - 1
                select pur;

                List<Purchase> purchaseList = new List<Purchase>();

                foreach (Purchase pc in purchases)
                {
                    purchaseList.Add(pc);
                }
                return purchaseList;
            }

            return p;

        }

        public IActionResult Search(int id, string searchStr)
        {
            if (searchStr == null)
            {
                searchStr = "";
            }

            string username = "charles";
            User Buyer = dbContext.Users.FirstOrDefault(u => u.UserName == username);
            List<Product> products = dbContext.Products.Where(x => x.ProductName.Contains(searchStr)).ToList();
            List<Purchase> purchases = dbContext.Purchases.Where(x => x.UserId == Buyer.Id).ToList();

            foreach (Product pdt in products)
            {
                purchases = dbContext.Purchases.Where(x => x.ProductId == pdt.Id).ToList();
            }

            ViewData["purchases"] = purchases;
            ViewData["products"] = products;
            ViewData["searchStr"] = searchStr;

            return View("Index");
        }

    }
}
