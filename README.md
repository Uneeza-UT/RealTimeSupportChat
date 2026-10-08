# Real-Time Support Chat System API

A RESTful ASP.NET Core Web API for managing customer support tickets, real-time messaging, ticket assignments, file attachments, notifications, and user accounts.

The API uses ASP.NET Core SignalR to provide real-time communication between customers and support staff, while REST API endpoints are used for ticket management, message history, notifications, authentication, and other operations.



## Features

- User registration and login
- JWT authentication
- Role-based authorization
  - Customer
  - SupportManager
  - SupportAgent
- Customer support ticket management
- Ticket creation and management
- Support agent assignment and reassignment
- Ticket status management
- Real-time messaging using SignalR
- Message history
- Message deletion
- File attachments in messages
- Local file storage
- Real-time notifications
- Persistent notification history
- Read/unread notification management
- Swagger/OpenAPI documentation
- Input validation using FluentValidation
- Global exception handling
- SQL Server database with Entity Framework Core
- ASP.NET Core Identity
- Password reset and password change functionality
- Role-based and resource-based authorization



## User Roles

### Customer

- Create support tickets
- View their own tickets
- Send messages in their tickets
- View message history
- Send file attachments
- Delete their own messages
- View notifications
- Mark notifications as read
- Delete their own notifications
- Track ticket status



### SupportManager

- View all support tickets
- Assign tickets to support agents
- Reassign tickets
- View ticket details
- Receive notifications about new tickets
- Send messages where permitted
- Delete closed tickets



### SupportAgent

- View their assigned ticket details
- Send messages to customers in their assigned tickets
- View message history
- Send file attachments
- Delete support messages
- Change the status of assigned tickets
- Receive real-time messages and notifications



## Technologies

- ASP.NET Core Web API
- .NET 10
- C#
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- JWT Authentication
- SignalR
- FluentValidation
- AutoMapper
- Swagger / OpenAPI
- Resend
- Clean Architecture



## Architecture

The project follows Clean Architecture principles:


```text
JobManagement
│
├── API
│   ├── wwwroot
│   ├── Controllers
│   ├── Middleware
│   ├── Models
│   ├── Swagger
│   └── Program.cs
│
├── Core
│   ├── Application
│   │   ├── Contracts
│   │          ├── Email
│   │          ├── Identity
│   │          ├── Persistence
│   │          ├── Services
│   │   ├── DTOs
│   │   └── Exceptions
│   │   └── Mapping Profiles
│   │   └── Models
│   │   ├── Validations
│   │
│   └── Domain
│       ├── Entities
│       ├── Common
│       └── Enums
│
└── Infrastructure
    ├── Identity
    │   ├── Configurations
    │   ├── DbContext
    │   └── Models
    │   └── Services
    │
    └── Infrastructure       
    │   ├── Email Service
    │   ├── Hubs
    │   ├── Services
    │
    └── Persistence
        ├── Configurations
        ├── DatabaseContext
        └── Repositories

```



## Authentication

The API uses ASP.NET Core Identity for user management and JWT Bearer tokens for authentication.

After logging in, the client receives a JWT token which must be included in authenticated requests:

Authorization: Bearer <token>

Most API endpoints require authentication.



## Using Swagger

1. Use the **Login** endpoint in Swagger to log in with one of the demo accounts provided below in the Test Accounts section.
2. Copy the `token` value from the login response.
3. Click **Authorize** 🔒 at the top of the Swagger page.
4. Paste **only the token value** into the authorization field.
5. Do **not** include quotation marks (`" "`) or the `Bearer ` prefix.
6. Click **Authorize**, then **Close**.
7. You can now test the protected endpoints according to the user's role.

**Example:** If the login response contains:

```json
{
  "token": "eyJhbGciOiJIUzI1Ni..."
}
```

paste:

```text
eyJhbGciOiJIUzI1Ni...
```

into the Swagger authorization field.




## Authorization

Authorization is role-based and resource-based.

