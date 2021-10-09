using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart_Team7.Models
{
    public class Review
    {
        public Review()
        {
            Id = new Guid();
        }

        // map to primary key
        public Guid Id { get; set; }

        // map to other columns 
        [Required]
        [MaxLength(500)]
        public string Comments { get; set; }
        [Required]
        public float Rating { get; set; }
        [Required]
        public DateTime ReviewDate { get; set; }

        // navigational property: 1-to-1 relationship to Purchase
        public virtual Purchase Purchases { get; set; }
        
        // ensure foreign key is not null
        public virtual Guid ProductId { get; set; }
    }
}
