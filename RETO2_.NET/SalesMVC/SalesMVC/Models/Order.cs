using System;
using System.Collections.Generic;

namespace SalesMVC.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public int CustomerFk { get; set; }
        public DateTime OrderD { get; set; }

        public virtual Customer CustomerFkNavigation { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
