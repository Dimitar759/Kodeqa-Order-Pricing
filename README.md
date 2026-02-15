# Kodeqa Order Pricing System – Technical Assignment

This repository contains my solution for the **Kodeqa Technologies – Software Developer (Reporting & ERP Development)** technical assignment.

## How to run
From the project root folder:

```bash
dotnet run
The API will start and expose the endpoint at:

http://localhost:5204

https://localhost:7014

Swagger UI (for testing):

/swagger
API Endpoint
GET /api/pricing/calculate

Example request:

/api/pricing/calculate?productId=PROD-001&quantity=55&country=MK
Test Case Results (manually calculated)
Product: PROD-001 – Premium Widget
Unit Price: 12.00 EUR


## Test Case 1

Input: productId=PROD-001, quantity=55, country=MK

Subtotal = 55 × 12 = 660.00

Discount = 10% (50–99 units, subtotal ≥ 500) = 66.00

Subtotal after discount = 594.00

Tax (MK 18%) = 106.92
Final Price = 700.92


## Test Case 2

Input: productId=PROD-001, quantity=100, country=DE

Subtotal = 100 × 12 = 1200.00

Discount = 15% (100+ units, subtotal ≥ 500) = 180.00

Subtotal after discount = 1020.00

Tax (DE 20%) = 204.00
Final Price = 1224.00


## Test Case 3

Input: productId=PROD-001, quantity=25, country=USA

Subtotal = 25 × 12 = 300.00

No discount (subtotal < 500)

Tax (USA 10%) = 30.00
Final Price = 330.00


## Bugs Fixed from Starter Code

Fixed tax calculation to apply after discounts

Fixed discount threshold to apply when subtotal ≥ 500 EUR

Corrected tiered discount logic:

10–49 → 5%

50–99 → 10%

100+ → 15%

Implemented missing product loading from products.json

Implemented response building according to the required JSON structure

Added validation, error handling, and logging
