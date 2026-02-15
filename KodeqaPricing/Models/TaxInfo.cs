namespace KodeqaPricing.Models
{
    public class TaxInfo
    {
        public string Country { get; set; } = string.Empty;
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
    }
}
