
using KodeqaPricing.Models;
using KodeqaPricing.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace KodeqaPricing.Controllers;


[ApiController]
[Route("api/pricing")]
public class PricingController : ControllerBase
{
    private readonly IPricingService _pricingService;

    public PricingController(IPricingService pricingService)
    {
        _pricingService = pricingService;
    }


    [HttpGet("calculate")]
    public async Task<ActionResult<PricingResponse>> Calculate(
        [FromQuery] string productId,
        [FromQuery] int quantity,
        [FromQuery] string country,
        CancellationToken ct)
    {
        var request = new OrderRequest
        {
            ProductId = productId,
            Quantity = quantity,
            Country = country
        };

        var response = await _pricingService.CalculatePriceAsync(request, ct);
        return Ok(response);
    }
}
