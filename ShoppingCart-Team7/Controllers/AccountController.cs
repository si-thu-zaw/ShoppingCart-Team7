using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart_Team7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart_Team7.Controllers
{
    public class AccountController : Controller
    {
        private DBContext dbContext; 

        public AccountController(DBContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public IActionResult index()
        {
            return View();
        }

        public IActionResult Login(IFormCollection form)
        {
            string username = form["username"];
            string password = form["password"];

            HashAlgorithm sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(username + password));

            User user = dbContext.Users.FirstOrDefault(x =>
                x.UserName == username &&
                x.PasswordHash == hash
            );

            if (user == null)
            {
                ViewData["exist"] = "N";
                return View("Index");
            }

            Session session = new Session()
            {
                Valid = true
            };
            user.Sessions.Add(session);
            dbContext.SaveChanges();

            Response.Cookies.Append("SessionId", session.Id.ToString());
            Response.Cookies.Append("Username", user.UserName);
            
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {

            Response.Cookies.Delete("SessionId");
            Response.Cookies.Delete("Username");

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult NewUser()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
