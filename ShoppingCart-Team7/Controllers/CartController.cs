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

        /* public IActionResult Index()
        {
            string userid = "991A7B0D-E236-4E07-DE0B-08D98EF0D7D7";
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
            string userid = "991A7B0D-E236-4E07-DE0B-08D98EF0D7D7";
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
            //List<Item> cart = (List<Item>)
            //List<string> addCartItem = new List<string>;

            //var addCartItem = dbContext.Carts.FirstOrDefault(
            //    cart => cart.Id == ShoppingId);

            //addCartItem = new addToCart
            //{
            //    CartID = sh
            //};

            return RedirectToAction("Index");
        } */

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

        public string IssueTempSession()
        {
            string tempCookie = Guid.NewGuid().ToString();
            Response.Cookies.Append("tempSession", tempCookie);
            return tempCookie;
        }

        public string GetUserOrSession()
        {
            string user = Request.Cookies["SessionID"];
            if (user == null)
            {
                user = Request.Cookies["tempSession"];
                if (user == null)
                {
                    user = IssueTempSession();
                }
            }
            else
            {
                Session currentSession = dbContext.Sessions.FirstOrDefault(x => x.Id == Guid.Parse(user));
                user = currentSession.UserId.ToString();
            }
            return user;
        }

        [Route("issuetemp")]
        public void IssueTest()
        {
            string SessionID = "820B2B6A-2286-48B6-BB91-08D98F8B36C7";
            Response.Cookies.Append("SessionID", SessionID);
        }

        public IActionResult GetTempCart()
        {

        }

        public IActionResult Index()
        {
            string userid = GetUserOrSession();
            List<CartItems> anotherCartList = new List<CartItems>();
            anotherCartList = dbContext.TempCarts.Join(
                dbContext.Products,
                Cart => Cart.ProductId,
                Product => Product.Id,
                (Cart, Product) => new CartItems
                {
                    ProductName = Product.ProductName,
                    ProductImg = Product.ImageSrc,
                    Quantity = Cart.Quantity,
                    Price = Product.Price,
                    User = Cart.TempSessionId.ToString(),
                    CartID = Cart.Id.ToString()
                }
                ).Where(x =>
                    x.User.Equals(userid)
                ).ToList();

            // compute price of every item in cart
            var iter =
                from cart in anotherCartList
                select new { itemsPrice = cart.Price * cart.Quantity };

            //compute total price in cart
            var totalPrice = 0f;
            foreach (var cart in iter)
            {
                totalPrice += cart.itemsPrice;
            }

            ViewData["totalprice"] = totalPrice;
            ViewData["whatisthis"] = anotherCartList;

            return View();
        }

        [Route("addtocart/{id}")]
        public IActionResult AddToCart(string id)
        {
            string userid = GetUserOrSession();
            TempCart cart = dbContext.TempCarts.FirstOrDefault(x => x.TempSessionId == Guid.Parse(userid) && x.ProductId == Guid.Parse(id));

            if (cart == null)
            {
                dbContext.TempCarts.Add(new TempCart
                {
                    TempSessionId = Guid.Parse(userid),
                    ProductId = Guid.Parse(id),
                    Quantity = 1
                }
                );

                dbContext.SaveChanges();
            }
            else
            {
                cart.Quantity++;

                dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
