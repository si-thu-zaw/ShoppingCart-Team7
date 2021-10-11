using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCart_Team7.Models;
using ShoppingCart_Team7.Controllers;

namespace ShoppingCart_Team7.Controllers
{
    public class CartController : Controller
    {
        private DBContext dbContext;

        public CartController(DBContext dBContext)
        {
            this.dbContext = dBContext;
        }

        public IActionResult Index()
        {
            string userid = "71d50ea1-419c-46dc-ef3a-08d98d0741f1";
            List<CartItems> anotherCartList = new List<CartItems>();
            anotherCartList = dbContext.Carts.Join(
                dbContext.Products,
                Cart => Cart.ProductId,
                Product => Product.Id,
                (Cart, Product) => new CartItems
                {
                    ProductName = Product.ProductName,
                    Quantity = Cart.Quantity,
                    Price = Product.Price,
                    User = Cart.UserId.ToString()
                }
                ).Where(x =>
                    x.User.Equals(userid)
                ).ToList();
            ViewData["whatisthis"] = anotherCartList;

            //anotherCartList =
            //    from cart in dbContext.Carts
            //    join product in dbContext.Products on cart.ProductId equals product.Id
            //    select new { ProductName = product.ProductName, Quantity = cart.Quantity, Price = product.Price, User = cart.UserId.ToString() }
            //    where 


            //foreach (var thingy in anotherCartList)
            //{
            //    Console.WriteLine(thingy.ProductName + " " + thingy.Price);
            //}

            return View();
        }

        public IActionResult AddToCart()
        {
            return View();
        }
    }

    
}
