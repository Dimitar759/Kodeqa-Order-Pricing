using System.Globalization;

namespace KodeqaPricing.Models
{
    public class PricingResponse
    {
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Country { get; set; } = string.Empty;

        public decimal Subtotal { get; set; }

        public DiscountInfo Discount { get; set; } = new();
        public decimal SubtotalAfterDiscount { get; set; }

        public TaxInfo Tax { get; set; } = new();
        public decimal FinalPrice { get; set; }
    }

}
