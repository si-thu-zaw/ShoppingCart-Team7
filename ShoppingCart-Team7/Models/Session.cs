using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart_Team7.Models
{
    public class Session
    {
        public Session()
        {
            Id = new Guid();
            SessionTime = DateTimeOffset.Now.ToUnixTimeSeconds();
        }

        // map to primary key
        public Guid Id { get; set; }

        // map to other columns
        [Required]
        public long SessionTime { get; set; }
        [Required]
        public bool Valid { get; set; }

        // ensure that foreign key is not null
        public virtual Guid UserId { get; set; }

    }
}
