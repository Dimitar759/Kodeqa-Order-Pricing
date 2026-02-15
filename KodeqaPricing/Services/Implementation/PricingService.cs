using KodeqaPricing.Exceptions;
using KodeqaPricing.Models;
using KodeqaPricing.Repositories.Interface;
using KodeqaPricing.Services.Interface;

namespace KodeqaPricing.Services.Implementation;

public class PricingService : IPricingService
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<PricingService> _logger;

    public PricingService(IProductRepository productRepository, ILogger<PricingService> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<PricingResponse> CalculatePriceAsync(OrderRequest request, CancellationToken ct = default)
    {
        ValidateRequest(request);

        var product = await _productRepository.GetByIdAsync(request.ProductId, ct);

        var subtotal = request.Quantity * product.Price;

        var discountPct = CalculateDiscountPct(request.Quantity, subtotal);
        var discountAmount = RoundMoney(subtotal * discountPct);
        var subtotalAfterDiscount = RoundMoney(subtotal - discountAmount);

        var taxRate = GetTaxRate(request.Country);
        var taxAmount = RoundMoney(subtotalAfterDiscount * taxRate);

        var finalPrice = RoundMoney(subtotalAfterDiscount + taxAmount);

        _logger.LogInformation(
            "Calculated pricing: product={ProductId}, qty={Qty}, country={Country}, final={Final}",
            product.Id, request.Quantity, request.Country, finalPrice);

        return new PricingResponse
        {
            ProductId = product.Id,
            ProductName = product.Name,
            Quantity = request.Quantity,
            UnitPrice = product.Price,
            Country = request.Country.Trim().ToUpperInvariant(),
            Subtotal = RoundMoney(subtotal),
            Discount = new DiscountInfo
            {
                Percentage = discountPct,
                Amount = discountAmount
            },
            SubtotalAfterDiscount = subtotalAfterDiscount,
            Tax = new TaxInfo
            {
                Country = request.Country.Trim().ToUpperInvariant(),
                Rate = taxRate,
                Amount = taxAmount
            },
            FinalPrice = finalPrice
        };
    }

    private static void ValidateRequest(OrderRequest request)
    {
        if (request is null) throw new ValidationException("Request is required.");

        if (string.IsNullOrWhiteSpace(request.ProductId))
            throw new ValidationException("productId is required.");

        if (request.Quantity <= 0)
            throw new ValidationException("quantity must be greater than 0.");

        if (string.IsNullOrWhiteSpace(request.Country))
            throw new ValidationException("country is required (MK, DE, FR, USA).");

        var c = request.Country.Trim().ToUpperInvariant();
        if (c is not ("MK" or "DE" or "FR" or "USA"))
            throw new ValidationException("country must be one of: MK, DE, FR, USA.");
    }

    private static decimal CalculateDiscountPct(int quantity, decimal subtotal)
    {
        if (subtotal < 500m) return 0m;

        return quantity switch
        {
            >= 100 => 0.15m,
            >= 50 => 0.10m,
            >= 10 => 0.05m,
            _ => 0m
        };
    }

    private static decimal GetTaxRate(string country)
    {
        return country.Trim().ToUpperInvariant() switch
        {
            "MK" => 0.18m,
            "DE" => 0.20m,
            "FR" => 0.20m,
            "USA" => 0.10m,
            _ => throw new ValidationException("Unsupported country.")
        };
    }

    private static decimal RoundMoney(decimal value) =>
        Math.Round(value, 2, MidpointRounding.AwayFromZero);
}
