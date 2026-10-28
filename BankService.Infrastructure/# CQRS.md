# Domain Structure

## Bounded Contexts

```text
Customer Management
│
└── Customer

Banking
│
└── BankAccount

Lending
│
├── InstantLoan
└── HomeLoan
```

---

# Customer Management

## Aggregate Root

```csharp
Customer
```

### Purpose

Represents a person interacting with the bank and owning banking products.

### Properties

```text
CustomerId
FirstName
LastName
DateOfBirth
Email
MobileNumber
Address
Status
CreatedOnUtc
```

### Commands

```text
RegisterCustomer
UpdateCustomer
DeactivateCustomer
```

### Queries

```text
GetCustomerById
SearchCustomers
GetCustomerProfile
```

### Domain Events

```text
CustomerRegistered
CustomerUpdated
CustomerDeactivated
```

---

# Banking

## Aggregate Root

```csharp
BankAccount
```

### Purpose

Represents a customer's bank account and its onboarding documents.

### Properties

```text
Id
CustomerId
AccountNumber
CreditScore
Status
CreatedOnUtc
Documents
```

### Child Entities

```csharp
Document
```

### Commands

```text
OpenAccount
AddDocument
VerifyDocument
RejectDocument
CloseAccount
```

### Queries

```text
GetAccountDetails
GetAccountDocuments
```

### Domain Events

```text
AccountOpened
DocumentUploaded
DocumentVerified
DocumentRejected
AccountClosed
```

---

# Lending

## Instant Loan

### Aggregate Root

```csharp
InstantLoan
```

### Purpose

Short-term unsecured loan.

A customer can apply without owning a bank account.

Maximum loan amount:

```text
$10,000
```

### Properties

```text
LoanId
CustomerId
RequestedAmount
ApprovedAmount
Status
CreatedOnUtc
```

### Commands

```text
ApplyForInstantLoan
ApproveInstantLoan
RejectInstantLoan
DisburseInstantLoan
RepayInstantLoan
```

### Queries

```text
GetInstantLoanDetails
GetCustomerInstantLoans
```

### Domain Events

```text
InstantLoanApplied
InstantLoanApproved
InstantLoanRejected
InstantLoanDisbursed
InstantLoanRepaymentReceived
```

---

# Home Loan

## Aggregate Root

```csharp
HomeLoan
```

### Purpose

Property financing product.

### Properties

```text
LoanId
CustomerId
AccountId
PropertyAddress
LoanAmount
OutstandingAmount
InterestRate
LoanTermMonths
Status
CreatedOnUtc
```

### Commands

```text
ApplyForHomeLoan
ApproveHomeLoan
RejectHomeLoan
DisburseHomeLoan
PayEmi
```

### Queries

```text
GetHomeLoanDetails
GetOutstandingAmount
GetCustomerHomeLoans
```

### Domain Events

```text
HomeLoanApplied
HomeLoanApproved
HomeLoanRejected
HomeLoanDisbursed
EmiPaid
```

---

# Solution Structure

```text
src
│
├── Api
│
├── Application
│   │
│   ├── CustomerManagement
│   │   ├── Commands
│   │   ├── Queries
│   │   └── DTOs
│   │
│   ├── Banking
│   │   ├── Commands
│   │   ├── Queries
│   │   └── DTOs
│   │
│   └── Lending
│       ├── InstantLoan
│       └── HomeLoan
│
├── Domain
│   │
│   ├── CustomerManagement
│   │   └── Customer
│   │
│   ├── Banking
│   │   ├── BankAccount
│   │   └── Document
│   │
│   └── Lending
│       ├── InstantLoan
│       └── HomeLoan
│
├── Infrastructure
│
└── SharedKernel
```

---

# Implementation Order

```text
1. Customer
2. BankAccount
3. InstantLoan
4. HomeLoan
5. CQRS Commands & Handlers
6. Queries
7. Persistence
8. Domain Events
9. API Endpoints
10. Semantic Kernel
11. MCP Server
```