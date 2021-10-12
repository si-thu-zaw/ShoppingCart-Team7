using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCart_Team7.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCart_Team7.Data;

namespace ShoppingCart_Team7.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private DBContext dbContext;

        public HomeController(ILogger<HomeController> logger, DBContext dbContext)
        {
            _logger = logger;
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
                                             .Take(4);

            // Add top 5 products into TopProdByPurchase
            foreach (var product in PurchaseCnt)
            {
                TopProdByPurchase.Add(dbContext.Products.FirstOrDefault(x => x.Id == product.Key));
            }

            // To find top 5 products based on rating
            var Ratings = AllReviews.GroupBy(x => x.ProductId)
                                    .OrderByDescending(x => x.Average(x => x.Rating))
                                    .Take(4);

            // Add top 5 products into TopProdByRatings
            foreach (var product in Ratings)
            {
                TopProdByRatings.Add(dbContext.Products.FirstOrDefault(x => x.Id == product.Key));
            }

            ViewData["TopProdByPurchase"] = TopProdByPurchase;
            ViewData["TopProdByRatings"] = TopProdByRatings;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ProductDetail(string ProductId)
        {
            Product product = DetailData.GetAllDetails(ProductId);
            ViewData["ProductId"] = ProductId;
            ViewData["product"] = product;


            List<Review> reviewList = dbContext.Reviews.ToList();
            var ratings = reviewList.GroupBy(x => x.ProductId);
            float arating = 0;
            foreach (var grp in ratings)
            {
                if (grp.Key.ToString() == ProductId)
                {
                    Debug.WriteLine("okok");
                    arating = grp.Average(x => x.Rating);
                }
                
                Debug.WriteLine($"{ grp.Key} { grp.Average(x => x.Rating)}");
            };

            ViewData["rating"] = arating;
            Debug.WriteLine(arating);

            return View();

            /*var iter =
                from re in reviewList
                where re.ProductId.Equals(ProductId)
                select re.Rating;

            foreach (var i in iter)
                Debug.WriteLine(i.Rating);

            /*var Ratings = AllReviews.GroupBy(x => x.ProductId)
                                    .OrderByDescending(x => x.Average(x => x.Rating))
                                    .Take(5);*/
        }
    }
}
