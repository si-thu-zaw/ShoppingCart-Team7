using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCart_Team7.Models;
using System.Diagnostics;

namespace ShoppingCart_Team7.Controllers
{
    public class ProductController : Controller
    {
        private DBContext dbContext;

        public ProductController(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(Guid ID)
        {
            Product product = dbContext.Products.FirstOrDefault(x => x.Id == ID);
            ViewData["Product"] = product;
            ViewData["Recommendation"] = GetRecommendations(ID);
            return View();
        }

        public List<Product> GetRecommendations(Guid Id)
        {
            Random r = new Random();
            List<Product> Recommendations = new List<Product>();
            List<Purchase> PastPurchases = new List<Purchase>();

            int SelectionCount = 4; // Number of recommendations to return

            // Find list of purchases with the same Product ID
            List<Purchase> SalesWithSameProductId = dbContext.Purchases.Where(x => x.ProductId == Id).ToList();

            // If no one has bought this item before, update selectionCount so it won't return a recommendation list
            if (SalesWithSameProductId.Count() == 0)
            {
                SelectionCount = 0;
                return Recommendations;
            }

            // Get distinct users who bought the same book before
            List<Guid> Users = (from purchase in SalesWithSameProductId
                                select purchase.UserId).Distinct().ToList();

            // Get all past purchases made by users who bought the same book
            foreach (Guid user in Users)
            {
                List<Purchase> UserPurchase = dbContext.Purchases.Where(x => x.UserId == user && x.ProductId != Id).ToList();

                // Append list of Purchases to
                PastPurchases.AddRange(UserPurchase);
            }

            // Get distinct Product Id from Past Purchases
            List<Guid> ProductIds = (from pur in PastPurchases
                                     select pur.ProductId).Distinct().ToList();

            // Change SelectionCount according to ProductIds count if ProductIds count < 4
            if (ProductIds.Count() < 4)
                SelectionCount = ProductIds.Count();

            // Generate recommendations
            List<int> BookIndex = new List<int>();

            // Choose n number of random books based on Selection count
            for (int i = 0; i < SelectionCount; i++)
            {
                bool check = true;
                while (check)
                {
                    int index = r.Next(ProductIds.Count());
                    if (!BookIndex.Contains(index))
                    {
                        BookIndex.Add(index);
                        check = false;
                    }
                }
            }

            // Add books into Recommendation List
            foreach (int i in BookIndex)
            {
                Recommendations.Add(dbContext.Products.FirstOrDefault(x => x.Id == ProductIds[i]));
            }

            return Recommendations;
        }
    }
}
