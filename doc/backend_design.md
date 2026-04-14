# Backend Design

This document outlines the backend architecture for clean, scalable, and maintainable code. It is designed around principles of clean architecture and includes:

- Shared Kernel: Contains shared domain logic across subdomains.
- Domain: Entities, aggregates, value objects, domain events, and domain services.
- Application Layer: API contracts, DTOs, application services, and mediators.
- Infrastructure Layer: Handles EF Core, SQL execution, and repositories.
- Presentation Layer: Controllers and API endpoints handling user interaction.

### Key Features
1. Dependency Injection for better testability.
2. Repository Base Class for CRUD and raw SQL queries.
3. Middleware pipeline including JWT Authentication.