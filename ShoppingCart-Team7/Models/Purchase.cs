using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart_Team7.Models
{
    public class Purchase
    {
        public Purchase()
        {
            Id = new Guid();
        }

        // map to primary key
        public Guid Id { get; set; }
        
        // map to columns in the table
        [Required]
        public DateTime PurchaseDate { get; set; }
        [Required]
        public Guid ActivationCode { get; set; }

        // ensure foreign keys are not null
        public virtual Guid UserId { get; set; }
        public virtual Guid ProductId { get; set; }
    }
}
