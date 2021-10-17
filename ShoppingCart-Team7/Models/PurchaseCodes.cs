using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart_Team7.Models
{
    public class PurchaseCodes
    {
        public string PID_PDATE { get; set; }
        public Guid PdtId { get; set; }
        public List<Guid> ActivationCodes { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public List<Guid> PurchaseIDs { get; set; }
    }
}
