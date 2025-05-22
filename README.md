# Order Management API

This is a Clean Architecture .NET 8 API built for the Smoothstack .NET Developer assignment. It manages orders, applies customer-based discounts, tracks order status
transitions, and computes order analytics. The solution includes unit-tested service logic, Swagger-documented APIs, and integration with a real MSSQL database 
using Dapper ORM.

---

## Features Implemented

### 1. Discounting Logic

* Applies tiered discounts based on customer segment:

  * VIP → 15%
  * New Customer → 10%
  * Regular (with >5 orders) → 5%
* Endpoint: `POST /api/order/apply-discount`

### 2. Order Status Tracking

* Order transitions are strictly validated:

  * Placed → Processing → Shipped → Delivered [0, 1, 2, 3]
* Invalid transitions are rejected with proper exceptions.
* Endpoint: `POST /api/order/update-status`

### 3. Order Analytics

* Computes:

  * Average order value
  * Average fulfillment time (in hours)
* Supports optional date filtering via query string.
* Endpoint: `GET /api/analytics/order-metrics?startDate=&endDate=`

---

## 🧪 Testing

* Unit tests written using xUnit, Moq, and FluentAssertions.
* Tested components:

  * `DiscountService` for all segment logic
  * `OrderStatusService` for transition rules
  * `OrderController` and `AnalyticsController` for endpoint behavior

To run tests:

```bash
cd OrderManagement.Tests
 dotnet test
```

---

## 🛠️ Tech Stack

* .NET 8
* ASP.NET Core Web API
* Dapper ORM
* SQL Server (LocalDB)
* Serilog (logging to console + file)
* Clean Architecture (Application, Domain, Infrastructure, API)
* Swagger (Swashbuckle)

---

## 🗄️ Database

* Uses LocalDB `(localdb)\MSSQLLocalDB`
* Default connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=OrderDb;Trusted_Connection=True;"
}
```

### Schema

* `Customers(Id, Name, Segment)`
* `Orders(Id, CustomerId, TotalAmount, Status, CreatedAt, DeliveredAt)`

### Sample Data (Preloaded)

* 3 customer segments (VIP, NewCustomer, Regular)
* Multiple orders across different status levels

---

## 🔧 Getting Started

1. Clone the repo:

```bash
git clone https://github.com/your-username/order-management-api.git
```

2. Set up DB:

* Use the `OrderDb.sql` file or run provided insert scripts

3. Run the API:

```bash
cd OrderManagement.API
 dotnet run
```

4. Open Swagger:

```
https://localhost:{PORT}/swagger
```

---

## 📂 Project Structure (Clean Architecture)

```
OrderManagementService
│
├── API                # Controllers & Swagger setup
├── Application        # Services, Interfaces, DTOs
├── Domain             # Entities and Enums
├── Infrastructure     # Dapper implementation, DB access
├── Tests              # Unit tests for services and controllers
```

---

## 📞 Contact

**Lucky Okorodiden**
.NET Developer
\[Ochukolucky2017@gmail.com or GitHub Profile ]

---

## Notes

* Designed with extensibility in mind: easily pluggable repositories or ORM
* DB operations use efficient Dapper queries
* Error handling and logging are in place
* Fully async, DI-compliant, production-style structure
# order-management-api
