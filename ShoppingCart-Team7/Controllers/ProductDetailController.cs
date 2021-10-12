using Microsoft.AspNetCore.Mvc;
using ShoppingCart_Team7.Data;
using ShoppingCart_Team7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using ShoppingCart_Team7.Controllers;

namespace ShoppingCart_Team7.Controllers
{
    public class ProductDetailController : Controller
    {
        private DBContext dbContext;

        public ProductDetailController(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index(string ProductId)
        {
            Product product = GetAllDetails(ProductId);
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
        }

        protected static readonly string connectionString = "Server=localhost;Database=ShoppingCartDB; Integrated Security=true";
        public static Product GetAllDetails(string ProductId)
        {
            Product product = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"select Products.ProductName, Products.Price, Products.Description, Products.ImageSrc, Products.Category from Products
                    where Products.Id = '" + ProductId + "'";

                //string sql1 = @"select firstname from users where firstname = 'jack'";

                Debug.WriteLine(sql);
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Debug.WriteLine("okokok");
                    product = new Product()
                    {
                        ProductName = (string)reader["ProductName"],
                        Price = (float)reader["Price"],
                        ImageSrc = (string)reader["ImageSrc"],
                        Category = (string)reader["Category"],
                        Description = (string)reader["Description"]
                    };
                };
            }
            return product;
        }

        public IActionResult Details(Guid ID)
        {
            Product product = dbContext.Products.FirstOrDefault(x => x.Id == ID);
            ViewData["Product"] = product;
            ViewData["Recommendation"] = GetRecommendations(ID);
            return View();
        }
    }
}