Users can only access resources they are authorized to access.

For example:

- Customers can only view their own tickets.
- Support agents can only view tickets assigned to them.
- Support managers can view all tickets.
- Customers can only send messages in tickets they created.
- Support agents and support managers can only send messages in tickets assigned to them.
- Users can only access their own notifications.
- Customers can only delete customer-side messages.
- Support staff can only delete support-side messages.



## Real-Time Messaging

Real-time communication is implemented using ASP.NET Core SignalR.

The SignalR hub is available at:

`/hubs/support-chat`

When a user connects to the hub, their JWT is used to identify their user account.

Messages are delivered to the intended recipient in real time.

The general flow is:

```text
Customer
   │
   │ Send Message
   ▼
ASP.NET Core API
   │
   ├── Save message to SQL Server
   │
   ├── Send SignalR message
   │
   └── Create notification
            │
            ▼
       Support Agent/Manager
```

SignalR is used for real-time delivery, while the REST API is used to retrieve existing message history.



## Message History

Messages are stored in SQL Server and can be retrieved through the API.

This allows users to load previous messages when opening a ticket.

The system uses both:

```text
REST API
    ↓
Load existing message history

SignalR
    ↓
Receive new messages in real time
```

This means messages remain available even if a user was offline when the message was sent.


# Real-Time Support Chat System API

A RESTful ASP.NET Core Web API for managing customer support tickets, real-time messaging, ticket assignments, file attachments, notifications, and user accounts.

The API uses **ASP.NET Core SignalR** to provide real-time communication between customers and support staff, while REST API endpoints are used for ticket management, message history, notifications, authentication, and other operations.

---

## Features

- User registration and login
- JWT authentication
- Role-based authorization
  - Customer
  - SupportAgent
  - SupportManager
- Customer support ticket management
- Ticket creation and management
- Support agent assignment and reassignment
- Ticket status management
- Real-time messaging using SignalR
- Message history
- Message deletion
- File attachments in messages
- Local file storage
- Real-time notifications
- Persistent notification history
- Read/unread notification management
- Password change
- Swagger/OpenAPI documentation
- Input validation using FluentValidation
- Global exception handling
- Custom logging
- SQL Server database with Entity Framework Core
- ASP.NET Core Identity
- Role-based and resource-based authorization

---

## User Roles

### Customer

- Create support tickets
- View their own tickets
- Send messages in their tickets
- View message history
- Send file attachments
- Delete their own messages
- View notifications
- Mark notifications as read
- Delete their own notifications
- Track ticket status

### SupportAgent

- View tickets assigned to them
- View ticket details
- Send messages to customers
- View message history
- Send file attachments
- Delete support messages
- Change the status of assigned tickets
- Receive real-time messages and notifications

### SupportManager

- View all support tickets
- Assign tickets to support agents
- Reassign tickets
- View ticket details
- Receive notifications about new tickets
- Send messages where permitted
- Manage tickets assigned to support staff
- Delete closed tickets

---

## Technologies

- ASP.NET Core Web API
- .NET 10
- C#
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- JWT Authentication
- SignalR
- FluentValidation
- AutoMapper
- Swagger / OpenAPI
- Clean Architecture

---

## Architecture

The project follows Clean Architecture principles:

```text
RealTimeSupportChat
│
├── API
│   ├── Controllers
│   ├── Middleware
│   ├── Models
│   └── Program.cs
│
├── Application
│   ├── DTOs
│   ├── Interfaces
│   ├── IServices
│   ├── Services
│   ├── Validations
│   ├── Mapping Profiles
│   └── Exceptions
│
├── Domain
│   ├── Entities
│   └── Enums
│
└── Infrastructure
    ├── Data
    │   ├── DbContext
    │   ├── Configurations
    │   └── Repositories
    │
    ├── Identity
    │   ├── DbContext
    │   ├── Configurations
    │   └── Models
    │
    ├── Hubs
    │   └── SupportChatHub
    │
    └── Services
        ├── ChatHubService
        ├── FileStorageService
        └── SignalRUserIdProvider
```

