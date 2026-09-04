# 750 Hours Tracker

750 Hours Tracker is an ASP.NET Core Web API built to support activity tracking and broader user, team, permission, subscription, and administrative workflows.

The project evolved over time into a structured backend application with authentication, authorization, role and permission management, activity logging, document handling, subscriptions, webhooks, database persistence, and third-party integrations.

## Core Features

- User authentication and JWT-based authorization
- Role and permission management
- Activity logging and categorization
- Team and user management
- Subscription and payment-related workflows
- Document and property management
- Admin and dashboard endpoints
- Webhook handling
- Relational data persistence with Entity Framework Core
- Swagger/OpenAPI documentation
- Global error handling
- Email integration
- External service integrations

## Tech Stack

- C#
- .NET 6
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- JWT Authentication
- AutoMapper
- Swagger / OpenAPI
- SendGrid
- Stripe
- Azure Blob Storage
- Bugsnag

## Architecture

The application is organized into separate layers and concerns, including:

- Controllers
- Services
- Repositories
- DTOs
- Models
- Middleware
- Persistence
- Permission Management
- AutoMapper Profiles
- Extensions
- Providers

This structure helps keep business logic, data access, API contracts, and infrastructure concerns separated and easier to maintain.
