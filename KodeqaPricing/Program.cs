using KodeqaPricing.Exceptions;
using KodeqaPricing.Repositories.Implementation;
using KodeqaPricing.Repositories.Interface;
using KodeqaPricing.Services.Implementation;
using KodeqaPricing.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IProductRepository, JsonProductRepository>();
builder.Services.AddScoped<IPricingService, PricingService>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    var logger = context.RequestServices
        .GetRequiredService<ILoggerFactory>()
        .CreateLogger("GlobalExceptionHandler");

    try
    {
        await next();
    }
    catch (ValidationException ex)
    {
        logger.LogWarning(ex, "Validation error");
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
    catch (NotFoundException ex)
    {
        logger.LogWarning(ex, "Not found");
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Unhandled error");
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new { error = "Internal server error." });
    }
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