---

## Authentication

The API uses **ASP.NET Core Identity** for user management and **JWT Bearer tokens** for authentication.

After logging in, the client receives a JWT token which must be included in authenticated requests:

```text
Authorization: Bearer <token>
```

Most API endpoints require authentication.

### Using Swagger

1. Use the **Login** endpoint in Swagger.
2. Copy the `token` value from the login response.
3. Click **Authorize** 🔒 at the top of the Swagger page.
4. Paste **only the token value** into the authorization field.
5. Do **not** include quotation marks (`" "`) or the `Bearer ` prefix.
6. Click **Authorize**, then **Close**.
7. You can now test the protected endpoints according to the user's role.

Example login response:

```json
{
  "token": "eyJhbGciOiJIUzI1Ni..."
}
```

Paste only:

```text
eyJhbGciOiJIUzI1Ni...
```

into the Swagger authorization field.

---

## Authorization

Authorization is role-based and resource-based.

Users can only access resources they are authorized to access.

For example:

- Customers can only view their own tickets.
- Support agents can only view tickets assigned to them.
- Support managers can view all tickets.
- Customers can only send messages in tickets they created.
- Support agents can only send messages in tickets assigned to them.
- Users can only access their own notifications.
- Customers can only delete customer-side messages.
- Support staff can only delete support-side messages.

---

## Real-Time Messaging

Real-time communication is implemented using **ASP.NET Core SignalR**.

The SignalR hub is available at:

```text
/hubs/support-chat
```

When a user connects to the hub, their JWT is used to identify their user account.

Messages are delivered to the intended recipient in real time.

The general flow is:

```text
Customer
   │
   │ Send Message
   ▼
ASP.NET Core API
   │
   ├── Save message to SQL Server
   │
   ├── Send SignalR message
   │
   └── Create notification
            │
            ▼
       Support Agent
```

SignalR is used for real-time delivery, while the REST API is used to retrieve existing message history.




## Message History

Messages are stored in SQL Server and can be retrieved through the API.

This allows users to load previous messages when opening a ticket.

The system uses both:

```text
REST API
    ↓
Load existing message history

SignalR
    ↓
Receive new messages in real time
```

This means messages remain available even if a user was offline when the message was sent.




## Ticket Management

Support tickets contain information such as:

- Customer
- Subject
- Description
- Assigned support user
- Status
- Creation date
- Last updated date
- Messages

Tickets can move through different statuses during their lifecycle.

A ticket can only be closed after it has been resolved.

Closed tickets cannot have their status changed or receive new messages.




## Ticket Assignment

Support managers can assign tickets to support agents.

Tickets can also be reassigned when necessary.

When a ticket is assigned or reassigned, notifications are generated for the relevant users.

The system keeps previous messages in the ticket even when the assigned support agent changes.




## Notifications

The system creates notifications for important events, including:

- New support ticket
- Ticket assignment
- Ticket reassignment
- Ticket status changes
- New messages

Notifications are stored in the database so users can view their notification history.

Available notification operations include:

```text
GET    /api/notification
GET    /api/notification/unread
PATCH  /api/notification/{id}/read
DELETE /api/notification
```

Real-time notifications are also delivered through SignalR.




## File Attachments

Users can attach files to support messages.

Attachments are stored in the application's `wwwroot` directory.

The database stores information about each attachment, including:

- File name
- File path
- Content type
- Associated message

A message can contain text, attachments, or both.




## Validation

Request validation is implemented using **FluentValidation**.

Validation includes rules such as:

- Required fields
- Maximum field lengths
- Valid ticket information
- Message content or attachment requirement
- Valid ticket status transitions

Validation errors are returned as appropriate HTTP `400 Bad Request` responses.




## Database

The application uses **SQL Server** with Entity Framework Core.

The database contains entities for:

- Users and roles
- Tickets
- Messages
- Attachments
- Notifications

