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
            string userid = "A4533357-1B91-40AA-DA50-08D98D9D73B2";
            List<CartItems> anotherCartList = new List<CartItems>();
            anotherCartList = dbContext.Carts.Join(
                dbContext.Products,
                Cart => Cart.ProductId,
                Product => Product.Id,
                (Cart, Product) => new CartItems
                {
                    ProductName = Product.ProductName,
                    ProductImg = Product.ImageSrc,
                    Quantity = Cart.Quantity,
                    Price = Product.Price,
                    User = Cart.UserId.ToString(),
                    CartID = Cart.Id.ToString()
                }
                ).Where(x =>
                    x.User.Equals(userid)
                ).ToList();

            // compute price of every item in cart
            var iter =
                from cart in anotherCartList
                select new { itemsPrice = cart.Price*cart.Quantity};

            //compute total price in cart
            var totalPrice = 0f;
            foreach(var cart in iter)
            {
                totalPrice += cart.itemsPrice;
            }

            ViewData["totalprice"] = totalPrice;
            ViewData["whatisthis"] = anotherCartList;

            //foreach (var thingy in anotherCartList)
            //{
            //    Console.WriteLine(thingy.ProductName + " " + thingy.Price);
            //}

            return View();
        }

        [Route("addtocart/{id}")]
        public IActionResult AddToCart(string id)
        {
            string userid = "71d50ea1-419c-46dc-ef3a-08d98d0741f1";
            Cart cart = dbContext.Carts.FirstOrDefault(x => x.UserId == Guid.Parse(userid) && x.ProductId == Guid.Parse(id));
            
            if (cart == null)
            {
                dbContext.Carts.Add(new Cart
                {
                    UserId = Guid.Parse(userid),
                    ProductId = Guid.Parse(id),
                    Quantity = 1
                }
                ) ;

                dbContext.SaveChanges();
            }
            else
            {
                cart.Quantity++;

                dbContext.SaveChanges();
            }    

            return RedirectToAction("Index");
        }

        [Route("removefromcart/{id}")]
        public IActionResult RemoveFromCart(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            Cart cart = dbContext.Carts.FirstOrDefault(x => x.Id == Guid.Parse(id));

            dbContext.Carts.Remove(cart);

            dbContext.SaveChanges();

            return RedirectToAction("Index");
        }


        [Route("changequantity")]
        public IActionResult ChangeQuantity(string id, int quantity)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            Cart cart = dbContext.Carts.FirstOrDefault(x => x.Id == Guid.Parse(id));

            cart.Quantity = quantity;

            dbContext.SaveChanges();
            return RedirectToAction("Index");

        }

        //[Route("checkout/{id}")]
        //public IActionResult Checkout(string id)
        //{
        //    List<Cart>cart=dbContext.Carts.Where(x =>x.)
        //}
    }
}
