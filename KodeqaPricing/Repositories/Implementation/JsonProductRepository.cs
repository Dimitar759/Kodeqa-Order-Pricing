using KodeqaPricing.Exceptions;
using KodeqaPricing.Models;
using KodeqaPricing.Repositories.Interface;
using System.Text.Json;

namespace KodeqaPricing.Repositories.Implementation;

public class JsonProductRepository : IProductRepository
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<JsonProductRepository> _logger;
    private IReadOnlyDictionary<string, Product>? _cache;

    public JsonProductRepository(IWebHostEnvironment env, ILogger<JsonProductRepository> logger)
    {
        _env = env;
        _logger = logger;
    }

    public async Task<Product> GetByIdAsync(string productId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(productId))
            throw new ValidationException("productId is required.");

        await EnsureLoadedAsync(ct);

        if (_cache!.TryGetValue(productId.Trim(), out var product))
            return product;

        throw new NotFoundException($"Product '{productId}' was not found.");
    }

    private async Task EnsureLoadedAsync(CancellationToken ct)
    {
        if (_cache != null) return;

        var path = Path.Combine(_env.ContentRootPath, "Data", "products.json");
        if (!File.Exists(path))
            throw new NotFoundException($"products.json not found at '{path}'.");

        _logger.LogInformation("Loading products from {Path}", path);

        var json = await File.ReadAllTextAsync(path, ct);

        var root = JsonSerializer.Deserialize<ProductsRoot>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        var products = root?.Products ?? new List<Product>();

        _cache = products
            .Where(p => !string.IsNullOrWhiteSpace(p.Id))
            .ToDictionary(p => p.Id, p => p, StringComparer.OrdinalIgnoreCase);
    }

    private sealed class ProductsRoot
    {
        public List<Product> Products { get; set; } = new();
    }
}
