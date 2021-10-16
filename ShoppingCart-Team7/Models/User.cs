using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart_Team7.Models
{
    public class User
    {
        public User()
        {
            Id = new Guid();
            Purchases = new List<Purchase>();
            Sessions = new List<Session>();
            Carts = new List<Cart>();
        }

        // maps to primary key
        public Guid Id { get; set; }

        // maps to remaining columns in User
        [Required]
        public string UserName { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }

        // navigational property: 1-To-Many relationship with Session
        public virtual ICollection<Session> Sessions { get; set; }

        // navigational property: 1-To-Many relationship with Purchase
        public virtual ICollection<Purchase> Purchases { get; set; }

        // navigational property: 1-To-Many relationship with Cart
        public virtual ICollection<Cart> Carts { get; set; }
    }
}
