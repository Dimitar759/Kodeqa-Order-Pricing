using KodeqaPricing.Models;

namespace KodeqaPricing.Repositories.Interface
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(string productId, CancellationToken ct = default);
    }
}
