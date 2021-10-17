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

        //Gets the list of Cart items, calculates the prices to be shown upon load 
        public IActionResult Index()
        {


            List<CartItems> CartList = new List<CartItems>();
            CartList = GetCart();
                
            // compute price of every item in cart
            var iter =
                from cart in CartList
                select new { itemsPrice = cart.Price * cart.Quantity };

            //compute total price in cart
            var totalPrice = 0f;
            foreach (var cart in iter)
            {
                totalPrice += cart.itemsPrice;
            }

            ViewData["totalprice"] = totalPrice;
            ViewData["cartlist"] = CartList;

            return View();
        }

        //Called by index page, determines the type of cart to call and creates a list of cart items
        public List<CartItems> GetCart()
        {
            string userid = GetUserOrSession();
            List<CartItems> CartList = new List<CartItems>();
            if (Request.Cookies["SessionId"] == null)
            {
                CartList = dbContext.TempCarts.Join(
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
            }
            else
            {
                CartList = dbContext.Carts.Join(
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
            }
            return CartList;
        }

        //Called from products' add to cart buttons, addes the clicked product to the user's or temporary cart
        [Route("addtocart/{id}")]
        public IActionResult AddToCart(string id)
        {
            string userid = GetUserOrSession();
            if (Request.Cookies["SessionId"] == null)
            {
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
                }
                else
                {
                    cart.Quantity++;
                }
            }
            else
            {
                Cart cart = dbContext.Carts.FirstOrDefault(x => x.UserId == Guid.Parse(userid) && x.ProductId == Guid.Parse(id));

                if (cart == null)
                {
                    dbContext.Carts.Add(new Cart
                    {
                        UserId = Guid.Parse(userid),
                        ProductId = Guid.Parse(id),
                        Quantity = 1
                    }
                    );
                }
                else
                {
                    cart.Quantity++;
                }
            }
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        //Removes clicked item from the user's or temporary cart
        [Route("removefromcart/{id}")]
        public IActionResult RemoveFromCart(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            string SessionID = Request.Cookies["SessionId"];
            if (SessionID == null)
            {
                
                TempCart cart = dbContext.TempCarts.FirstOrDefault(x => x.Id == Guid.Parse(id));
                dbContext.Remove(cart);
            }
            else 
            {
                Cart cart = dbContext.Carts.FirstOrDefault(x => x.Id == Guid.Parse(id));
                dbContext.Remove(cart);
            }
            dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        //Updates the item quantity in the user's or temporary cart
        [Route("changequantity")]
        public IActionResult ChangeQuantity(string id, int quantity)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            if (Request.Cookies["SessionId"] == null)
            {
                TempCart cart = dbContext.TempCarts.FirstOrDefault(x => x.Id == Guid.Parse(id));
                cart.Quantity = quantity;
            }
            else
            {
                Cart cart = dbContext.Carts.FirstOrDefault(x => x.Id == Guid.Parse(id));
                cart.Quantity = quantity;
            }         
            dbContext.SaveChanges();
            return RedirectToAction("Index");

        }

        //Called on login, moves all items from the temporary cart to the logged in user's cart
        public IActionResult CartLogin()
        {
            string userid = GetUserOrSession();
            string tempsession = Request.Cookies["tempSession"];
            List<TempCart> tempCarts = dbContext.TempCarts.Where(x => x.TempSessionId.ToString() == tempsession).ToList();
            foreach (TempCart item in tempCarts)
            {
                Cart existingCart = dbContext.Carts.FirstOrDefault(x => x.UserId.ToString() == userid && x.ProductId.ToString() == item.ProductId.ToString());

                if (existingCart == null)
                {
                    dbContext.Add(new Cart
                    {
                        UserId = Guid.Parse(userid),
                        Quantity = item.Quantity,
                        ProductId = item.ProductId
                    });
                }
                else
                {
                    existingCart.Quantity++;
                }
                
                dbContext.Remove(item);
            }
            dbContext.SaveChanges();

            Response.Cookies.Delete("tempSession");

            return RedirectToAction("Index", "Home");
        }

        //Returns the logged in user's userid, the temporary session id or issues a new temporary session id if none exists
        public string GetUserOrSession()
        {
            string user = Request.Cookies["SessionId"];
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

        //Creates a new temporary session id
        public string IssueTempSession()
        {
            string tempCookie = Guid.NewGuid().ToString();
            Response.Cookies.Append("tempSession", tempCookie);
            return tempCookie;
        }
        public IActionResult Checkout()
        {
            if (Request.Cookies["SessionId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            string userid = GetUserOrSession();
            List<Cart> cart = dbContext.Carts.Where(x => x.UserId == Guid.Parse(userid)).ToList();
            DateTime currentDate = DateTime.Now;
            foreach (Cart item in cart)
            {

                for (int i = 0; i < item.Quantity; i++)
                {
                    dbContext.Add(new Purchase
                    {
                        UserId = Guid.Parse(userid),
                        ProductId = item.ProductId,
                        PurchaseDate = currentDate,
                        ActivationCode = Guid.NewGuid()
                    }); ;

                }
                dbContext.Remove(item);
            }
            dbContext.SaveChanges();
            return RedirectToAction("Index", "MyPurchase");
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

            return RedirectToAction("Index");
        } 

        [Route("issuetemp")]
        public void IssueTest()
        {

            string SessionID = "820B2B6A-2286-48B6-BB91-08D98F8B36C7";
            Response.Cookies.Append("SessionId", SessionID);
        } */

    }
}
