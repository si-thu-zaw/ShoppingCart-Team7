using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCart_Team7.Models;

namespace ShoppingCart_Team7.Controllers
{
    public class GalleryController : Controller
    {
        private DBContext dbContext;
        
        public GalleryController(DBContext dBContext)
        {
            this.dbContext = dBContext;
        }

        public IActionResult Index(string searchstring)
        {
            List<Product> products = new List<Product>();
            if (searchstring == null)
            {
                searchstring = "";
                products = dbContext.Products.ToList();
            }
            else
            {
                products = dbContext.Products.Where(x =>
                    x.ProductName.Contains(searchstring) ||
                    x.Description.Contains(searchstring)
                    ).ToList();
            }
            ViewData["productlist"] = products;
            ViewData["searchstring"] = searchstring;
            return View();

        }

        
    }
}
