using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCart_Team7.Models;
using System.Diagnostics;

namespace ShoppingCart_Team7.Controllers
{
    public class RecommendationController : Controller
    {
        private DBContext dbContext;

        public RecommendationController(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            List<Purchase> AllPurchases = dbContext.Purchases.ToList();
            List<Review> AllReviews = dbContext.Reviews.ToList();
            List<Product> TopProdByPurchase = new List<Product>();
            List<Product> TopProdByRatings = new List<Product>();

            // To find top 5 products based on purchase count
            var PurchaseCnt = AllPurchases.GroupBy(x => x.ProductId)
                                             .OrderByDescending(x => x.Count())
                                             .Take(5);

            // Add top 5 products into TopProdByPurchase
            foreach (var product in PurchaseCnt)
            {
                TopProdByPurchase.Add(dbContext.Products.FirstOrDefault(x => x.Id == product.Key));
            }

            // To find top 5 products based on rating
            var Ratings = AllReviews.GroupBy(x => x.ProductId)
                                    .OrderByDescending(x => x.Average(x => x.Rating))
                                    .Take(5);

            // Add top 5 products into TopProdByRatings
            foreach (var product in Ratings)
            {
                TopProdByRatings.Add(dbContext.Products.FirstOrDefault(x => x.Id == product.Key));
            }

            ViewData["TopProdByPurchase"] = TopProdByPurchase;
            ViewData["TopProdByRatings"] = TopProdByRatings;

            return View();
        }
    }
}
