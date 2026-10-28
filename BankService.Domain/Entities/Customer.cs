using BankService.Domain.Common;
using BankService.Domain.Enums;
using BankService.Domain.Events;
using BankService.Domain.ValueObjects;

namespace BankService.Domain.Entities;

/// <summary>
/// Aggregate root representing a person interacting with the bank.
/// </summary>
public sealed class Customer : AggregateRoot
{
    public CustomerId CustomerId { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public DateOnly DateOfBirth { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string MobileNumber { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public CustomerStatus Status { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }

    private Customer()
    {
    }

    private Customer(
        CustomerId customerId,
        string firstName,
        string lastName,
        DateOnly dateOfBirth,
        string email,
        string mobileNumber,
        string address,
        DateTime createdOnUtc)
        : base(Guid.NewGuid())
    {
        CustomerId = customerId;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Email = email;
        MobileNumber = mobileNumber;
        Address = address;
        Status = CustomerStatus.Active;
        CreatedOnUtc = createdOnUtc;
    }

    public static Customer Register(
        string firstName,
        string lastName,
        DateOnly dateOfBirth,
        string email,
        string mobileNumber,
        string address,
        DateTime createdOnUtc)
    {
        var customer = new Customer(
            CustomerId.New(),
            firstName,
            lastName,
            dateOfBirth,
            email,
            mobileNumber,
            address,
            createdOnUtc);

        customer.RaiseDomainEvent(new CustomerRegisteredEvent(customer.CustomerId, customer.Id, createdOnUtc));
        return customer;
    }

    public void Update(
        string firstName,
        string lastName,
        DateOnly dateOfBirth,
        string email,
        string mobileNumber,
        string address,
        DateTime updatedOnUtc)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Email = email;
        MobileNumber = mobileNumber;
        Address = address;

        RaiseDomainEvent(new CustomerUpdatedEvent(CustomerId, Id, updatedOnUtc));
    }

    public void Deactivate(DateTime deactivatedOnUtc)
    {
        if (Status == CustomerStatus.Inactive)
        {
            return;
        }

        Status = CustomerStatus.Inactive;
        RaiseDomainEvent(new CustomerDeactivatedEvent(CustomerId, Id, deactivatedOnUtc));
    }

    public static Customer Restore(
        Guid id,
        CustomerId customerId,
        string firstName,
        string lastName,
        DateOnly dateOfBirth,
        string email,
        string mobileNumber,
        string address,
        CustomerStatus status,
        DateTime createdOnUtc)
    {
        return new Customer
        {
            Id = id,
            CustomerId = customerId,
            FirstName = firstName,
            LastName = lastName,
            DateOfBirth = dateOfBirth,
            Email = email,
            MobileNumber = mobileNumber,
            Address = address,
            Status = status,
            CreatedOnUtc = createdOnUtc
        };
    }
}
