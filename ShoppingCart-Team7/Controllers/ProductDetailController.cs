using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index(Guid Id)
        {
            // use GetAllDetails function to get info from BD products table(Ado.NET)
            //Product product = dbContext.Products.FirstOrDefault(x => x.Id == Id);
            //Product product = dbContext.Products.FirstOrDefault(x => x.Id.ToString().ToUpper() == ProductId);
            Product product = GetAllDetails(Id);
            ViewData["product"] = product;

            // get rating info from DB reviews table(LINQ)
            List<Review> reviewList = dbContext.Reviews.ToList();
            var ratings = reviewList.GroupBy(x => x.ProductId);
            int arating = 0;
            int arating_num = 0;
            foreach (var grp in ratings)
                if (grp.Key == Id)
                {
                    arating = (int)Math.Round(grp.Average(x => x.Rating));
                    arating_num = grp.Count();
                }

            ViewData["rating_num"] = arating_num;
            ViewData["rating"] = arating;
            ViewData["Recommendation"] = GetRecommendations(Id);

            // ger review number from DB reviews table

            return View();
        }

        // Get AllDetail funtion
        protected static readonly string connectionString = "Server=localhost;Database=ShoppingCartDB; Integrated Security=true";
        public static Product GetAllDetails(Guid Id)
        {
            Product product = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"select Products.Id, Products.ProductName, Products.Price, Products.Description, Products.ImageSrc, Products.Category from Products
                    where Products.Id = '" + Id + "'";

                Debug.WriteLine(sql);
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    product = new Product()
                    {
                        ProductName = (string)reader["ProductName"],
                        Price = (float)reader["Price"],
                        ImageSrc = (string)reader["ImageSrc"],
                        Category = (string)reader["Category"],
                        Description = (string)reader["Description"],
                        Id = (Guid)reader["Id"]
                    };
                };
            }
            return product;
        }

        // GetRecommendations function
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
