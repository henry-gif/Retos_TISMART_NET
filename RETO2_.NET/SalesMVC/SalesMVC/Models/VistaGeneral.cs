using System;
using System.Collections.Generic;

namespace SalesMVC.Models
{
    public partial class VistaGeneral
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int OrderId { get; set; }
        public DateTime OrderD { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryDescription { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
