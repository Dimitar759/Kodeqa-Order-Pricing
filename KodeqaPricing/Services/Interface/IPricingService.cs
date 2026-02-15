using KodeqaPricing.Models;

namespace KodeqaPricing.Services.Interface
{
    public interface IPricingService
    {
        Task<PricingResponse> CalculatePriceAsync(OrderRequest request, CancellationToken ct = default);
    }
}
