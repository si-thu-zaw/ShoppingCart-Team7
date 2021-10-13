﻿using Microsoft.AspNetCore.Mvc;
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
            List<Purchase> sortPurchases = Sort(purchases, id);

            List<PurchaseCodes> codes = (List<PurchaseCodes>)(from p in sortPurchases
                                                                     group p by new { p.ProductId, p.PurchaseDate } into grp
                                                                     select new PurchaseCodes()
                                                                     {
                                                                         PID_PDATE = grp.Key.ToString(),
                                                                         ActivationCodes = grp.Select(a => a.ActivationCode).ToList(),
                                                                         Quantity = grp.Select(a => a.ActivationCode).Count()
                                                                     }).ToList();

            ViewData["codes"] = codes;

            ViewData["purchases"] = sortPurchases;
            ViewData["products"] = products;
            ViewData["searchStr"] = "";

            return View();
        }

        public List<Purchase> Sort(List<Purchase> p, int id)
        {
            DateTime today = DateTime.Today;
            DateTime endMonth = DateTime.Today.AddDays(-30);
            DateTime endYear = DateTime.Today.AddYears(-1);


            if (id == 2)
            {
                var purchases =
                from pur in p
                where pur.PurchaseDate <= today  && pur.PurchaseDate >= endMonth
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
                where pur.PurchaseDate <= today && pur.PurchaseDate >= endYear
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

        public IActionResult Search(string searchStr)
        {
            if (searchStr == null)
            {
                searchStr = "";
            }
            string searchStrLower = searchStr.ToLower();
            string username = "charles";
            User Buyer = dbContext.Users.FirstOrDefault(u => u.UserName == username);

            List<Product> products = dbContext.Products.Where(x => x.ProductName.ToLower().Contains(searchStrLower)).ToList();
            List<Purchase> purchases = dbContext.Purchases.Where(x => x.UserId == Buyer.Id).ToList();
            List<Purchase> sortedPurchases = new List<Purchase>();
            foreach (Product p in products)
            {
                foreach (Purchase pur in purchases)
                {
                    if (p.Id==pur.ProductId)
                    {
                        sortedPurchases.Add(pur);
                    }
                }
            }
            List<PurchaseCodes> codes = (List<PurchaseCodes>)(from p in sortedPurchases
                                                              group p by new { p.ProductId, p.PurchaseDate } into grp
                                                              select new PurchaseCodes()
                                                              {
                                                                  PID_PDATE = grp.Key.ToString(),
                                                                  ActivationCodes = grp.Select(a => a.ActivationCode).ToList(),
                                                                  Quantity = grp.Select(a => a.ActivationCode).Count()
                                                              }).ToList();

            ViewData["codes"] = codes;

            ViewData["purchases"] = purchases;
            ViewData["products"] = products;
            ViewData["searchStr"] = "";


            return View("Index");
        }

    }
}