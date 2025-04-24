# ERP Web Application

This is a Web Application focused on managing Purchase-to-Pay processes, including contracts and purchase invoices. The application provides a user-friendly interface for managing these business processes through master-detail pages. 

This applicatin is meant to be used in tandem with another Solution that uses AI to launch the web pages in this application and perform the CRUD operations on the data. [Here](https://github.com/MSFT-Innovation-Hub-India/CUA-ProcureToPay-Automation) is the GitHub Repo of that Solution.

## Features

### Contract Management

The application provides complete CRUD (Create, Read, Update, Delete) operations for contracts:

- **View Contracts**: Browse through a list of all available contracts with key information like Contract ID, Supplier, Dates, and Status
- **Create Contracts**: Add new contracts with header information and associated line items
- **Edit Contracts**: Modify existing contract details and their line items
- **Delete Contracts**: Remove contracts from the system
- **Contract Details**: Master-detail views that display contract headers with their associated line items

### Purchase Invoice Management

The application provides complete CRUD operations for purchase invoices:

- **View Invoices**: Browse through a list of all purchase invoices
- **Create Invoices**: Add new purchase invoices with header information and associated line items
- **Edit Invoices**: Modify existing invoice details and their line items
- **Delete Invoices**: Remove invoices from the system
- **Invoice Details**: Master-detail views that display invoice headers with their associated line items
- **Contract Reference**: Link purchase invoices to related contracts

## Database Structure

The application uses a relational database with the following core tables:

### Contract Tables
- **contract_headers**: Stores contract master data (Contract ID, Supplier, Dates, Amount, etc.)
- **contract_lines**: Stores contract line items (Items, Quantities, Prices, etc.)
- **contracts_by_id**: A view that joins contract headers and lines for easy access

### Purchase Invoice Tables
- **purchase_invoice_header**: Stores invoice master data (Invoice No., Supplier, Total Value, etc.)
- **purchase_invoice_lines**: Stores invoice line items (Items, Quantities, Unit Prices, etc.)

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server (local or Azure SQL)

### Database Setup

Before running the application, you need to set up the database:

1. Create a database named `cdcsampledb` in your SQL Server instance
2. Execute the SQL scripts provided in the `db_scripts/db-sql.sql` file to create the necessary tables and sample data

### Configuration

Update the connection string in `appsettings.json` to point to your database:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Initial Catalog=cdcsampledb;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=\"Active Directory Default\""
}
```

Replace `YOUR_SERVER` with your actual SQL Server name or address.

### Running the Application

1. Open the solution in Visual Studio
2. Build the solution
3. Run the application

## Development Notes

This ERP Web application was entirely built through vibe coding in GitHub Copilot Agent, showcasing the power of AI-assisted development for business applications. The application demonstrates:

- ASP.NET Core MVC architecture
- Entity Framework Core for data access
- Master-detail views for business document management
- Responsive design for better user experience

## Additional Information

- The database scripts include sample data for contracts to get you started quickly
- The application uses Entity Framework Core for data access
- The UI is built with Bootstrap for a responsive design