ASP.NET Core Identity is used for user and role management.





## Database Setup


This project uses SQL Server and Entity Framework Core.
The repository includes EF Core migrations required to create the database.

1. Create a SQL Server database or configure a SQL Server instance.
2. Update the connection string in `appsettings.json` or user secrets.
3. Open the project in Visual Studio.
4. Migrations are committed to GitHub.
5. After configuring the SQL Server connection string, run:

Select Persistence project as default project:
update-database -Context ApplicationDbContext

Select Identity project as default project:
update-database -Context ApplicationIdentityDbContext

6. Start the application.





## Prerequisites

- .NET 10 SDK
- SQL Server
- GitHub
- A Resend account and API key for email functionality



## Configuration

Before running the application, configure the following settings:

- SQL Server connection string
- JWT settings
- Resend API key



```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your-sql-server-connection-string"
  },
  "JwtSettings": {
    "Key": "your-secret-key",
    "Issuer": "your-issuer",
    "Audience": "your-audience"
  },
  "Resend": {
    "ApiKey": "your-resend-api-key"
  }
}
```

Sensitive values such as the JWT signing key and database credentials should be configured using **.NET User Secrets, environment variables, or deployment platform configuration** rather than committed to the repository.






## Running the Application


Clone the repository:

```bash
git clone <repository-url>
```

Navigate to the API project:

```bash
cd RealTimeSupportChat.Api
```

Restore dependencies:

```bash
dotnet restore
```

Apply database migrations:

```bash
dotnet ef database update
```

Run the application:

```bash
dotnet run
```





## Swagger / API Documentation

The API is documented using Swagger/OpenAPI.

Once the application is running, open:

```text
https://localhost:<port>/swagger
```

The Swagger interface can be used to test the REST API endpoints.

The project also includes a custom SignalR testing interface in Swagger for testing real-time messaging without a separate frontend application.






## Testing the API

Recommended testing flow:

1. Clone the repository.
2. Configure the SQL Server connection string.
3. Configure JWT settings.
4. Update database for both dbContexts
5. Start the API.
6. Open Swagger.
7. Register a customer and support user, or use seeded test accounts if configured.
8. Log in to obtain a JWT token.
9. Click **Authorize** in Swagger.
10. Enter the JWT token.
11. Create a support ticket as a customer.
12. Assign the ticket to a support agent using the support manager account.
13. Connect to the SignalR hub.
14. Send messages between the customer and support agent.
15. Test real-time message delivery.
16. Test message history.
17. Test file attachments.
18. Test ticket status changes.
19. Test notifications.





## Test Accounts

The following demo accounts are pre-configured through the application's database seeding configuration and can be used to test the different user roles.

### Customer
Email: customer@example.com
Password: Customer1234!

### SuportManager
Email: support.manager@example.com
Password: Manager1234!

### SupportAgent
Email: support.agent@example.com
Password: Agent1234!
 





## Email Notifications

Email notifications are implemented using Resend.

Emails are sent for events such as:

- Account registration
- Password change
- Password reset requests






## Error Handling

The API uses global exception handling middleware to provide consistent error responses.

Common HTTP status codes include:

- `200 OK`
- `201 Created`
- `204 No Content`
- `400 Bad Request`
- `401 Unauthorized`
- `403 Forbidden`
- `404 Not Found`





## Project Status

Backend API completed.

The project includes REST API functionality, real-time communication using SignalR, ticket management, authentication, authorization, notifications, message history, and file attachments.

The API can be consumed by a frontend application in the future.





## Implementation Highlights

- Clean Architecture
- Repository and service patterns
- ASP.NET Core Identity
- JWT authentication
- Role-based authorization
- Resource-based authorization
- Real-time communication using SignalR
- Real-time notifications
- Entity Framework Core with SQL Server
- FluentValidation
- AutoMapper
- Global exception handling middleware
- File attachment support
- Persistent message history
- Ticket assignment and reassignment
- Ticket status management
- Swagger/OpenAPI documentation

