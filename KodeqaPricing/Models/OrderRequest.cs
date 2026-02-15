namespace KodeqaPricing.Models
{
    public class OrderRequest
    {
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Country { get; set; } = string.Empty; 
    }
}
