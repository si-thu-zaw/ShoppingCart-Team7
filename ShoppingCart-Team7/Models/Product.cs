using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart_Team7.Models
{
    public class Product
    {
        public Product()
        {
            Id = new Guid();
        }

        // map to primary key
        public Guid Id { get; set; }

        // map to other columns in the table
        [Required]
        public string ProductName { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        [MaxLength(600)]
        public string Description { get; set; }
        [Required]
        public string ImageSrc { get; set; }
        [Required]
        public string Category { get; set; }

        // navigational property: 1-To-Many relationship with Review
        public virtual ICollection<Review> Reviews { get; set; }

        // navigational property: 1-To-Many relationship with Purchase
        public virtual ICollection<Purchase> Purchases { get; set; }

        // navigational property: 1-To-Many relationship with Cart
        public virtual ICollection<Cart> Carts { get; set; }
    }
}
