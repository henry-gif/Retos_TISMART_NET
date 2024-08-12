using System;
using System.Collections.Generic;

namespace SalesMVC.Models
{
    public partial class OrderDetail
    {
        public int OrderIdfk { get; set; }
        public int ProductIdfk { get; set; }
        public int Quantity { get; set; }

        public virtual Order OrderIdfkNavigation { get; set; } = null!;
        public virtual Product ProductIdfkNavigation { get; set; } = null!;
    }
}
