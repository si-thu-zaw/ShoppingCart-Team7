using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data.SqlClient;
using ShoppingCart_Team7.Models;
using Microsoft.Data.SqlClient;
using ShoppingCart_Team7.Controllers;

namespace ShoppingCart_Team7.Data
{
    public class DetailData
    {
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
    }
}
