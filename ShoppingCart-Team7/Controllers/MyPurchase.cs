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

            List<PurchaseCodes> codes = (List<PurchaseCodes>)(from p in purchases
                                                                     group p by new { p.ProductId, p.PurchaseDate } into grp
                                                                     select new PurchaseCodes()
                                                                     {
                                                                         PID_PDATE = grp.Key.ToString(),
                                                                         ActivationCodes = grp.Select(a => a.ActivationCode).ToList(),
                                                                         Quantity = grp.Select(a => a.ActivationCode).Count()
                                                                     }).ToList();

            ViewData["codes"] = codes;

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
            List<Purchase> purchases = dbContext.Purchases.Where(x => x.UserId == Buyer.Id).ToList();
            List<Product> products = dbContext.Products.Where(x => x.ProductName.Contains(searchStr)).ToList();


            foreach (Product pdt in products)
            {
                purchases = purchases.Where(x => x.ProductId == pdt.Id).ToList();
            }

            ViewData["purchases"] = purchases;
            ViewData["products"] = products;
            ViewData["searchStr"] = searchStr;

            return View("Index");
        }

    }
}
