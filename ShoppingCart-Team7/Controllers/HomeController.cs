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
        public  DBContext dbContext;
        public HomeController(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
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
